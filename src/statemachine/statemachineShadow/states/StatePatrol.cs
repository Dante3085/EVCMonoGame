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

namespace EVCMonoGame.src.statemachine.shadow
{
    class StatePatrol : State
    {
        public float patrolLength;
        public Vector2 startPosition;
        public StateManagerShadow stateManagerShadow;
        public Shadow shadow;
        private Random ran = new Random();
        public bool stateFinished = false;
        public StatePatrol(Shadow shadow, params Transition[] transitions)
            : base("Patrol", transitions)
        {
            this.shadow = shadow;

        }
        public override void Enter(GameTime gameTime)
        {
            stateFinished = false;
            base.Enter(gameTime);
            startPosition = shadow.WorldPosition;
            Vector2 nextPatrolDirection = new Vector2(ran.Next(-100, 100), ran.Next(-100, 100));
            shadow.movementDirection = Utility.ScaleVectorTo(nextPatrolDirection, shadow.movementSpeed);
            patrolLength = ran.Next((int)shadow.sightRange / 2, (int)shadow.sightRange);
            float orientationAngle = Utility.GetAngleOfVectorInDegrees(shadow.movementDirection);
            shadow.Sprite.SetAnimation("WALK_DOWN_RIGHT");
            if (orientationAngle > (0) && orientationAngle <= (90))
            {
                shadow.Sprite.SetAnimation("WALK_UP_RIGHT");
            }
            if (orientationAngle > (90) && orientationAngle <= (180))
            {
                shadow.Sprite.SetAnimation("WALK_UP_LEFT");
            }
            if (orientationAngle > (-180) && orientationAngle <= (-90))
            {
                shadow.Sprite.SetAnimation("WALK_DOWN_LEFT");
            }
            if (orientationAngle > (-90) && orientationAngle <= (0))
            {
                shadow.Sprite.SetAnimation("WALK_DOWN_RIGHT");
            }
        }

        public override void Exit(GameTime gameTime)
        {
            base.Exit(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            shadow.PreviousWorldPosition = shadow.worldPosition;
            shadow.WorldPosition += Utility.ScaleVectorTo(shadow.movementDirection, shadow.MovementSpeed);
            stateFinished = CollisionManager.IsCollisionAfterMove(shadow, true, true);
            stateFinished = stateFinished || ((startPosition-shadow.WorldPosition).Length()>patrolLength);
        }
    }
}
