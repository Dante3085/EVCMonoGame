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

using EVCMonoGame.src.collision;

namespace EVCMonoGame.src.scenes
{
    public abstract class Scene
    {
        #region Fields
        protected List<IUpdateable> updateables;
        protected List<IDrawable> drawables;
        protected SceneManager sceneManager;
        protected Camera camera;
        #endregion
        #region Constructors
        public Scene(SceneManager sceneManager)
        {
            this.sceneManager = sceneManager;
            updateables = new List<IUpdateable>();
            drawables = new List<IDrawable>();
            this.camera = new Camera(sceneManager, new ITranslatablePosition(0, 0), Screenpoint.UP_LEFT_EDGE);

        }
        #endregion
        #region Methods
        public virtual void Update(GameTime gameTime)
        {
            foreach (IUpdateable u in updateables)
            {
                u.Update(gameTime);
            }
            camera.Update(gameTime);
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: camera.GetTransformationMatrix());

            CollisionManager.Draw(gameTime, spriteBatch);

            foreach (IDrawable d in drawables)
            {
                d.Draw(gameTime, spriteBatch);
            }
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

        }

        public virtual void OnExitScene()
        {
            updateables.Clear();
            drawables.Clear();
        }

        #endregion
    }
}
