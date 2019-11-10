using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using EVCMonoGame.src.collision;

using EVCMonoGame.src.input;
using EVCMonoGame.src.states;

namespace EVCMonoGame.src.scenes
{
    public enum EScene
    {
        MAIN_MENU,
        OPTIONS,
        DEBUG,
        DEBUG_2,
    }

    public class SceneManager
    {
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
        private FpsCounter fpsCounter;

        // Alles was nicht an einzelnen Stellen(Methoden) übergeben werden kann,
        // weil nicht klar ist wo es benötigt wird, wird über Properties bereitgestellt.
        // Der Rest wird wie gesagt einfach bei Methodenaufrufen übergeben.
        public GraphicsDevice GraphicsDevice
        {
            get { return game.GraphicsDevice; }
        }

        public SceneManager(Game game)
        {
            this.game = game;
            spriteBatch = new SpriteBatch(game.GraphicsDevice);

            nextScene = null;
            easer = new Easer(0, 255, 0, Easing.SineEaseIn);
            transitioning = false;
            reverseTransitionFinished = false;

            fpsCounter = new FpsCounter(Vector2.Zero, Color.White);

            scenes = new Dictionary<EScene, Scene>();
			scenes[EScene.DEBUG] = new CollisionDebugScene(this);
			scenes[EScene.DEBUG_2] = new ItemDebugScene(this);
			currentScene = previousScene = scenes[EScene.DEBUG];
			SceneTransition(EScene.DEBUG); // Debug
        }

        public void LoadContent()
        {
            //sceneTransitionTexture = game.Content.Load<Texture2D>("rsrc/backgrounds/blank");
            fpsCounter.LoadContent(game.Content);

            foreach (Scene s in scenes.Values)
            {
                s.LoadContent(game.Content);
            }
        }

        public void Update(GameTime gameTime)
        {
            // GameScreen Updating
            currentScene.Update(gameTime);

            // Global Updating
            // if (InputManager.OnKeyPressed(Keys.Escape)) { game.Exit(); }
            if (transitioning) { UpdateTransition(gameTime); }
            fpsCounter.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            // GameScreen Drawing
            currentScene.Draw(gameTime, spriteBatch);

            // Global Drawing
            spriteBatch.Begin();

            if (transitioning) { DrawTransition(); }
            fpsCounter.Draw(gameTime, spriteBatch);

			CollisionManager.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        public void SceneTransition(EScene to)
        {
            if (!scenes.ContainsKey(to))
            {
                throw new ArgumentException(to + " is not known to the ScreenManager.");
            }

			CollisionManager.CleanCollisonManager();
			nextScene = scenes[to];
			previousScene.LevelEndsEvent();
			nextScene.LevelStartsEvent();
			nextScene.LoadContent(game.Content); // Content Laden
			transitioning = true;
            easer.start();

		
			// Collision verpassen evtl auslagern in "Play"
			CollisionManager.AddCollidable(GameplayState.PlayerOne, CollisionManager.playerCollisionChannel);
			CollisionManager.AddCollidable(GameplayState.PlayerOne, CollisionManager.obstacleCollisionChannel);
			//CollisionManager.AddCollidables(GameplayState.PlayerTwo);
			//CollisionManager.AddCollidables(GameplayState.PlayerThree);
			//CollisionManager.AddCollidables(GameplayState.PlayerFour);

		}

        public void TransitionToPreviousScreen()
        {
            nextScene = previousScene;
            transitioning = true;
            easer.start();
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
                    easer.reverse();
                }
                else
                {
                    previousScene = currentScene;
                    currentScene = nextScene;
                    easer.reverse();
                    easer.start();
                    reverseTransitionFinished = true;
                }
            }

        }

        private void DrawTransition()
        {
            // Achtung: Easer haben immer float Werte. Bei float denkt der Color Konstruktor allerdings
            // er bekommt einen Wert von 0.0 bis 1.0, also nach int casten.
            //spriteBatch.Draw(sceneTransitionTexture, game.GraphicsDevice.Viewport.Bounds, new Color(0, 0, 0, (int)easer.CurrentValue));
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
    }
}
