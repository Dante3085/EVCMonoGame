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

namespace EVCMonoGame.src.statemachine.gargoyle
{
    class StatePatrol : State
    {
        public float patrolLength;
        public Vector2 startPosition;
        public StateManagerGargoyle stateManagerGargoyle;
        public Gargoyle gargoyle;
        private Random ran = new Random();
        public bool stateFinished = false;
        public StatePatrol(Gargoyle gargoyle, params Transition[] transitions)
            : base("Patrol", transitions)
        {
            this.gargoyle = gargoyle;

        }
        public override void Enter(GameTime gameTime)
        {
            stateFinished = false;
            base.Enter(gameTime);
            startPosition = gargoyle.WorldPosition;
            Vector2 nextPatrolDirection = new Vector2(ran.Next(-100, 100), ran.Next(-100, 100));
            gargoyle.movementDirection = Utility.ScaleVectorTo(nextPatrolDirection, gargoyle.movementSpeed);
            patrolLength = ran.Next((int)gargoyle.sightRange / 2, (int)gargoyle.sightRange);
            float orientationAngle = Utility.GetAngleOfVectorInDegrees(gargoyle.movementDirection);
            gargoyle.Sprite.SetAnimation("FLYING_RIGHT");
            if (orientationAngle > (0) && orientationAngle <= (90))
            {
                gargoyle.Sprite.SetAnimation("FLYING_RIGHT");
            }
            if (orientationAngle > (90) && orientationAngle <= (180))
            {
                gargoyle.Sprite.SetAnimation("FLYING_LEFT");
            }
            if (orientationAngle > (-180) && orientationAngle <= (-90))
            {
                gargoyle.Sprite.SetAnimation("FLYING_LEFT");
            }
            if (orientationAngle > (-90) && orientationAngle <= (0))
            {
                gargoyle.Sprite.SetAnimation("FLYING_RIGHT");
            }
        }

        public override void Exit(GameTime gameTime)
        {
            base.Exit(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            gargoyle.PreviousWorldPosition = gargoyle.worldPosition;
            gargoyle.WorldPosition += Utility.ScaleVectorTo(gargoyle.movementDirection, gargoyle.MovementSpeed);
            stateFinished = CollisionManager.IsCollisionAfterMove(gargoyle, true, true);
            stateFinished = stateFinished || ((startPosition-gargoyle.WorldPosition).Length()>patrolLength);
        }
    }
}
