using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using EVCMonoGame.src.gui;
using EVCMonoGame.src.input;
using EVCMonoGame.src.scenes;
using EVCMonoGame.src.collision;
using EVCMonoGame.src.animation;
using EVCMonoGame.src.utility;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.states;

namespace EVCMonoGame.src.statemachine.riku
{
    public class StateManagerRiku : StateManager
    {
        public StateManagerRiku()
        {
            this.states.Add(new StateIdle(
                    new TransitionOnFlinchAttack("Flinching"),
                    new TransitionAttackButtonPressed("Attacking"),
                    new TransitionOnMoving("Moving")
                )
            );
            this.states.Add(new statemachine.riku.StateMoving(
                    new TransitionOnFlinchAttack("Flinching"),
                    new TransitionAttackButtonPressed("Attacking"),
                    new TransitionOnNotMoving("Idle")
                )
            );
            this.states.Add(new StateFlinching(
                    new TransitionFinishedWithMoving("Moving"), 
                    new TransitionFinishedNotMoving("Idle")
                )
            );
            this.states.Add(new StateAttacking(
                    new TransitionOnFlinchAttack("Flinching"),
                    new TransitionFinishedWithMoving("Moving"),
                    new TransitionFinishedNotMoving("Idle")
                )
            );
            currentState = states.Find((a) => { return a.stateId.Equals("Idle"); });
            

        }

    }
}
