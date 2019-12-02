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
using EVCMonoGame.src.characters;

namespace EVCMonoGame.src.scenes
{
    // TODO: Tilemap vor Enemy initialisieren.

    public abstract class Scene
    {
        #region Fields
        public List<IUpdateable> updateables;
        public List<IDrawable> drawables;
        public static List<IUpdateable> updateablesToAdd;
        public static List<IDrawable> drawablesToAdd;
        public static List<IUpdateable> updateablesToRemove;
        public static List<IDrawable> drawablesToRemove;


        protected SceneManager sceneManager;
        protected Camera camera;
		protected bool pauseScene;

        protected bool drawCollisionInfo = false;

        protected Door doorPlayerOne;
        protected Door doorPlayerTwo;

        protected Tilemap tilemap;

        protected PlayerOne sora = GameplayState.PlayerOne;
        protected PlayerTwo riku = GameplayState.PlayerTwo;

        #endregion
        #region Constructors
        public Scene(SceneManager sceneManager)
        {
            this.sceneManager = sceneManager;
            updateables = new List<IUpdateable>();
            drawables = new List<IDrawable>();
            updateablesToAdd = new List<IUpdateable>();
            drawablesToAdd = new List<IDrawable>();
            updateablesToRemove = new List<IUpdateable>();
            drawablesToRemove = new List<IDrawable>();

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

                foreach (IUpdateable u in updateablesToAdd)
                    updateables.Add(u);
                updateablesToAdd.Clear();

                foreach (IUpdateable u in updateablesToRemove)
                    updateables.Remove(u);
                updateablesToRemove.Clear();
            }
            CollisionManager.Update(gameTime);
        }

		public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: camera.GetTransformationMatrix());

            if (tilemap != null)
            {
                tilemap.Draw(gameTime, spriteBatch);
            }

			foreach (IDrawable d in drawables)
			{
				d.Draw(gameTime, spriteBatch);
			}

            foreach (IDrawable d in drawablesToAdd)
            {
                drawables.Add(d);
                d.LoadContent(sceneManager.Content);
            }
            drawablesToAdd.Clear();

            foreach (IDrawable d in drawablesToRemove)
            {
                drawables.Remove(d);
            }
            drawablesToRemove.Clear();

            CollisionManager.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        public virtual void LoadContent(ContentManager contentManager)
        {
            foreach (IDrawable d in drawables)
            {
                d.LoadContent(contentManager);
            }

            if (tilemap != null)
            {
                tilemap.LoadContent(contentManager);
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
            updateablesToAdd.Clear();
            updateablesToRemove.Clear();
            drawablesToAdd.Clear();
            drawablesToRemove.Clear();
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
