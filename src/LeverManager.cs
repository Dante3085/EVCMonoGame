using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using C3.MonoGame;
using Microsoft.Xna.Framework.Input;

using EVCMonoGame.src.scenes;
using EVCMonoGame.src.input;
using EVCMonoGame.src.states;
using EVCMonoGame.src.utility;

namespace EVCMonoGame.src
{
    public class LeverManager : scenes.IUpdateable, scenes.IDrawable
    {
        private List<Lever> levers = new List<Lever>();
        private int currentLeverIndex;
        private Rectangle interactionArea;
        private bool horizontalSelection;

        private bool playerOneInteracting = false;
        private bool playerTwoInteracting = false;

        public bool DoUpdate
        {
            get; set;
        } = true;

        public LeverManager(bool horizontalSelection, params Lever[] levers)
        {
            this.horizontalSelection = horizontalSelection;

            foreach (Lever lever in levers)
            {
                lever.BlockPlayerInteraction = true;
                this.levers.Add(lever);
            }



            interactionArea = Utility.CalcMinimalBoundingBox(levers.Select(o => o.Bounds.Location.ToVector2()).Union
                                                            (levers.Select(o => new Vector2(o.Bounds.Right, o.Bounds.Bottom))).ToList());
            interactionArea.Inflate(50, 50);

            currentLeverIndex = 0;
        }

        public void Update(GameTime gameTime)
        {
            if (GameplayState.PlayerOne.CollisionBox.Intersects(interactionArea))
            {
                playerOneInteracting = true;
                UpdateSelection(PlayerIndex.One);
            }
            else
            {
                playerOneInteracting = false;
            }
            
            if (GameplayState.PlayerTwo.CollisionBox.Intersects(interactionArea))
            {
                playerTwoInteracting = true;
                UpdateSelection(PlayerIndex.Two);
            }
            else
            {
                playerTwoInteracting = false;
            }
        }

        private void UpdateSelection(PlayerIndex playerIndex)
        {
            if (horizontalSelection)
            {
                if (InputManager.OnButtonPressed(Buttons.DPadRight, playerIndex))
                    currentLeverIndex = (currentLeverIndex + 1) % levers.Count;
                else if (InputManager.OnButtonPressed(Buttons.DPadLeft, playerIndex))
                    currentLeverIndex = (currentLeverIndex - 1) < 0 ? (levers.Count - 1) : (currentLeverIndex - 1);
            }
            else
            {
                if (InputManager.OnButtonPressed(Buttons.DPadDown, playerIndex))
                    currentLeverIndex = (currentLeverIndex + 1) % levers.Count;
                else if (InputManager.OnButtonPressed(Buttons.DPadUp, playerIndex))
                    currentLeverIndex = (currentLeverIndex - 1) < 0 ? (levers.Count - 1) : (currentLeverIndex - 1);
            }

            if (InputManager.OnButtonPressed(Buttons.A, playerIndex))
            {
                levers[currentLeverIndex].Activated = !levers[currentLeverIndex].Activated;
            }
        }

        public void LoadContent(ContentManager contentManager)
        {

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (playerOneInteracting || playerTwoInteracting)
            {
                Primitives2D.DrawRectangle(spriteBatch, levers[currentLeverIndex].Bounds, Color.DarkRed, 10);
            }

            //Color blue = Color.Blue;
            //blue.A = 10;
            //Primitives2D.FillRectangle(spriteBatch, interactionArea, blue);
        }
    }
}
