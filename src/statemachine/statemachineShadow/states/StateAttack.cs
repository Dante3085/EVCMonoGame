using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using EVCMonoGame.src.collision;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.characters.enemies;

namespace EVCMonoGame.src.statemachine.shadow

{
    class StateAttack : State
    {
        public StateManagerShadow stateManagerShadow;
        public TimeSpan lastAttack = new TimeSpan(0, 0, 0);
        public TimeSpan cooldown = new TimeSpan(0, 0, 3);
        public Shadow shadow;
        public StateAttack(/*StateManagerShadow stateManager*/ Shadow shadow, params Transition[] transitions)
            : base("Attack", transitions)
        {
            this.shadow = /*stateManager.*/shadow;
            //this.stateManagerShadow = stateManagerShadow;
        }
        public override void Enter(GameTime gameTime)
        {
            base.Enter(gameTime);
            if (CollisionManager.IsPlayerInRange(shadow, 300)) {
                Player nearestPlayer = CollisionManager.GetNearestPlayerInRange(shadow, 10000);

                if (nearestPlayer.Sprite.Bounds.Center.X > shadow.Sprite.Bounds.Center.X)
                {
                    shadow.Sprite.SetAnimation("NORMAL_ATTACK_RIGHT");
                }
                else
                {
                    shadow.Sprite.SetAnimation("NORMAL_ATTACK_LEFT");
                }
            }
            //attacklogic

        }

        public override void Exit(GameTime gameTime)
        {
            base.Exit(gameTime);
            lastAttack = gameTime.TotalGameTime;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            CollisionManager.CheckCombatCollisions(shadow);
        }
    }
}
