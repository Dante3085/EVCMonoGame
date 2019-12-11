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

namespace EVCMonoGame.src.statemachine.defender
{
    class StatePatrol : State
    {
        public float patrolLength;
        public Vector2 startPosition;
        public StateManagerDefender stateManagerDefender;
        public Defender defender;
        private Random ran = new Random();
        public bool stateFinished = false;
        public StatePatrol(Defender defender, params Transition[] transitions)
            : base("Patrol", transitions)
        {
            this.defender = defender;

        }
        public override void Enter(GameTime gameTime)
        {
            stateFinished = false;
            base.Enter(gameTime);
            startPosition = defender.WorldPosition;
            Vector2 nextPatrolDirection = new Vector2(ran.Next(-100, 100), ran.Next(-100, 100));
            defender.movementDirection = Utility.ScaleVectorTo(nextPatrolDirection, defender.movementSpeed);
            patrolLength = ran.Next((int)defender.sightRange / 2, (int)defender.sightRange);
            float orientationAngle = Utility.GetAngleOfVectorInDegrees(defender.movementDirection);
            defender.Sprite.SetAnimation("RUN_RIGHT");
            if (orientationAngle > (0) && orientationAngle <= (90))
            {
                defender.Sprite.SetAnimation("RUN_RIGHT");
            }
            if (orientationAngle > (90) && orientationAngle <= (180))
            {
                defender.Sprite.SetAnimation("RUN_LEFT");
            }
            if (orientationAngle > (-180) && orientationAngle <= (-90))
            {
                defender.Sprite.SetAnimation("RUN_LEFT");
            }
            if (orientationAngle > (-90) && orientationAngle <= (0))
            {
                defender.Sprite.SetAnimation("RUN_RIGHT");
            }
        }

        public override void Exit(GameTime gameTime)
        {
            base.Exit(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            defender.PreviousWorldPosition = defender.worldPosition;
            defender.WorldPosition += Utility.ScaleVectorTo(defender.movementDirection, defender.MovementSpeed);
            stateFinished = CollisionManager.IsCollisionAfterMove(defender, true, true);
            stateFinished = stateFinished || ((startPosition-defender.WorldPosition).Length()>patrolLength);
        }
    }
}
