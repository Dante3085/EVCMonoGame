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


namespace EVCMonoGame.src.statemachine
{
    class TransitionFinishedWithMoving: Transition
    {
        public TransitionFinishedWithMoving(String nextStateId) : base(nextStateId)
        {
        }
        public override bool checkCondition()
        {
            if (GameplayState.PlayerOne.Sprite.AnimationFinished &&
                InputManager.CurrentThumbSticks(PlayerIndex.One).Left != Vector2.Zero)
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
