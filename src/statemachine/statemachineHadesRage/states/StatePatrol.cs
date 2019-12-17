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

namespace EVCMonoGame.src.statemachine.hadesRage
{
    class StatePatrol : State
    {
        public float patrolLength;
        public Vector2 startPosition;
        public StateManagerHadesRage stateManagerHadesRage;
        public Hades hades;
        private Random ran = new Random();
        public bool stateFinished = false;
        public StatePatrol(Hades hades, params Transition[] transitions)
            : base("Patrol", transitions)
        {
            this.hades = hades;

        }
        public override void Enter(GameTime gameTime)
        {
            stateFinished = false;
            base.Enter(gameTime);
            startPosition = hades.WorldPosition;
            Vector2 nextPatrolDirection = new Vector2(ran.Next(-100, 100), ran.Next(-100, 100));
            hades.movementDirection = Utility.ScaleVectorTo(nextPatrolDirection, hades.movementSpeed);
            patrolLength = ran.Next((int)hades.sightRange / 2, (int)hades.sightRange);
            float orientationAngle = Utility.GetAngleOfVectorInDegrees(hades.movementDirection);
            hades.Sprite.SetAnimation("RAGE_MOVE_RIGHT");
            if (orientationAngle > (0) && orientationAngle <= (90))
            {
                hades.Sprite.SetAnimation("RAGE_MOVE_RIGHT");
            }
            if (orientationAngle > (90) && orientationAngle <= (180))
            {
                hades.Sprite.SetAnimation("RAGE_MOVE_LEFT");
            }
            if (orientationAngle > (-180) && orientationAngle <= (-90))
            {
                hades.Sprite.SetAnimation("RAGE_MOVE_LEFT");
            }
            if (orientationAngle > (-90) && orientationAngle <= (0))
            {
                hades.Sprite.SetAnimation("RAGE_MOVE_RIGHT");
            }
        }

        public override void Exit(GameTime gameTime)
        {
            base.Exit(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            hades.PreviousWorldPosition = hades.worldPosition;
            hades.WorldPosition += Utility.ScaleVectorTo(hades.movementDirection, hades.MovementSpeed);
            stateFinished = CollisionManager.IsCollisionAfterMove(hades, true, true);
            stateFinished = stateFinished || ((startPosition-hades.WorldPosition).Length()>patrolLength);
        }
    }
}
