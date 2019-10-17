using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EVCMonoGame.src.scenes
{
    public abstract class Scene
    {
        protected List<Updateable> updateables;
        protected List<IDrawable> drawables;
        protected SceneManager sceneManager;

        private Camera camera;
        public Scene(SceneManager sceneManager)
        {
            this.sceneManager = sceneManager;
            updateables = new List<Updateable>();
            drawables = new List<IDrawable>();

            camera = new Camera(sceneManager.GraphicsDevice.Viewport);
        }

        public virtual void LoadContent(ContentManager contentManager)
        {
            foreach (IDrawable d in drawables)
            {
                d.LoadContent(contentManager);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            camera.UpdateCamera(sceneManager.GraphicsDevice.Viewport);

            foreach (Updateable u in updateables)
            {
                u.Update(gameTime);
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            foreach (IDrawable d in drawables)
            {
                d.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
        }
    }
}
