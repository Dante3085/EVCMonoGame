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

namespace EVCMonoGame.src.statemachine.riku
{
    class StateIdle: State
    {
        private PlayerTwo riku = GameplayState.PlayerTwo;
        public StateIdle(params Transition[] transitions)
            : base("Idle", transitions){}

        public override void Enter(GameTime gameTime)
        {
            switch (riku.playerOrientation)
            {
                case Orientation.LEFT: riku.Sprite.SetAnimation("IDLE_LEFT"); break;
                case Orientation.UP_LEFT: riku.Sprite.SetAnimation("IDLE_UP_LEFT"); break;
                case Orientation.UP: riku.Sprite.SetAnimation("IDLE_UP"); break;
                case Orientation.UP_RIGHT: riku.Sprite.SetAnimation("IDLE_UP_RIGHT"); break;
                case Orientation.RIGHT: riku.Sprite.SetAnimation("IDLE_RIGHT"); break;
                case Orientation.DOWN_RIGHT: riku.Sprite.SetAnimation("IDLE_DOWN_RIGHT"); break;
                case Orientation.DOWN: riku.Sprite.SetAnimation("IDLE_DOWN"); break;
                case Orientation.DOWN_LEFT: riku.Sprite.SetAnimation("IDLE_DOWN_LEFT"); break;
            }
        }
    }
}
