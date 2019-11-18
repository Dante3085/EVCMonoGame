﻿using System;
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

namespace EVCMonoGame.src.statemachine
{
    public abstract class StateManager : scenes.IUpdateable
    {
        List<State> states;
        State currentState;

        public bool DoUpdate
        {
            get; set;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (currentState.transitions.Exists((a) => { return a.checkCondition(); }))// check if transition is necessery
            {
                currentState.Exit(gameTime);
                String idNextState = currentState.transitions.Find((a) => { return a.checkCondition(); }).nextStateId; //get next State
                currentState = states.Find((a) => { return idNextState.Equals(a.stateId); });
                currentState.Enter(gameTime);
            }
            else
            {
                currentState.Update(gameTime);
            }
        }
    }
}
