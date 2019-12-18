using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using EVCMonoGame.src.collision;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.characters.enemies;

namespace EVCMonoGame.src.statemachine.defender

{
    class StateAttack : State
    {
        public StateManagerDefender stateManagerDefender;
        public TimeSpan lastAttack = new TimeSpan(0, 0, 0);
        public TimeSpan cooldown = new TimeSpan(0, 0, 1);
        public Defender defender;
        public StateAttack(/*StateManagerDefender stateManager*/ Defender defender, params Transition[] transitions)
            : base("Attack", transitions)
        {
            this.defender = defender;
        }
        public override void Enter(GameTime gameTime)
        {
            base.Enter(gameTime);
            Console.WriteLine("Defender entered ATTACKSTATE");
            Player nearestPlayer = CollisionManager.GetNearestPlayerInRange(defender, defender.attackRange+10);
            
            defender.CombatArgs.NewId();

            float distanceFromPlayer = (defender.CollisionBox.Center - nearestPlayer.CollisionBox.Center).ToVector2().Length();
            Console.WriteLine("distanceFromPlayer: " + distanceFromPlayer);

            Random random = new Random();
            int rndNum = random.Next(0, 2);

            if (nearestPlayer.Sprite.Bounds.Center.X > defender.CollisionBox.Center.X)
            {
                if (distanceFromPlayer <= 180)
                {
                    defender.Sprite.SetAnimation("SHIELD_SPIN_RIGHT");
                }
                else if (distanceFromPlayer > 180)
                {
                    defender.CombatArgs.knockBack = new Vector2(25, 0);
                    defender.Sprite.SetAnimation("SHIELD_CHARGE_RIGHT");
                }
            }
            else
            {
                if (distanceFromPlayer <= 180)
                {
                    defender.Sprite.SetAnimation("SHIELD_SPIN_LEFT");
                }
                else if (distanceFromPlayer > 180)
                {
                    defender.CombatArgs.knockBack = new Vector2(-25, 0);
                    defender.Sprite.SetAnimation("SHIELD_CHARGE_LEFT");
                }
            }
        }

        public override void Exit(GameTime gameTime)
        {
            base.Exit(gameTime);
            lastAttack = gameTime.TotalGameTime;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            CollisionManager.CheckCombatCollisions(defender);
        }
    }
}
