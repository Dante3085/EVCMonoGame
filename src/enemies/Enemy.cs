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

        protected CombatArgs debugCombatArgs;

        #endregion

        #region Properties

        public AnimatedSprite Sprite
        {
            get { return enemySprite; }
        }

        #region CombatCollidable

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

        public bool HasActiveHurtBounds
        {
            get; private set;
        }

        public bool IsAlive
        {
            get { return enemyHealthbar.CurrentHp > 0; }
        }

        public CombatArgs CurrentCombatArgs
        {
            get { return debugCombatArgs; }
        }

        #endregion

        #endregion

        #region Constructors

        public Enemy(Vector2 position)
        {
            enemySprite = new AnimatedSprite(position, 5.0f);
            enemyHealthbar = new Healthbar(9999, 9999, Vector2.Zero, new Vector2(100, 10));

            debugCombatArgs = new CombatArgs(this, this, new Vector2(2, 0), 5);
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
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            enemySprite.Draw(gameTime, spriteBatch);
            enemyHealthbar.Draw(gameTime, spriteBatch);
        }

        public virtual void LoadContent(ContentManager content)
        {
            enemySprite.LoadContent(content);
            enemyHealthbar.LoadContent(content);
        }
        #endregion

        #region CombatCollidable
        public virtual void OnCombatCollision(CombatArgs combatArgs)
        {
            enemyHealthbar.CurrentHp -= combatArgs.damage;
            enemySprite.Position += combatArgs.knockBack;
        }

        #endregion
    }
}
