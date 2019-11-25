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
using EVCMonoGame.src.statemachine.statemachinePlayer;
using EVCMonoGame.src.input;

namespace EVCMonoGame.src.statemachine.statemachinePlayer.transitions
{
    class TransitionFinishedNoMoving : Transition
    {
        State state;
        StateManagerPlayer stateManager;
        public TransitionFinishedNoMoving(StateManagerPlayer stateManager, String nextStateId)
        {
            this.stateManager = stateManager;
            this.nextStateId = nextStateId;
        }
        public override bool checkCondition()
        {
            if (((StateManagerPlayer)stateManager).player.Sprite.AnimationFinished &&
                InputManager.CurrentThumbSticks(PlayerIndex.One).Left == Vector2.Zero)
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
