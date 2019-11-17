using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using EVCMonoGame.src.scenes;
using EVCMonoGame.src.collision;
using EVCMonoGame.src.animation;
using EVCMonoGame.src.gui;
using EVCMonoGame.src.characters;

namespace EVCMonoGame.src.characters
{
    public class Enemy : Character, scenes.IUpdateable, scenes.IDrawable
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructors

        public Enemy(int maxHp, int currentHp, Vector2 position)
            : base(maxHp, currentHp, position)
        {
            sprite = new AnimatedSprite(position, 5.0f);

            CollisionManager.AddCollidable(this, CollisionManager.enemyCollisionChannel);
        }

        #endregion

        #region Updateables
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        #endregion

        #region IDrawable
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
        }
        #endregion
    }
}
