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
    class StateFlinching : State
    {
        public StateFlinching(params Transition[] transitions)
            : base("Flinching", transitions)
        {
        }
        public override void Enter(GameTime gameTime)
        {
            Orientation o = GameplayState.PlayerOne.playerOrientation;
            if (o == Orientation.LEFT || o == Orientation.DOWN_LEFT || o == Orientation.UP_LEFT || o == Orientation.UP)
            {
                GameplayState.PlayerOne.Sprite.SetAnimation("FLINCH_LEFT");
            }
            else
            {
                GameplayState.PlayerOne.Sprite.SetAnimation("FLINCH_RIGHT");
            }
        }
    }
}
