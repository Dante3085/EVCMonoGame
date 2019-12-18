﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using EVCMonoGame.src.collision;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.characters.enemies;
using EVCMonoGame.src.states;

namespace EVCMonoGame.src.statemachine.hadesRage

{
    public enum NEXTATTACK
    {
        FIREBLAST,
        STRIKE,
        METEOR,
        UNDEFINED
    }
    class StateAttack : State
    {
        public NEXTATTACK nextAttack = NEXTATTACK.UNDEFINED;
        public StateManagerHadesRage stateManagerHadesRage;
        public TimeSpan lastFireBlastAttack = new TimeSpan(0, 0, 0);
        public TimeSpan lastStrikeAttack = new TimeSpan(0, 0, 0);
        public TimeSpan lastMeteorAttack = new TimeSpan(0, 0, 0);
        public bool setFireBlastTime = false;
        public bool setStrikeTime = false;
        public bool setMeteorTime = false;
        public TimeSpan cooldownFireBlast = new TimeSpan(0, 0, 5);
        public TimeSpan cooldownStrike = new TimeSpan(0, 0, 1);
        public TimeSpan cooldownMeteor = new TimeSpan(0, 0, 15);
        public bool meteorLeft = false;
        public bool meteorRight = false;
        public Hades hades;
        public StateAttack(/*StateManagerHadesRage stateManager*/ Hades hades, params Transition[] transitions)
            : base("Attack", transitions)
        {
            this.hades = hades;
        }
        public override void Enter(GameTime gameTime)
        {
            base.Enter(gameTime);
            Console.WriteLine("Hades entered ATTACKSTATE");
            Player nearestPlayer = CollisionManager.GetNearestPlayerInRange(hades, hades.attackRangeMeteor+ 10);

            hades.CombatArgs.NewId();

            switch (nextAttack)
            {
                case NEXTATTACK.FIREBLAST:
                    Fireblast(nearestPlayer);
                    break;
                case NEXTATTACK.METEOR:
                    Meteor();
                    break;
                case NEXTATTACK.STRIKE:
                    Strike(nearestPlayer);
                    break;
                case NEXTATTACK.UNDEFINED:
                    Strike(nearestPlayer);
                    break;
            }

        }
        public void Strike(Player nearestPlayer)
        {
            hades.CombatArgs.damage = 40;
            setStrikeTime = true;
            if (nearestPlayer.Sprite.Bounds.Center.X > hades.CollisionBox.Center.X)
            {
                hades.Sprite.SetAnimation("RAGE_STRIKE_RIGHT");
            }
            else
            {
                hades.Sprite.SetAnimation("RAGE_STRIKE_LEFT");
            }
        }
        public void Fireblast(Player nearestPlayer)
        {
            hades.CombatArgs.damage = 80;
            setFireBlastTime = true;
            if (nearestPlayer.Sprite.Bounds.Center.X > hades.CollisionBox.Center.X)
            {
                hades.Sprite.SetAnimation("RAGE_FIRE_BLAST_RIGHT");
            }
            else
            {
                hades.Sprite.SetAnimation("RAGE_FIRE_BLAST_LEFT");
            }
        }
        public void Meteor()
        {
            Player targetPlayer;
            if((GameplayState.PlayerOne.worldPosition-hades.worldPosition).Length()>
                (GameplayState.PlayerTwo.worldPosition - hades.worldPosition).Length())
            {
                targetPlayer = GameplayState.PlayerOne;
            }
            else
            {
                targetPlayer = GameplayState.PlayerTwo;
            }
            setMeteorTime = true;
            if (targetPlayer.Sprite.Bounds.Center.X > hades.CollisionBox.Center.X)
            {
                meteorRight = true;
                hades.Sprite.SetAnimation("RAGE_METEOR_RIGHT");
            }
            else
            {
                meteorLeft = true;
                hades.Sprite.SetAnimation("RAGE_METEOR_LEFT");
            }

        }
        public override void Exit(GameTime gameTime)
        {
            base.Exit(gameTime);

            if (setStrikeTime)
            {
                lastStrikeAttack = gameTime.TotalGameTime;
                setStrikeTime = false;
            }
            if (setMeteorTime)
            {
                lastMeteorAttack = gameTime.TotalGameTime;
                setMeteorTime = false;
            }
            if (setFireBlastTime)
            {
                lastFireBlastAttack = gameTime.TotalGameTime;
                setFireBlastTime = false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            CollisionManager.CheckCombatCollisions(hades);
            if (setMeteorTime) { }
        }
    }
}