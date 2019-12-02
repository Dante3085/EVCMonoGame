using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using C3.MonoGame;

using EVCMonoGame.src.input;
using EVCMonoGame.src.states;

namespace EVCMonoGame.src
{
    public class Door : scenes.IDrawable, scenes.IUpdateable
    {
        private Texture2D texture;
        private Vector2 position;

        private Rectangle openDoorTextureRec = new Rectangle(368, 0, 16, 32);
        private Rectangle closedDoorTextureRec = new Rectangle(384, 0, 16, 32);

        private Rectangle interactionArea;

        private bool open = false;

        public bool Open
        {
            get { return open; }
            set { open = value; }
        }

        public bool BlockPlayerInteraction
        {
            get; set;
        } = false;

        public bool DoUpdate
        {
            get; set;
        } = true;

        public Door(Vector2 position)
        {
            this.position = position;

            interactionArea = new Rectangle(position.ToPoint(), new Point(128, 256));
            interactionArea.Inflate(100, 100);
        }

        public void Update(GameTime gameTime)
        {
            if (BlockPlayerInteraction)
                return;

            if ((InputManager.OnButtonPressed(Buttons.A, PlayerIndex.One)||InputManager.OnKeyPressed(Keys.I)) &&
                GameplayState.PlayerOne.CollisionBox.Intersects(interactionArea))
            {
                open = !open;
            }
            else if ((InputManager.OnButtonPressed(Buttons.A, PlayerIndex.Two) || InputManager.OnKeyPressed(Keys.O)) &&
                     GameplayState.PlayerTwo.CollisionBox.Intersects(interactionArea))
            {
                open = !open;
            }
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("rsrc/tilesets/FF6MapTileCastles");
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (open)
            {
                spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, 128, 256), 
                                 openDoorTextureRec, Color.White);
            }
            else
            {
                spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, 128, 256),
                                 closedDoorTextureRec, Color.White);
            }

            //Color color = Color.Aqua;
            //color.A = 25;
            //Primitives2D.FillRectangle(spriteBatch, interactionArea, color);
        }
    }
}
