using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using EVCMonoGame.src.input;
using EVCMonoGame.src.states;

namespace EVCMonoGame.src.statemachine.riku
{
    class TransitionAttackButtonPressed : Transition
    {
        public StateManagerRiku stateManagerRiku;
        public TransitionAttackButtonPressed(String nextStateId, StateManagerRiku stateManagerRiku) : base(nextStateId)
        {
            this.stateManagerRiku = stateManagerRiku;
        }
        public override bool checkCondition()
        {
            StateAttacking state = ((StateAttacking)stateManagerRiku.states.Find((a) => { return a.stateId == "Attacking"; }));
            return (Game1.totalGametime - state.lastAttack)>state.cooldown && 
                (InputManager.OnAnyButtonPressed(PlayerIndex.Two, Buttons.B, Buttons.X, Buttons.Y, Buttons.RightShoulder, Buttons.LeftShoulder)
                || InputManager.OnAnyKeyPressed(Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5))
                && GameplayState.PlayerTwo.IsAlive;
        }
    }
}
