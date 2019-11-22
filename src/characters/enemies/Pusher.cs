
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
    public class Pusher : Enemy
    {
        public Pusher(int maxHp, int currentHp, Vector2 position)
            : base(maxHp, currentHp, position)
        {
            sprite.LoadAnimationsFromFile("Content/rsrc/spritesheets/configFiles/shadow.anm.txt");
            sprite.SetAnimation("SPAWN_LEFT");


			CollisionBox = new Rectangle(position.ToPoint(), new Point(200, 200));

			agentMindestBreite = 200;
			attackRange = 250;
			attackSpeed = 3000;
			attackDmg = 0;
        }

        #region Updateables
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (sprite.CurrentAnimation == "SPAWN_LEFT" && 
                sprite.AnimationFinished)
            {
                sprite.SetAnimation("SPAWN_LEFT");
            }


			if (target != null && CollisionManager.IsPlayerInRange(this, attackRange))
			{
				Vector2 push = target.CollisionBox.Center.ToVector2() - CollisionBox.Center.ToVector2();
				push.Normalize();
				
				target.WorldPosition = target.WorldPosition + push * new Vector2(movementSpeed, movementSpeed);
				CollisionManager.IsCollisionAfterMove(target, true, true);
			}

			
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

		public override void Attack(Character target)
		{
			base.Attack(target);
		}

		#region CombatCollidable
		//public override void OnCombatCollision(CombatArgs combatArgs)
		//{
		//    base.OnCombatCollision(combatArgs);
		//    enemySprite.SetAnimation("FLINCH_LEFT");
		//}

		#endregion
	}
}
