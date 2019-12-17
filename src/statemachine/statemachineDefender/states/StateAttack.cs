﻿using System;
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

            Random random = new Random();
            int rndNum = random.Next(0, 2);

            if (nearestPlayer.Sprite.Bounds.Center.X > defender.CollisionBox.Center.X)
            {
                if (rndNum == 1)
                {
                    defender.Sprite.SetAnimation("SHIELD_SPIN_RIGHT");
                }
                else
                {
                    defender.CombatArgs.knockBack = new Vector2(150, 0);
                    defender.Sprite.SetAnimation("SHIELD_CHARGE_RIGHT");
                }
            }
            else
            {
                if (rndNum == 1)
                {
                    defender.Sprite.SetAnimation("SHIELD_SPIN_LEFT");
                }
                else
                {
                    defender.CombatArgs.knockBack = new Vector2(-150, 0);
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
