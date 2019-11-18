
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using EVCMonoGame.src.collision;

namespace EVCMonoGame.src.characters.enemies
{
    public class Shadow : Enemy
    {
        public Shadow(int maxHp, int currentHp, Vector2 position)
            : base(maxHp, currentHp, position)
        {
            sprite.LoadAnimationsFromFile("Content/rsrc/spritesheets/configFiles/shadow.anm.txt");
            sprite.SetAnimation("NORMAL_ATTACK_LEFT");
        }

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

        #region CombatCollidable
        //public override void OnCombatCollision(CombatArgs combatArgs)
        //{
        //    base.OnCombatCollision(combatArgs);
        //    enemySprite.SetAnimation("FLINCH_LEFT");
        //}

        #endregion
    }
}
