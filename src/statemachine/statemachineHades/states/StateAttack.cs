﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using EVCMonoGame.src.collision;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.characters.enemies;

namespace EVCMonoGame.src.statemachine.hades

{
    class StateAttack : State
    {
        public StateManagerHades stateManagerHades;
        public TimeSpan lastAttack = new TimeSpan(0, 0, 0);
        public TimeSpan cooldown = new TimeSpan(0, 0, 1);
        public Hades hades;
        public StateAttack(/*StateManagerHades stateManager*/ Hades hades, params Transition[] transitions)
            : base("Attack", transitions)
        {
            this.hades = hades;
        }
        public override void Enter(GameTime gameTime)
        {
            base.Enter(gameTime);
            Console.WriteLine("Hades entered ATTACKSTATE");
            Player nearestPlayer = CollisionManager.GetNearestPlayerInRange(hades, hades.attackRange+10);
            
            hades.CombatArgs.NewId();
            
            if (nearestPlayer.Sprite.Bounds.Center.X > hades.CollisionBox.Center.X)
            {
                hades.Sprite.SetAnimation("ATTACK_GUN_RIGHT");
            }
            else
            {
                hades.Sprite.SetAnimation("ATTACK_GUN_LEFT");
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
            CollisionManager.CheckCombatCollisions(hades);
        }
    }
}
