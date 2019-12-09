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

using EVCMonoGame.src.scenes.tutorial;
using EVCMonoGame.src.scenes.desert;
using EVCMonoGame.src.scenes.train;
using EVCMonoGame.src.scenes.castle;
using EVCMonoGame.src.utility;

namespace EVCMonoGame.src.scenes
{
    public enum EScene
    {
        MAIN_MENU,
        OPTIONS,
        SAND_CASTLES,
        INSIDE_CASTLE,

        GAME_OVER,
        REST_ROOM,

        TUTORIAL_ROOM_1,
        TUTORIAL_ROOM_2,
        TUTORIAL_ROOM_3,
        TUTORIAL_ROOM_4,
        TUTORIAL_ROOM_5,

        DESERT_ROOM_1,
        DESERT_ROOM_2,
        DESERT_ROOM_3,
        DESERT_ROOM_4,
        DESERT_ROOM_5,

        TRAIN_ROOM_1,
        TRAIN_ROOM_2,
        TRAIN_ROOM_3,
        TRAIN_ROOM_4,
        TRAIN_ROOM_5,

        CASTLE_ROOM_1,
        CASTLE_ROOM_2,
        CASTLE_ROOM_3,
        CASTLE_ROOM_4,
        CASTLE_ROOM_5,

        BARREN_FALLS_ENTRANCE,
        BARREN_FALLS,
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

        private static List<EScene> roomSeqence;

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

            scenes = new Dictionary<EScene, Scene>();

            // Diese 2 Scenes sind alt und nicht für das fertige Spiel gedacht.
            scenes[EScene.SAND_CASTLES] = new Scene_DesertWithCastles(this);
            //scenes[EScene.INSIDE_CASTLE] = new Scene_InsideCastle(this);

            scenes[EScene.GAME_OVER] = new Scene_GameOver(this);
            scenes[EScene.REST_ROOM] = new Scene_RestRoom(this);

            scenes[EScene.TUTORIAL_ROOM_1] = new Scene_Tutorial_Room1(this);
            scenes[EScene.TUTORIAL_ROOM_2] = new Scene_Tutorial_Room2(this);
            scenes[EScene.TUTORIAL_ROOM_3] = new Scene_Tutorial_Room3(this);
            scenes[EScene.TUTORIAL_ROOM_4] = new Scene_Tutorial_Room4(this);
            scenes[EScene.TUTORIAL_ROOM_5] = new Scene_Tutorial_Room5(this);

            scenes[EScene.TRAIN_ROOM_1] = new Scene_Train_Room1(this);
            scenes[EScene.TRAIN_ROOM_2] = new Scene_Train_Room2(this);
            scenes[EScene.TRAIN_ROOM_3] = new Scene_Train_Room3(this);
            scenes[EScene.TRAIN_ROOM_4] = new Scene_Train_Room4(this);
            scenes[EScene.TRAIN_ROOM_5] = new Scene_Train_Room5(this);

            scenes[EScene.DESERT_ROOM_1] = new Scene_Desert_Room1(this);
            scenes[EScene.DESERT_ROOM_2] = new Scene_Desert_Room2(this);
            scenes[EScene.DESERT_ROOM_3] = new Scene_Desert_Room3(this);
            scenes[EScene.DESERT_ROOM_4] = new Scene_Desert_Room4(this);
            scenes[EScene.DESERT_ROOM_5] = new Scene_Desert_Room5(this);

            scenes[EScene.CASTLE_ROOM_1] = new Scene_Castle_Room1(this);
            scenes[EScene.CASTLE_ROOM_2] = new Scene_Castle_Room2(this);
            scenes[EScene.CASTLE_ROOM_3] = new Scene_Castle_Room3(this);
            scenes[EScene.CASTLE_ROOM_4] = new Scene_Castle_Room4(this);
            scenes[EScene.CASTLE_ROOM_5] = new Scene_Castle_Room5(this);

            scenes[EScene.BARREN_FALLS_ENTRANCE] = new Scene_BarrenFallsEntrance(this);
            scenes[EScene.BARREN_FALLS] = new Scene_BarrenFalls(this);

            roomSeqence = new List<EScene>();

            roomSeqence.AddRange(new EScene[]
            {
                // 0
                EScene.TUTORIAL_ROOM_1,
                EScene.TUTORIAL_ROOM_2,
                EScene.TUTORIAL_ROOM_3,
                EScene.TUTORIAL_ROOM_4,
                EScene.TUTORIAL_ROOM_5,

                EScene.REST_ROOM,

                // 6
                EScene.TRAIN_ROOM_1,
                EScene.TRAIN_ROOM_2,
                EScene.TRAIN_ROOM_3,
                EScene.TRAIN_ROOM_4,
                EScene.TRAIN_ROOM_5,

                EScene.REST_ROOM,

                // 12
                EScene.DESERT_ROOM_1,
                EScene.DESERT_ROOM_2,
                EScene.DESERT_ROOM_3,
                EScene.DESERT_ROOM_4,
                EScene.DESERT_ROOM_5,

                EScene.REST_ROOM,

                // 18
                EScene.CASTLE_ROOM_1,
                EScene.CASTLE_ROOM_2,
                EScene.CASTLE_ROOM_3,
                EScene.CASTLE_ROOM_4,
                EScene.CASTLE_ROOM_5,

                // 23
                EScene.BARREN_FALLS_ENTRANCE,
                EScene.BARREN_FALLS,
            });

            // Tutorial is not random.

            // Randomize Train Rooms 3 to 5.
            Utility.RandomizeList<EScene>(roomSeqence, 8, 10);

            // Randomize Desert Rooms 1 to 4.
            Utility.RandomizeList<EScene>(roomSeqence, 12, 15);

            // Randomize Castle Rooms 1 to 4.
            Utility.RandomizeList<EScene>(roomSeqence, 18, 21);

            currentScene = previousScene = scenes[EScene.CASTLE_ROOM_5];

			CollisionManager.AddCollidable(GameplayState.PlayerOne, CollisionManager.playerCollisionChannel);
			CollisionManager.AddCollidable(GameplayState.PlayerOne, CollisionManager.obstacleCollisionChannel);
			if (GameplayState.IsTwoPlayer)
			{
				CollisionManager.AddCollidable(GameplayState.PlayerTwo, CollisionManager.playerCollisionChannel);
				CollisionManager.AddCollidable(GameplayState.PlayerTwo, CollisionManager.obstacleCollisionChannel);
			}

			currentScene.OnEnterScene();
			currentScene.LoadContent(game.Content);

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

        public void SceneTransitionNextRoom()
        {
            if (!transitioning)
            {
                EScene nextRoom = roomSeqence[0];
                roomSeqence.RemoveAt(0);
                SceneTransition(nextRoom);
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

					// Kollision für Players setzen
					//CollisionManager.AddCollidables(GameplayState.PlayerTwo);
					//CollisionManager.AddCollidables(GameplayState.PlayerThree);
					//CollisionManager.AddCollidables(GameplayState.PlayerFour);
					CollisionManager.AddCollidable(GameplayState.PlayerOne, CollisionManager.playerCollisionChannel);
					CollisionManager.AddCollidable(GameplayState.PlayerOne, CollisionManager.obstacleCollisionChannel);
					if (GameplayState.IsTwoPlayer)
					{
						CollisionManager.AddCollidable(GameplayState.PlayerTwo, CollisionManager.playerCollisionChannel);
						CollisionManager.AddCollidable(GameplayState.PlayerTwo, CollisionManager.obstacleCollisionChannel);
					}

					nextScene.OnEnterScene();
					nextScene.LoadContent(game.Content);


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
