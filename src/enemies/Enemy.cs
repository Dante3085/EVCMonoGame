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

namespace EVCMonoGame.src.enemies
{
    public class Enemy : Updateable, scenes.IDrawable, CombatCollidable
    {
        #region Fields

        protected AnimatedSprite enemySprite;
        protected Healthbar enemyHealthbar;

        #endregion

        #region Properties

        #region CombatCollidable

        public AnimatedSprite Sprite
        {
            get { return enemySprite; }
        }

        public Rectangle HurtBounds
        {
            get { return enemySprite.CurrentHurtBounds; }
        }

        public Rectangle AttackBounds
        {
            get { return enemySprite.CurrentAttackBounds; }
        }

        public bool HasActiveAttackBounds
        {
            get; private set;
        }

        public bool IsAlive
        {
            get { return enemyHealthbar.CurrentHp > 0; }
        }

        public int CurrentDamage
        {
            get 
            { 
                // TODO: Implement crazy damage calculations with Character stats and other stuff.
                return 30; 
            }
        }

        #endregion

        #endregion

        #region Constructors

        public Enemy(Vector2 position)
        {
            enemySprite = new AnimatedSprite(position, 5.0f);
            enemyHealthbar = new Healthbar(9999, 9999, Vector2.Zero, new Vector2(100, 10));
        }

        #endregion

        #region Updateables
        public override void Update(GameTime gameTime)
        {
            enemySprite.Update(gameTime);
            enemyHealthbar.Position = enemySprite.Position - new Vector2(0, enemyHealthbar.Size.Y);
        }
        #endregion

        #region IDrawable
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            enemySprite.Draw(gameTime, spriteBatch);
            enemyHealthbar.Draw(gameTime, spriteBatch);
        }

        public void LoadContent(ContentManager content)
        {
            enemySprite.LoadContent(content);
            enemyHealthbar.LoadContent(content);
        }
        #endregion

        #region CombatCollidable
        public void OnCombatCollision()
        {
            // TODO: This is specific to Enemy Type
        }

        public void ReceiveDamage(int amount)
        {
            enemyHealthbar.CurrentHp -= amount;
        }

        #endregion
    }
}
