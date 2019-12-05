using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using EVCMonoGame.src.characters;
using EVCMonoGame.src.states;
using EVCMonoGame.src.input;
using EVCMonoGame.src.projectiles;
using EVCMonoGame.src.collision;

namespace EVCMonoGame.src.statemachine.riku
{
    class StateAttacking : State
    {
        private PlayerTwo riku = GameplayState.PlayerTwo;
        private String nextAttackAnimation = "UNINITIALIZED";
        public TimeSpan lastAttack = new TimeSpan(0,0,0);
        public TimeSpan cooldown = new TimeSpan(0, 0, 3);

        public StateAttacking(params Transition[] transitions) 
            : base("Attacking", transitions)
        {
            cooldown = TimeSpan.FromMilliseconds(500);
        }

        public override void Enter(GameTime gameTime)
        {
            base.Enter(gameTime);
            if (InputManager.OnButtonPressed(Buttons.X, PlayerIndex.Two)||
                InputManager.OnKeyPressed(Keys.D1))
            {
                OnXPressed();
            }
            else if (InputManager.OnButtonPressed(Buttons.Y, PlayerIndex.Two) ||
                InputManager.OnKeyPressed(Keys.D2))
            {
                OnYPressed();
            }
            else if (InputManager.OnButtonPressed(Buttons.B, PlayerIndex.Two) ||
                InputManager.OnKeyPressed(Keys.D3))
            {
                OnBPressed();
            }else if(InputManager.OnButtonPressed(Buttons.RightShoulder, PlayerIndex.Two) ||
                InputManager.OnKeyPressed(Keys.D4))
            {
                OnRightShoulderPressed();
            }
            else if (InputManager.OnButtonPressed(Buttons.LeftShoulder, PlayerIndex.Two) ||
               InputManager.OnKeyPressed(Keys.D5))
            {
                OnLeftShoulderPressed();
            }

            riku.Sprite.SetAnimation(nextAttackAnimation);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            CollisionManager.CheckCombatCollisions(riku);
        }

        public override void Exit(GameTime gameTime)
        {
            base.Exit(gameTime);
            lastAttack = gameTime.TotalGameTime;
            riku.HasActiveAttackBounds = true;
        }

        private void OnXPressed()
        {
            cooldown = TimeSpan.FromMilliseconds(500);
            CombatArgs combatArgs = riku.CombatArgs;
            combatArgs.NewId();
            MagicMissileNormal missile = new MagicMissileNormal(Vector2.Zero, Orientation.DOWN);
            switch (riku.playerOrientation)
            {
                case Orientation.LEFT: nextAttackAnimation = "ATTACK_LEFT";
                    combatArgs.knockBack = new Vector2(-10, 0);
                    missile = new MagicMissileNormal(riku.CollisionBox.Location.ToVector2() + 
                        new Vector2(-(missile.CollisionBox.Width+1), riku.CollisionBox.Height / 2 - missile.CollisionBox.Height / 2), Orientation.LEFT);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;
                    
                case Orientation.UP_LEFT: nextAttackAnimation = "ATTACK_UP_LEFT";
                    combatArgs.knockBack = new Vector2(-10, -10);
                    missile = new MagicMissileNormal(riku.WorldPosition +
                        new Vector2(-(missile.CollisionBox.Width + 1), -(missile.CollisionBox.Height + 1)), Orientation.UP_LEFT, 10);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;

                case Orientation.UP: nextAttackAnimation = "ATTACK_UP";
                    combatArgs.knockBack = new Vector2(0, -200);
                    missile = new MagicMissileNormal(riku.WorldPosition + 
                        new Vector2(riku.CollisionBox.Width/2 - missile.CollisionBox.Width / 2, -(missile.CollisionBox.Height+1)), Orientation.UP);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;

                case Orientation.UP_RIGHT: nextAttackAnimation = "ATTACK_UP_RIGHT";
                    combatArgs.knockBack = new Vector2(10, -10);
                    missile = new MagicMissileNormal(riku.WorldPosition +
                        new Vector2(riku.CollisionBox.Width + 1, -(missile.CollisionBox.Height + 1)), Orientation.UP_RIGHT, 10);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;

                case Orientation.RIGHT: nextAttackAnimation = "ATTACK_RIGHT";
                    combatArgs.knockBack = new Vector2(10, 0);
                    missile = new MagicMissileNormal(riku.WorldPosition + 
                        new Vector2(riku.CollisionBox.Width+1, riku.CollisionBox.Height/2 - missile.CollisionBox.Height/2),Orientation.RIGHT);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;

                case Orientation.DOWN_RIGHT: nextAttackAnimation = "ATTACK_DOWN_RIGHT";
                    combatArgs.knockBack = new Vector2(10, 10);
                    missile = new MagicMissileNormal(riku.WorldPosition +
                        new Vector2(riku.CollisionBox.Width + 1, riku.CollisionBox.Height + 1), Orientation.DOWN_RIGHT, 10);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;

                case Orientation.DOWN: nextAttackAnimation = "ATTACK_DOWN";
                    combatArgs.knockBack = new Vector2(0, 10);
                    missile = new MagicMissileNormal(riku.WorldPosition + 
                        new Vector2(riku.CollisionBox.Width / 2 - missile.CollisionBox.Width / 2, riku.CollisionBox.Height + 1), Orientation.DOWN);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;

                case Orientation.DOWN_LEFT: nextAttackAnimation = "ATTACK_DOWN_LEFT";
                    combatArgs.knockBack = new Vector2(-10, 10);
                    missile = new MagicMissileNormal(riku.WorldPosition +
                        new Vector2(-(missile.CollisionBox.Width + 1), riku.CollisionBox.Height + 1), Orientation.DOWN_LEFT, 10);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;
            }
        }

        private void OnYPressed()
        {
            cooldown = TimeSpan.FromMilliseconds(1000);
            CombatArgs combatArgs = riku.CombatArgs;
            combatArgs.NewId();
            MagicMissileBounce missile = new MagicMissileBounce(Vector2.Zero, Orientation.DOWN);
            switch (riku.playerOrientation)
            {
                case Orientation.LEFT:
                    nextAttackAnimation = "THRUST_LEFT";
                    combatArgs.knockBack = new Vector2(-10, 0);
                    missile = new MagicMissileBounce(riku.CollisionBox.Location.ToVector2() +
                        new Vector2(-(missile.CollisionBox.Width + 1), riku.CollisionBox.Height / 2 - missile.CollisionBox.Height / 2), Orientation.LEFT);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;

                case Orientation.UP_LEFT:
                    nextAttackAnimation = "ATTACK_UP_LEFT";
                    combatArgs.knockBack = new Vector2(-10, -10);
                    missile = new MagicMissileBounce(riku.WorldPosition +
                        new Vector2(-(missile.CollisionBox.Width + 1), -(missile.CollisionBox.Height + 1)), Orientation.UP_LEFT, 10);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;

                case Orientation.UP:
                    nextAttackAnimation = "ATTACK_UP";
                    combatArgs.knockBack = new Vector2(0, -200);
                    missile = new MagicMissileBounce(riku.WorldPosition +
                        new Vector2(riku.CollisionBox.Width / 2 - missile.CollisionBox.Width / 2, -(missile.CollisionBox.Height + 1)), Orientation.UP);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;

                case Orientation.UP_RIGHT:
                    nextAttackAnimation = "ATTACK_UP_RIGHT";
                    combatArgs.knockBack = new Vector2(10, -10);
                    missile = new MagicMissileBounce(riku.WorldPosition +
                        new Vector2(riku.CollisionBox.Width + 1, -(missile.CollisionBox.Height + 1)), Orientation.UP_RIGHT, 10);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;

                case Orientation.RIGHT:
                    nextAttackAnimation = "THRUST_RIGHT";
                    combatArgs.knockBack = new Vector2(10, 0);
                    missile = new MagicMissileBounce(riku.WorldPosition +
                        new Vector2(riku.CollisionBox.Width + 1, riku.CollisionBox.Height / 2 - missile.CollisionBox.Height / 2), Orientation.RIGHT);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;

                case Orientation.DOWN_RIGHT:
                    nextAttackAnimation = "ATTACK_DOWN_RIGHT";
                    combatArgs.knockBack = new Vector2(10, 10);
                    missile = new MagicMissileBounce(riku.WorldPosition +
                        new Vector2(riku.CollisionBox.Width + 1, riku.CollisionBox.Height + 1), Orientation.DOWN_RIGHT, 10);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;

                case Orientation.DOWN:
                    nextAttackAnimation = "ATTACK_DOWN";
                    combatArgs.knockBack = new Vector2(0, 10);
                    missile = new MagicMissileBounce(riku.WorldPosition +
                        new Vector2(riku.CollisionBox.Width / 2 - missile.CollisionBox.Width / 2, riku.CollisionBox.Height + 1), Orientation.DOWN);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;

                case Orientation.DOWN_LEFT:
                    nextAttackAnimation = "ATTACK_DOWN_LEFT";
                    combatArgs.knockBack = new Vector2(-10, 10);
                    missile = new MagicMissileBounce(riku.WorldPosition +
                        new Vector2(-(missile.CollisionBox.Width + 1), riku.CollisionBox.Height + 1), Orientation.DOWN_LEFT, 10);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;
            }
        }

        private void OnBPressed()
        {
            cooldown = TimeSpan.FromMilliseconds(1500);
            CombatArgs combatArgs = riku.CombatArgs;
            combatArgs.NewId();
            MagicMissilePenetrate missile = new MagicMissilePenetrate(Vector2.Zero, Orientation.DOWN);
            switch (riku.playerOrientation)
            {
                case Orientation.LEFT:
                    nextAttackAnimation = "ROUND_SWING_LEFT";
                    combatArgs.knockBack = new Vector2(-10, 0);
                    missile = new MagicMissilePenetrate(riku.CollisionBox.Location.ToVector2() +
                        new Vector2(-(missile.CollisionBox.Width + 1), riku.CollisionBox.Height / 2 - missile.CollisionBox.Height / 2), Orientation.LEFT);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;

                case Orientation.UP_LEFT:
                    nextAttackAnimation = "ATTACK_UP_LEFT";
                    combatArgs.knockBack = new Vector2(-10, -10);
                    missile = new MagicMissilePenetrate(riku.WorldPosition +
                        new Vector2(-(missile.CollisionBox.Width + 1), -(missile.CollisionBox.Height + 1)), Orientation.UP_LEFT, 10);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;

                case Orientation.UP:
                    nextAttackAnimation = "ATTACK_UP";
                    combatArgs.knockBack = new Vector2(0, -200);
                    missile = new MagicMissilePenetrate(riku.WorldPosition +
                        new Vector2(riku.CollisionBox.Width / 2 - missile.CollisionBox.Width / 2, -(missile.CollisionBox.Height + 1)), Orientation.UP);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;

                case Orientation.UP_RIGHT:
                    nextAttackAnimation = "ATTACK_UP_RIGHT";
                    combatArgs.knockBack = new Vector2(10, -10);
                    missile = new MagicMissilePenetrate(riku.WorldPosition +
                        new Vector2(riku.CollisionBox.Width + 1, -(missile.CollisionBox.Height + 1)), Orientation.UP_RIGHT, 10);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;

                case Orientation.RIGHT:
                    nextAttackAnimation = "ROUND_SWING_RIGHT";
                    combatArgs.knockBack = new Vector2(10, 0);
                    missile = new MagicMissilePenetrate(riku.WorldPosition +
                        new Vector2(riku.CollisionBox.Width + 1, riku.CollisionBox.Height / 2 - missile.CollisionBox.Height / 2), Orientation.RIGHT);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;

                case Orientation.DOWN_RIGHT:
                    nextAttackAnimation = "ATTACK_DOWN_RIGHT";
                    combatArgs.knockBack = new Vector2(10, 10);
                    missile = new MagicMissilePenetrate(riku.WorldPosition +
                        new Vector2(riku.CollisionBox.Width + 1, riku.CollisionBox.Height + 1), Orientation.DOWN_RIGHT, 10);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;

                case Orientation.DOWN:
                    nextAttackAnimation = "ATTACK_DOWN";
                    combatArgs.knockBack = new Vector2(0, 10);
                    missile = new MagicMissilePenetrate(riku.WorldPosition +
                        new Vector2(riku.CollisionBox.Width / 2 - missile.CollisionBox.Width / 2, riku.CollisionBox.Height + 1), Orientation.DOWN);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;

                case Orientation.DOWN_LEFT:
                    nextAttackAnimation = "ATTACK_DOWN_LEFT";
                    combatArgs.knockBack = new Vector2(-10, 10);
                    missile = new MagicMissilePenetrate(riku.WorldPosition +
                        new Vector2(-(missile.CollisionBox.Width + 1), riku.CollisionBox.Height + 1), Orientation.DOWN_LEFT, 10);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;
            }
        }

        private void OnRightShoulderPressed()
        {
            cooldown = TimeSpan.FromMilliseconds(2500);
            CombatArgs combatArgs = riku.CombatArgs;
            combatArgs.NewId();
            MagicMissileSplit missile = new MagicMissileSplit(Vector2.Zero, Orientation.DOWN);
            switch (riku.playerOrientation)
            {
                case Orientation.LEFT:
                    nextAttackAnimation = "ATTACK_LEFT";
                    combatArgs.knockBack = new Vector2(-10, 0);
                    missile = new MagicMissileSplit(riku.CollisionBox.Location.ToVector2() +
                        new Vector2(-(missile.CollisionBox.Width + 1), riku.CollisionBox.Height / 2 - missile.CollisionBox.Height / 2), Orientation.LEFT);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;

                case Orientation.UP_LEFT:
                    nextAttackAnimation = "ATTACK_UP_LEFT";
                    combatArgs.knockBack = new Vector2(-10, -10);
                    missile = new MagicMissileSplit(riku.WorldPosition +
                        new Vector2(-(missile.CollisionBox.Width + 1), -(missile.CollisionBox.Height + 1)), Orientation.UP_LEFT, 10);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;

                case Orientation.UP:
                    nextAttackAnimation = "ATTACK_UP";
                    combatArgs.knockBack = new Vector2(0, -200);
                    missile = new MagicMissileSplit(riku.WorldPosition +
                        new Vector2(riku.CollisionBox.Width / 2 - missile.CollisionBox.Width / 2, -(missile.CollisionBox.Height + 1)), Orientation.UP);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;

                case Orientation.UP_RIGHT:
                    nextAttackAnimation = "ATTACK_UP_RIGHT";
                    combatArgs.knockBack = new Vector2(10, -10);
                    missile = new MagicMissileSplit(riku.WorldPosition +
                        new Vector2(riku.CollisionBox.Width + 1, -(missile.CollisionBox.Height + 1)), Orientation.UP_RIGHT, 10);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;

                case Orientation.RIGHT:
                    nextAttackAnimation = "ATTACK_RIGHT";
                    combatArgs.knockBack = new Vector2(10, 0);
                    missile = new MagicMissileSplit(riku.WorldPosition +
                        new Vector2(riku.CollisionBox.Width + 1, riku.CollisionBox.Height / 2 - missile.CollisionBox.Height / 2), Orientation.RIGHT);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;

                case Orientation.DOWN_RIGHT:
                    nextAttackAnimation = "ATTACK_DOWN_RIGHT";
                    combatArgs.knockBack = new Vector2(10, 10);
                    missile = new MagicMissileSplit(riku.WorldPosition +
                        new Vector2(riku.CollisionBox.Width + 1, riku.CollisionBox.Height + 1), Orientation.DOWN_RIGHT, 10);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;

                case Orientation.DOWN:
                    nextAttackAnimation = "ATTACK_DOWN";
                    combatArgs.knockBack = new Vector2(0, 10);
                    missile = new MagicMissileSplit(riku.WorldPosition +
                        new Vector2(riku.CollisionBox.Width / 2 - missile.CollisionBox.Width / 2, riku.CollisionBox.Height + 1), Orientation.DOWN);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;

                case Orientation.DOWN_LEFT:
                    nextAttackAnimation = "ATTACK_DOWN_LEFT";
                    combatArgs.knockBack = new Vector2(-10, 10);
                    missile = new MagicMissileSplit(riku.WorldPosition +
                        new Vector2(-(missile.CollisionBox.Width + 1), riku.CollisionBox.Height + 1), Orientation.DOWN_LEFT, 10);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;
            }
        }
        private void OnLeftShoulderPressed()
        {
            cooldown = TimeSpan.FromMilliseconds(3000);
            //Inventory... = cooldown.TotalMilliseconds;
            CombatArgs combatArgs = riku.CombatArgs;
            combatArgs.NewId();
            MagicMissileGodImperator missile = new MagicMissileGodImperator(Vector2.Zero, Orientation.DOWN);
            switch (riku.playerOrientation)
            {
                case Orientation.LEFT:
                    nextAttackAnimation = "ATTACK_LEFT";
                    combatArgs.knockBack = new Vector2(-10, 0);
                    missile = new MagicMissileGodImperator(riku.CollisionBox.Location.ToVector2() +
                        new Vector2(-(missile.CollisionBox.Width + 1), riku.CollisionBox.Height / 2 - missile.CollisionBox.Height / 2), Orientation.LEFT);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;

                case Orientation.UP_LEFT:
                    nextAttackAnimation = "ATTACK_UP_LEFT";
                    combatArgs.knockBack = new Vector2(-10, -10);
                    missile = new MagicMissileGodImperator(riku.WorldPosition +
                        new Vector2(-(missile.CollisionBox.Width + 1), -(missile.CollisionBox.Height + 1)), Orientation.UP_LEFT, 10);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;

                case Orientation.UP:
                    nextAttackAnimation = "ATTACK_UP";
                    combatArgs.knockBack = new Vector2(0, -200);
                    missile = new MagicMissileGodImperator(riku.WorldPosition +
                        new Vector2(riku.CollisionBox.Width / 2 - missile.CollisionBox.Width / 2, -(missile.CollisionBox.Height + 1)), Orientation.UP);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;

                case Orientation.UP_RIGHT:
                    nextAttackAnimation = "ATTACK_UP_RIGHT";
                    combatArgs.knockBack = new Vector2(10, -10);
                    missile = new MagicMissileGodImperator(riku.WorldPosition +
                        new Vector2(riku.CollisionBox.Width + 1, -(missile.CollisionBox.Height + 1)), Orientation.UP_RIGHT, 10);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;

                case Orientation.RIGHT:
                    nextAttackAnimation = "ATTACK_RIGHT";
                    combatArgs.knockBack = new Vector2(10, 0);
                    missile = new MagicMissileGodImperator(riku.WorldPosition +
                        new Vector2(riku.CollisionBox.Width + 1, riku.CollisionBox.Height / 2 - missile.CollisionBox.Height / 2), Orientation.RIGHT);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;

                case Orientation.DOWN_RIGHT:
                    nextAttackAnimation = "ATTACK_DOWN_RIGHT";
                    combatArgs.knockBack = new Vector2(10, 10);
                    missile = new MagicMissileGodImperator(riku.WorldPosition +
                        new Vector2(riku.CollisionBox.Width + 1, riku.CollisionBox.Height + 1), Orientation.DOWN_RIGHT, 10);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;

                case Orientation.DOWN:
                    nextAttackAnimation = "ATTACK_DOWN";
                    combatArgs.knockBack = new Vector2(0, 10);
                    missile = new MagicMissileGodImperator(riku.WorldPosition +
                        new Vector2(riku.CollisionBox.Width / 2 - missile.CollisionBox.Width / 2, riku.CollisionBox.Height + 1), Orientation.DOWN);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;

                case Orientation.DOWN_LEFT:
                    nextAttackAnimation = "ATTACK_DOWN_LEFT";
                    combatArgs.knockBack = new Vector2(-10, 10);
                    missile = new MagicMissileGodImperator(riku.WorldPosition +
                        new Vector2(-(missile.CollisionBox.Width + 1), riku.CollisionBox.Height + 1), Orientation.DOWN_LEFT, 10);
                    missile.LoadContent(MagicMissile.content);
                    GameplayState.PlayerTwo.missiles.Add(missile);
                    break;
            }
        }
    }
}
