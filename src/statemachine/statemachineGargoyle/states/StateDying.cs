using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using EVCMonoGame.src.collision;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.characters.enemies;
using EVCMonoGame.src.utility;
using EVCMonoGame.src.scenes;

namespace EVCMonoGame.src.statemachine.gargoyle
{
    class StateDying : State
    {
        public StateManagerGargoyle stateManagerGargoyle;
        public TimeSpan enteredState = new TimeSpan(0, 0, 0);
        public TimeSpan duration = new TimeSpan(0, 0, 10);
        public Gargoyle gargoyle;
        public StateDying(Gargoyle gargoyle, params Transition[] transitions)
            : base("Dying", transitions)
        {
            this.gargoyle = gargoyle;
        }
        public override void Enter(GameTime gameTime)
        {
            base.Enter(gameTime);
            switch (Utility.GetOrientationDiagonal(gargoyle.movementDirection))
            {
                case Orientation.DOWN_LEFT:
                    gargoyle.Sprite.SetAnimation("FLINCH_LEFT");
                    break;
                case Orientation.DOWN_RIGHT:
                    gargoyle.Sprite.SetAnimation("FLINCH_RIGHT");
                    break;
                case Orientation.UP_LEFT:
                    gargoyle.Sprite.SetAnimation("FLINCH_LEFT");
                    break;
                case Orientation.UP_RIGHT:
                    gargoyle.Sprite.SetAnimation("FLINCH_RIGHT");
                    break;
                default:
                    gargoyle.Sprite.SetAnimation("FLINCH_RIGHT");
                    break;
            }

        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (gargoyle.Sprite.AnimationFinished) Exit(gameTime);
        }

        public override void Exit(GameTime gameTime)
        {
            base.Exit(gameTime);
            gargoyle.FlaggedForRemove = true;
            Scene.updateablesToRemove.Add(gargoyle);
            Scene.drawablesToRemove.Add(gargoyle);
        }
    }
}
