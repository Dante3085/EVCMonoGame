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

using EVCMonoGame.src.statemachine;
using EVCMonoGame.src.statemachine.statemachinePlayer.states;

namespace EVCMonoGame.src.statemachine.statemachinePlayer
{
    class StateManagerPlayer : StateManager
    {
        public Player player;
        public StateManagerPlayer(Player player)
        {
            this.player = player;
            states.Add(new StateAttacking(this));
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
