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
            Player nearestPlayer = CollisionManager.GetNearestPlayerInRange(gargoyle, gargoyle.attackRange + 10);

            gargoyle.CombatArgs.NewId();
            Console.WriteLine("gargoyleattackdamega:" + gargoyle.CombatArgs.damage);
            if ((nearestPlayer.Sprite.Bounds.Center - gargoyle.CollisionBox.Center).ToVector2().Length() > gargoyle.attackRange / 2)
            {
                CloseAttack(nearestPlayer);
            }
            else
            {
                BattleCry(nearestPlayer);
            }

        }


        private void CloseAttack(Player nearestPlayer)
        {
            cooldown = new TimeSpan(0, 0, 1);
            gargoyle.attackDmg = 50;
            if (nearestPlayer.Sprite.Bounds.Center.X > gargoyle.CollisionBox.Center.X)
            {
                gargoyle.Sprite.SetAnimation("ATTACK_RIGHT");
            }
            else
            {
                gargoyle.Sprite.SetAnimation("ATTACK_LEFT");
            }
        }

        private void BattleCry(Player nearestPlayer)
        {
            cooldown = new TimeSpan(0, 0, 3);
            gargoyle.attackDmg = 20;
            if (nearestPlayer.Sprite.Bounds.Center.X > gargoyle.CollisionBox.Center.X)
            {
                gargoyle.Sprite.SetAnimation("BATTLE_CRY_RIGHT");
            }
            else
            {
                gargoyle.Sprite.SetAnimation("BATTLE_CRY_LEFT");
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
