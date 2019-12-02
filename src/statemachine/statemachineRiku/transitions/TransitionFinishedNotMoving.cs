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
    class TransitionFinishedNotMoving : Transition
    {
        public TransitionFinishedNotMoving(String nextStateId) : base(nextStateId)
        {
        }
        public override bool checkCondition()
        {
            if (GameplayState.PlayerTwo.Sprite.AnimationFinished &&
                InputManager.CurrentThumbSticks(PlayerIndex.Two).Left == Vector2.Zero &&
                !InputManager.IsAnyKeyPressed(GameplayState.PlayerTwo.keyboardControls))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
