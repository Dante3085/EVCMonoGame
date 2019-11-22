using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using EVCMonoGame.src.input;
using EVCMonoGame.src.collision;
using EVCMonoGame.src.states;

namespace EVCMonoGame.src.scenes
{
    public enum EScene
    {
        MAIN_MENU,
        OPTIONS,
        SAND_CASTLES,
        INSIDE_CASTLE,
        GAME_OVER,
    }

    public class SceneManager
    {
        #region Fields
        private Game game;
        private SpriteBatch spriteBatch;
        private Dictionary<EScene, Scene> scenes;
        private Scene currentScene;

        // SceneTransition stuff
        private Scene nextScene;
        private Scene previousScene;
        private Texture2D sceneTransitionTexture;
        private Easer easer;
        private bool transitioning;
        private bool reverseTransitionFinished;

        // Global Updateables/Drawables (They don't belong to a specific Scene)
        private SpriteFont globalFont;
        private DebugTexts debugTexts;

        private StateManager stateManager;

        #endregion
        #region Properties
        // Alles was nicht an einzelnen Stellen(Methoden) übergeben werden kann,
        // weil nicht klar ist wo es benötigt wird, wird über Properties bereitgestellt.
        // Der Rest wird wie gesagt einfach bei Methodenaufrufen übergeben.
        public GraphicsDevice GraphicsDevice
        {
            get { return game.GraphicsDevice; }
        }

        public SpriteFont GlobalFont
        {
            get { return globalFont; }
        }

        public DebugTexts GlobalDebugTexts
        {
            get { return debugTexts; }
        }

        public ContentManager Content
        {
            get { return game.Content; }
        }

        #endregion
        #region Constructors
        public SceneManager(Game game, StateManager stateManager)
        {
            this.game = game;
            this.stateManager = stateManager;
            spriteBatch = new SpriteBatch(game.GraphicsDevice);

            nextScene = null;
            easer = new Easer(new Vector2(0, -1), new Vector2(255, -1), 500, Easing.SineEaseIn);
            transitioning = false;
            reverseTransitionFinished = false;

            debugTexts = new DebugTexts(new Vector2(100, 100));
            debugTexts.Entries.Add("MousePos:");

            scenes = new Dictionary<EScene, Scene>();
            scenes[EScene.SAND_CASTLES] = new Scene_DesertWithCastles(this);
            scenes[EScene.INSIDE_CASTLE] = new Scene_InsideCastle(this);
            scenes[EScene.GAME_OVER] = new Scene_GameOver(this);
            currentScene = previousScene = scenes[EScene.GAME_OVER];

			currentScene.OnEnterScene();
			currentScene.LoadContent(game.Content);

            CollisionManager.AddCollidable(GameplayState.PlayerOne, CollisionManager.playerCollisionChannel);
            CollisionManager.AddCollidable(GameplayState.PlayerOne, CollisionManager.obstacleCollisionChannel);
        }
        #endregion
        #region Methods
        public void LoadContent()
        {
            sceneTransitionTexture = game.Content.Load<Texture2D>("rsrc/backgrounds/blank");
            globalFont = game.Content.Load<SpriteFont>("rsrc/fonts/DefaultFont");
            debugTexts.LoadContent(game.Content);

			// Evtl überflüssig, da wir nur den Content vom aktuellen Level fetchen möchten
			// Wir nehmen die Ladezeit von paar ms beim level transition im Kauf
            //foreach (Scene s in scenes.Values)
            //{
            //    s.LoadContent(game.Content);
            //}
        }

        public void Update(GameTime gameTime)
        {
            // GameScreen Updating
            currentScene.Update(gameTime);

            // Global Updating
            if (transitioning) { UpdateTransition(gameTime); }
        }

        public void Draw(GameTime gameTime)
        {
            // GameScreen Drawing
            currentScene.Draw(gameTime, spriteBatch);

            // Global Drawing
            spriteBatch.Begin();

            if (transitioning) { DrawTransition(); }
            debugTexts.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        public void SceneTransition(EScene to)
        {

			if (!transitioning && currentScene != scenes[to])
			{
				if (!scenes.ContainsKey(to))
				{
					throw new ArgumentException(to + " is not known to the ScreenManager.");
				}

				currentScene.Pause();
				CollisionManager.CleanCollisonManager();

				nextScene = scenes[to];

				transitioning = true;
				easer.Start();

				//todo loading screen


			}
			else
			{

			}
		}

		public void TransitionToPreviousScreen()
        {
            nextScene = previousScene;
            transitioning = true;
            easer.Start();
        }

        private void UpdateTransition(GameTime gameTime)
        {
            easer.Update(gameTime);
            if (easer.IsFinished)
            {
                if (reverseTransitionFinished)
                {
                    reverseTransitionFinished = false;
                    transitioning = false;
                    easer.Reverse();
                }
                else
                {
                    previousScene = currentScene;
					previousScene.OnExitScene();

                    currentScene = nextScene;
					currentScene.Unpause();
					easer.Reverse();
                    easer.Start();
                    reverseTransitionFinished = true;


					nextScene.OnEnterScene();
					nextScene.LoadContent(game.Content);

					// Kollision für Players setzen
					//CollisionManager.AddCollidables(GameplayState.PlayerTwo);
					//CollisionManager.AddCollidables(GameplayState.PlayerThree);
					//CollisionManager.AddCollidables(GameplayState.PlayerFour);
					CollisionManager.AddCollidable(GameplayState.PlayerOne, CollisionManager.playerCollisionChannel);
					CollisionManager.AddCollidable(GameplayState.PlayerOne, CollisionManager.obstacleCollisionChannel);

				}
            }

        }

        private void DrawTransition()
        {
            // Achtung: Easer haben immer float Werte. Bei float denkt der Color Konstruktor allerdings
            // er bekommt einen Wert von 0.0 bis 1.0, also nach int casten.
            spriteBatch.Draw(sceneTransitionTexture, 
                game.GraphicsDevice.Viewport.Bounds, new Color(0, 0, 0, (int)easer.CurrentValue.X));
        }

        public Vector2 GetViewportCenter()
        {
            Vector2 viewportCenter = Vector2.Zero;
            viewportCenter.X += game.GraphicsDevice.Viewport.Width * 0.5f;
            viewportCenter.Y += game.GraphicsDevice.Viewport.Height * 0.5f;
            return viewportCenter;
        }

        public void Exit()
        {
            game.Exit();
        }

        public void GoToMainMenu()
        {
            CollisionManager.CleanCollisonManager();
            LoadingState.Load(stateManager, false, null, new BackgroundState(),
                                                           new MainMenuState());
        }

        public void StartNewGame()
        {
            CollisionManager.CleanCollisonManager();
            LoadingState.Load(stateManager, false, null, new BackgroundState(),
                                                           new GameplayState());
        }

        #endregion
    }
}
