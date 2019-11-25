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

namespace EVCMonoGame.src.statemachine.sora
{
    class StateIdle: State
    {
        private PlayerOne sora = GameplayState.PlayerOne;
        public StateIdle(params Transition[] transitions)
            : base("Idle", transitions){}

        public override void Enter(GameTime gameTime)
        {
            switch (sora.playerOrientation)
            {
                case Orientation.LEFT: sora.Sprite.SetAnimation("IDLE_LEFT"); break;
                case Orientation.UP_LEFT: sora.Sprite.SetAnimation("IDLE_UP_LEFT"); break;
                case Orientation.UP: sora.Sprite.SetAnimation("IDLE_UP"); break;
                case Orientation.UP_RIGHT: sora.Sprite.SetAnimation("IDLE_UP_RIGHT"); break;
                case Orientation.RIGHT: sora.Sprite.SetAnimation("IDLE_RIGHT"); break;
                case Orientation.DOWN_RIGHT: sora.Sprite.SetAnimation("IDLE_DOWN_RIGHT"); break;
                case Orientation.DOWN: sora.Sprite.SetAnimation("IDLE_DOWN"); break;
                case Orientation.DOWN_LEFT: sora.Sprite.SetAnimation("IDLE_DOWN_LEFT"); break;
            }
        }
    }
}
