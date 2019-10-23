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
        #region Fields
        protected List<Updateable> updateables;
        protected List<IDrawable> drawables;
        protected SceneManager sceneManager;
        #endregion
        #region Constructors
        public Scene(SceneManager sceneManager)
        {
            this.sceneManager = sceneManager;
            updateables = new List<Updateable>();
            drawables = new List<IDrawable>();

        }
        #endregion
        #region Methods
        public virtual void Update(GameTime gameTime)
        {
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

        public virtual void LoadContent(ContentManager contentManager)
        {
            foreach (IDrawable d in drawables)
            {
                d.LoadContent(contentManager);
            }
        }
        #endregion
    }
}
