using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using EVCMonoGame.src.collision;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.characters.enemies;

namespace EVCMonoGame.src.statemachine.gargoyle

{
    class StateAttack : State
    {
        public StateManagerGargoyle stateManagerGargoyle;
        public TimeSpan lastAttack = new TimeSpan(0, 0, 0);
        public TimeSpan cooldown = new TimeSpan(0, 0, 1);
        public Gargoyle gargoyle;
        public StateAttack(/*StateManagerGargoyle stateManager*/ Gargoyle gargoyle, params Transition[] transitions)
            : base("Attack", transitions)
        {
            this.gargoyle = gargoyle;
        }
        public override void Enter(GameTime gameTime)
        {
            base.Enter(gameTime);
            Console.WriteLine("Gargoyle entered ATTACKSTATE");
            Player nearestPlayer = CollisionManager.GetNearestPlayerInRange(gargoyle, gargoyle.attackRange+10);
            
            gargoyle.CombatArgs.NewId();
            
            if (nearestPlayer.Sprite.Bounds.Center.X > gargoyle.CollisionBox.Center.X)
            {
                gargoyle.Sprite.SetAnimation("ATTACK_RIGHT");
            }
            else
            {
                gargoyle.Sprite.SetAnimation("ATTACK_LEFT");
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
            CollisionManager.CheckCombatCollisions(gargoyle);
        }
    }
}
