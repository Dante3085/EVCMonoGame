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
            
            if (nearestPlayer.Sprite.Bounds.Center.X > defender.CollisionBox.Center.X)
            {
                defender.Sprite.SetAnimation("SHIELD_SPIN_RIGHT");
            }
            else
            {
                defender.Sprite.SetAnimation("SHIELD_SPIN_LEFT");
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
