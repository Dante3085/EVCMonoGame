using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using EVCMonoGame.src.input;
using EVCMonoGame.src.states;

using EVCMonoGame.src.collision;
using EVCMonoGame.src.tilemap;

namespace EVCMonoGame.src.scenes
{
    // TODO: Tilemap vor Enemy initialisieren.

    public abstract class Scene
    {
        #region Fields
        protected List<IUpdateable> updateables;
        protected List<IDrawable> drawables;
        protected SceneManager sceneManager;
        protected Camera camera;
		protected bool pauseScene;

        protected bool drawCollisionInfo = false;
        private ITranslatablePosition cameraFocus;

        protected Door doorPlayerOne;
        protected Door doorPlayerTwo;

        protected Tilemap tilemap;

        #endregion
        #region Constructors
        public Scene(SceneManager sceneManager)
        {
            this.sceneManager = sceneManager;
            updateables = new List<IUpdateable>();
            drawables = new List<IDrawable>();

            //cameraFocus = new ITranslatablePosition(GameplayState.PlayerOne.WorldPosition +
            //    (GameplayState.PlayerTwo.WorldPosition - GameplayState.PlayerOne.Sprite.WorldPosition) / 2);
            //camera = new Camera(sceneManager, cameraFocus, Screenpoint.CENTER);

            camera = new Camera(sceneManager, Vector2.Zero);
            camera.FollowPlayers();
        }
        #endregion
        #region Methods
        public virtual void Update(GameTime gameTime)
        {
            //cameraFocus.Position = GameplayState.PlayerOne.WorldPosition +
            //    (GameplayState.PlayerTwo.WorldPosition - GameplayState.PlayerOne.Sprite.WorldPosition) / 2;

            if (!GameplayState.PlayerOne.IsAlive && !GameplayState.PlayerTwo.IsAlive)
            {
                sceneManager.SceneTransition(EScene.GAME_OVER);
            }

            if (!pauseScene)
			{
				foreach (IUpdateable u in updateables)
				{
					u.Update(gameTime);
				}
				camera.Update(gameTime);
			}
            CollisionManager.Update(gameTime);
        }

		public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: camera.GetTransformationMatrix());

			foreach (IDrawable d in drawables)
			{
				d.Draw(gameTime, spriteBatch);
			}

            CollisionManager.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        public virtual void LoadContent(ContentManager contentManager)
        {
            foreach (IDrawable d in drawables)
            {
                d.LoadContent(contentManager);
            }
        }

        public virtual void OnEnterScene()
        {
			updateables.Add(GameplayState.PlayerOne);
			drawables.Add(GameplayState.PlayerOne);

			if (GameplayState.IsTwoPlayer) {
				updateables.Add(GameplayState.PlayerTwo);
				drawables.Add(GameplayState.PlayerTwo);
			}
		}

        public virtual void OnExitScene()
        {
            updateables.Clear();
            drawables.Clear();
		}

		public void Pause()
		{
			pauseScene = true;
		}

		public void Unpause()
		{
			pauseScene = false;
		}

        #endregion
    }
}
