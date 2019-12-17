using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using EVCMonoGame.src.collision;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.characters.enemies;
using Microsoft.Xna.Framework.Audio;

namespace EVCMonoGame.src.statemachine.gargoyle

{
    public enum NextGargoyleAttack
    {
        CLOSEATTACK,
        CRYATTACK,
        UNKNOWN
    }
    class StateAttack : State
    {
        public NextGargoyleAttack nextGargoyleAttack = NextGargoyleAttack.UNKNOWN;
        public StateManagerGargoyle stateManagerGargoyle;
        public TimeSpan lastCloseAttack = new TimeSpan(0, 0, 0);
        public TimeSpan lastCryAttack = new TimeSpan(0, 0, 0);
        public bool setCryTime = false;
        public bool setCloseTime = false;

        public TimeSpan cooldownClose = new TimeSpan(0, 0, 1);
        public TimeSpan cooldownFar = new TimeSpan(0, 0, 5);
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
            switch (nextGargoyleAttack)
            {
                case NextGargoyleAttack.CLOSEATTACK:
                    CloseAttack(nearestPlayer);
                    break;
                case NextGargoyleAttack.CRYATTACK:
                    BattleCry(nearestPlayer);
                    break;
                case NextGargoyleAttack.UNKNOWN:
                    if ((nearestPlayer.Sprite.Bounds.Center - gargoyle.CollisionBox.Center).ToVector2().Length() < gargoyle.attackRange / 2)
                    {
                        CloseAttack(nearestPlayer);
                    }
                    else
                    {
                        BattleCry(nearestPlayer);
                    }
                    break;
            }

        }


        private void CloseAttack(Player nearestPlayer)
        {
            setCloseTime = true;
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
            gargoyle.scream.CreateInstance().Play();
            setCryTime = true;
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
            if (setCloseTime)
            {
                lastCloseAttack = gameTime.TotalGameTime;
                setCloseTime = false;
            }
            if (setCryTime)
            {
                lastCryAttack = gameTime.TotalGameTime;
                setCryTime = false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            CollisionManager.CheckCombatCollisions(gargoyle);
        }
    }
}
