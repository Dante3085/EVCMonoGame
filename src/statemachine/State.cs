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

namespace EVCMonoGame.src.statemachine
{
    public abstract class State : scenes.IUpdateable
    {
        public String stateId;
        public StateManager stateManager;
        public List<Transition> transitions;
        public bool DoUpdate
        {
            get; set;
        }

        public State(StateManager stateManager, String stateId)
        {
            this.stateManager = stateManager;
            this.stateId = stateId;
        }

        public enum MethodStage
        {
            Enter,
            Normal,
            Exit

        }
        public MethodStage methodStage;

        public virtual void Enter(GameTime gameTime) { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Exit(GameTime gameTime) { }

    }
}
