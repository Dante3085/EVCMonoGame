﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using C3.MonoGame;

using EVCMonoGame.src.scenes;
using EVCMonoGame.src.states;
using EVCMonoGame.src.input;
using EVCMonoGame.src.collision;
using EVCMonoGame.src.characters;

namespace EVCMonoGame.src
{
    public class Lever : scenes.IDrawable, Interactable
    {
        protected Texture2D texture;
        private Vector2 position;

        protected Rectangle deactivatedTextureRec = new Rectangle(368, 128, 16, 16);
        protected Rectangle activatedTextureRec = new Rectangle(384, 128, 16, 16);

        protected Rectangle interactionArea;

        private Rectangle screenBounds;

        private bool activated = false;

        public bool Activated
        {
            get { return activated; }
            set { activated = value; }
        }

        public bool BlockPlayerInteraction
        {
            get; set;
        } = false;

        public Rectangle Bounds
        {
            get { return screenBounds; }
		}

        public bool DoUpdate
        {
            get; set;
        } = true;
		public Rectangle InteractableBounds { get => interactionArea; set => interactionArea = value; }

		public Lever(Vector2 position)
        {
            this.position = position;

            interactionArea = new Rectangle(position.ToPoint(), new Point(128, 128));
            interactionArea.Inflate(100, 100);

            screenBounds = new Rectangle((int)position.X, (int)position.Y, 128, 128);

			CollisionManager.AddInteractable(this);
        }

		public virtual void Interact(Player player)
		{
			if (BlockPlayerInteraction)
				return;

			activated = !activated;
		}

		public virtual void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("rsrc/tilesets/FF6MapTileCastles");
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (activated)
            {
                spriteBatch.Draw(texture, screenBounds, activatedTextureRec, Color.White);
            }
            else
            {
                spriteBatch.Draw(texture, screenBounds, deactivatedTextureRec, Color.White);
            }

            //Color color = Color.RosyBrown;
            //color.A = 20;
            //Primitives2D.FillRectangle(spriteBatch, interactionArea, color);
        }


	}
}
