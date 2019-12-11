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
        public Vector2 targetPoint = Vector2.One;
        public StateManagerShadow stateManagerShadow;
        public Shadow shadow;
        private Random ran = new Random();
        public bool leaveState = false;
        public StatePatrol(Shadow shadow, params Transition[] transitions)
            : base("Attack", transitions)
        {
            this.shadow = shadow;

        }
        public override void Enter(GameTime gameTime)
        {
            base.Enter(gameTime);
            Vector2 nextPatrolPoint = new Vector2(ran.Next(-100, 100), ran.Next(-100, 100));
            nextPatrolPoint = Utility.ScaleVectorTo(nextPatrolPoint, ran.Next((int)shadow.sightRange / 2, (int)shadow.sightRange));
            nextPatrolPoint = Utility.ScaleVectorTo(nextPatrolPoint, ((int)(nextPatrolPoint.Length()/shadow.MovementSpeed))*shadow.MovementSpeed);
            shadow.movementDirection = Utility.ScaleVectorTo(nextPatrolPoint, shadow.MovementSpeed);
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
            shadow.WorldPosition += shadow.movementDirection;
            leaveState = CollisionManager.IsCollisionAfterMove(shadow, true, true);
            leaveState = shadow.WorldPosition == targetPoint;
        }
    }
}
