
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using EVCMonoGame.src.collision;

namespace EVCMonoGame.src.enemies
{
    public class Shadow : Enemy
    {

        public Shadow(Vector2 position)
            : base(position)
        {
            enemySprite.LoadFromFile("Content/rsrc/spritesheets/configFiles/shadow.txt");
            enemySprite.SetAnimation("IDLE_LEFT");
        }

        #region Updateables
        public override void Update(GameTime gameTime)
        {
            if (enemySprite.AnimationFinished)
            {
                enemySprite.SetAnimation("IDLE_LEFT");
            }

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

        #region CombatCollidable
        public override void OnCombatCollision(CombatCollidable attacker)
        {
            enemySprite.SetAnimation("FLINCH_LEFT");
        }

        public override void ReceiveDamage(int amount)
        {
            enemyHealthbar.CurrentHp -= amount;
        }

        #endregion
    }
}
