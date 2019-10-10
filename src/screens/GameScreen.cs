using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using EVCMonoGame.src.screens;

namespace EVCMonoGame.src.screens
{

    public abstract class GameScreen
    {
        protected List<Updateable> updateables = new List<Updateable>();
        protected List<IDrawable> drawables = new List<IDrawable>();
        protected ScreenManager screenManager;

        public GameScreen(ScreenManager screenManager)
        {
            this.screenManager = screenManager;
        }

        /// <summary>
        /// Loads content for all IDrawables of this GameScreen.
        /// </summary>
        /// <param name="content"></param>
        public virtual void LoadContent(ContentManager content)
        {
            foreach (IDrawable d in drawables)
            {
                d.LoadContent(content);
            }
        }

        /// <summary>
        /// Updates all IUpdateables of this GameScreen.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            foreach(Updateable u in updateables)
            {
                u.Update(gameTime);
            }
        }

        /// <summary>
        /// Draws all IDrawables of this GameScreen. 
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
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
