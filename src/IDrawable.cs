using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace EVCMonoGame.src
{
    public interface IDrawable
    {
        void Draw(SpriteBatch spriteBatch);
        void LoadContent(ContentManager content);
        void UnloadContent();
    }
}
