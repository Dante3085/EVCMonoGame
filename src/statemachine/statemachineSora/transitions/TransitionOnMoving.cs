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

namespace EVCMonoGame.src.statemachine.sora
{
    class TransitionOnMoving :Transition
    {
        public TransitionOnMoving(String nextStateId) : base(nextStateId) { }
        public override bool checkCondition()
        {
            return InputManager.CurrentThumbSticks(PlayerIndex.One).Left != Vector2.Zero ||
                InputManager.IsAnyKeyPressed(Keys.Left, Keys.Up, Keys.Right, Keys.Down);
        }
    }
}
