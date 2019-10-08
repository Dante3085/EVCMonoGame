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
        protected List<IUpdateable> updateables = new List<IUpdateable>();
        protected List<IDrawable> drawables = new List<IDrawable>();
        private ScreenManager screenManager;

        public GameScreen(ScreenManager screenManager)
        {
            this.screenManager = screenManager;
        }

        public virtual void LoadContent(ContentManager content)
        {
            foreach (IDrawable d in drawables)
            {
                d.LoadContent(content);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach(IUpdateable u in updateables)
            {
                u.Update(gameTime);
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (IDrawable d in drawables)
            {
                d.Draw(gameTime, spriteBatch);
            }
        }
    }
}
