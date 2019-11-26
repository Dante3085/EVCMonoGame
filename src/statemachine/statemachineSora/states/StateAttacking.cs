﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using EVCMonoGame.src.characters;
using EVCMonoGame.src.states;
using EVCMonoGame.src.input;
using EVCMonoGame.src.collision;

namespace EVCMonoGame.src.statemachine.sora
{
    class StateAttacking : State
    {
        private PlayerOne sora = GameplayState.PlayerOne;
        private String nextAttackAnimation = "UNINITIALIZED";

        public StateAttacking(params Transition[] transitions) 
            : base("Attacking", transitions)
        {
        }

        public override void Enter(GameTime gameTime)
        {
            base.Enter(gameTime);

            if (InputManager.OnButtonPressed(Buttons.X, PlayerIndex.One)||
                InputManager.OnKeyPressed(Keys.D1))
            {
                OnXPressed();
            }
            else if (InputManager.OnButtonPressed(Buttons.Y, PlayerIndex.One) ||
                InputManager.OnKeyPressed(Keys.D2))
            {
                OnYPressed();
            }
            else if (InputManager.OnButtonPressed(Buttons.B, PlayerIndex.One) ||
                InputManager.OnKeyPressed(Keys.D3))
            {
                OnBPressed();
            }

            sora.Sprite.SetAnimation(nextAttackAnimation);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            CollisionManager.CheckCombatCollisions(sora);
        }

        public override void Exit(GameTime gameTime)
        {
            base.Exit(gameTime);

            sora.HasActiveAttackBounds = true;
        }

        private void OnXPressed()
        {
            CombatArgs combatArgs = sora.CombatArgs;
            combatArgs.causesFlinch = false;
            combatArgs.damage = sora.Strength + 5;

            switch (sora.playerOrientation)
            {
                case Orientation.LEFT: nextAttackAnimation = "ATTACK_STD_COMBO_LEFT_0";
                    combatArgs.knockBack = new Vector2(-10, 0);
                    break;

                case Orientation.UP_LEFT: nextAttackAnimation = "ATTACK_UP_LEFT";
                    combatArgs.knockBack = new Vector2(-10, -10);
                    break;

                case Orientation.UP: nextAttackAnimation = "ATTACK_UP";
                    combatArgs.knockBack = new Vector2(0, -200);
                    break;

                case Orientation.UP_RIGHT: nextAttackAnimation = "ATTACK_UP_RIGHT";
                    combatArgs.knockBack = new Vector2(10, -10);
                    break;

                case Orientation.RIGHT: nextAttackAnimation = "ATTACK_STD_COMBO_RIGHT_0";
                    combatArgs.knockBack = new Vector2(10, 0);
                    break;

                case Orientation.DOWN_RIGHT: nextAttackAnimation = "ATTACK_DOWN_RIGHT";
                    combatArgs.knockBack = new Vector2(10, 10);
                    break;

                case Orientation.DOWN: nextAttackAnimation = "ATTACK_DOWN";
                    combatArgs.knockBack = new Vector2(0, 10);
                    break;

                case Orientation.DOWN_LEFT: nextAttackAnimation = "ATTACK_DOWN_LEFT";
                    combatArgs.knockBack = new Vector2(-10, 10);
                    break;
            }
        }

        private void OnYPressed()
        {
            switch (sora.playerOrientation)
            {
                case Orientation.LEFT: nextAttackAnimation = "ATTACK_STD_COMBO_LEFT_1"; break;
                case Orientation.UP_LEFT: nextAttackAnimation = "ATTACK_UP_LEFT"; break;
                case Orientation.UP: nextAttackAnimation = "ATTACK_UP"; break;
                case Orientation.UP_RIGHT: nextAttackAnimation = "ATTACK_UP_RIGHT"; break;
                case Orientation.RIGHT: nextAttackAnimation = "ATTACK_STD_COMBO_RIGHT_1"; break;
                case Orientation.DOWN_RIGHT: nextAttackAnimation = "ATTACK_DOWN_RIGHT"; break;
                case Orientation.DOWN: nextAttackAnimation = "ATTACK_DOWN"; break;
                case Orientation.DOWN_LEFT: nextAttackAnimation = "ATTACK_DOWN_LEFT"; break;
            }
        }

        private void OnBPressed()
        {
            switch (sora.playerOrientation)
            {
                case Orientation.LEFT: nextAttackAnimation = "ATTACK_STD_COMBO_LEFT_2"; break;
                case Orientation.UP_LEFT: nextAttackAnimation = "ATTACK_UP_LEFT"; break;
                case Orientation.UP: nextAttackAnimation = "ATTACK_UP"; break;
                case Orientation.UP_RIGHT: nextAttackAnimation = "ATTACK_UP_RIGHT"; break;
                case Orientation.RIGHT: nextAttackAnimation = "ATTACK_STD_COMBO_RIGHT_2"; break;
                case Orientation.DOWN_RIGHT: nextAttackAnimation = "ATTACK_DOWN_RIGHT"; break;
                case Orientation.DOWN: nextAttackAnimation = "ATTACK_DOWN"; break;
                case Orientation.DOWN_LEFT: nextAttackAnimation = "ATTACK_DOWN_LEFT"; break;
            }
        }
    }
}