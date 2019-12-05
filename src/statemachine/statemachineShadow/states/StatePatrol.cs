using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using EVCMonoGame.src.collision;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.characters.enemies;

namespace EVCMonoGame.src.statemachine.shadow
{
    class StatePatrol : State
    {
        Vector2 targetPoint = Vector2.One;
        List<Point> waypoints= new List<Point>();
        public StateManagerShadow stateManagerShadow;
        public Shadow shadow;
        public StatePatrol(Shadow shadow, params Transition[] transitions)
            : base("Attack", transitions)
        {
            this.shadow = shadow;
            
        }
        public override void Enter(GameTime gameTime)
        {
            base.Enter(gameTime);
            //
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
