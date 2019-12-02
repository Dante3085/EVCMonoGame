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

namespace EVCMonoGame.src.statemachine.riku
{
    class TransitionAttackButtonPressed : Transition
    {
        public TransitionAttackButtonPressed(String nextStateId) : base(nextStateId)
        {
        }
        public override bool checkCondition()
        {
            return InputManager.OnAnyButtonPressed(PlayerIndex.Two, Buttons.B, Buttons.X, Buttons.Y, Buttons.RightShoulder, Buttons.LeftShoulder)
                || InputManager.OnAnyKeyPressed(Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5);
        }
    }
}
