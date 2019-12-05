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
        Vector2 targetPoint = Vector2.One;
        List<Point> waypoints = new List<Point>();
        public StateManagerShadow stateManagerShadow;
        public Shadow shadow;
        private Random ran = new Random();
        public StatePatrol(Shadow shadow, params Transition[] transitions)
            : base("Attack", transitions)
        {
            this.shadow = shadow;

        }
        public override void Enter(GameTime gameTime)
        {
            base.Enter(gameTime);

            for (int i = 0; i < 1000; i++)
            {
                Vector2 nextPatrolPoint = new Vector2(ran.Next(-100, 100), ran.Next(-100, 100));
                nextPatrolPoint = Utility.ScaleVectorTo(nextPatrolPoint, ran.Next((int)shadow.sightRange / 2, (int)shadow.sightRange));
                if (!CollisionManager.IsBlockedRaycast(shadow, new CollidableHelper(shadow.CollisionBox.Location, shadow.CollisionBox.Size)))
                {
                    break;
                }else if (i > 999)
                {
                    targetPoint = shadow.WorldPosition;
                }
            }
            waypoints = Shadow.pathfinder.Pathfind(shadow.WorldPosition.ToPoint(), targetPoint.ToPoint());
        }

        public override void Exit(GameTime gameTime)
        {
            base.Exit(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


        }
    }
}
