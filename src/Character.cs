using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EVCMonoGame.src.scenes;
using Microsoft.Xna.Framework;
using EVCMonoGame.src.collision;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using EVCMonoGame.src.gui;

namespace EVCMonoGame.src
{
	public abstract class Character : Updateable, scenes.IDrawable, GeometryCollidable
	{

		public Vector2 position;
		public Rectangle bounds;
		
		public Healthbar Healthbar { get; set; }

		// GeometryCollidable
		public Vector2 Position {
			set
			{
				position.X = (int)value.X;
				position.Y = (int)value.Y;
				bounds.X = (int) value.X;
				bounds.Y = (int)value.Y;
			}
			get
			{
				return position;
			}
		}

		public Vector2 PreviousPosition { get; set; }
		
		public Rectangle Bounds {
			set
			{
				bounds = value;
				position = value.Location.ToVector2();
			}
			get
			{
				return bounds;
			}
		}


		// Draw
		public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			Healthbar.Draw(gameTime, spriteBatch);
		}
		public virtual void LoadContent(ContentManager content)
		{
			Console.WriteLine(Position);
			Healthbar = new Healthbar(100, 100, Position - new Vector2(0, 35), new Vector2(100, 10));
			Healthbar.LoadContent(content);
		}
		

		// Update
		public override void Update(GameTime gameTime)
		{
		}

		public virtual void OnMove()
		{
			Healthbar.Position = Position - new Vector2(0, Healthbar.Size.Y);
		}

		// Events
		public virtual void OnGeometryCollision(GeometryCollidable collider)
		{
			OnMove();
		}

	}
}
