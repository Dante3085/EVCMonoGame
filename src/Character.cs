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

		public Vector2 worldPosition;
		public Rectangle geoHitbox;
		
		public Healthbar Healthbar { get; set; }

		// GeometryCollidable
		public Vector2 WorldPosition {
			set
			{
				worldPosition.X = (int)value.X;
				worldPosition.Y = (int)value.Y;
				geoHitbox.X = (int) value.X;
				geoHitbox.Y = (int)value.Y;
			}
			get
			{
				return worldPosition;
			}
		}

		public Vector2 PreviousWorldPosition { get; set; }
		
		public Rectangle GeoHitbox {
			set
			{
				geoHitbox = value;
				worldPosition = value.Location.ToVector2();
			}
			get
			{
				return geoHitbox;
			}
		}


		// Draw
		public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			Healthbar.Draw(gameTime, spriteBatch);
		}
		public virtual void LoadContent(ContentManager content)
		{
			Console.WriteLine(WorldPosition);
			Healthbar = new Healthbar(100, 80, WorldPosition - new Vector2(0, 35), new Vector2(100, 10));
			Healthbar.LoadContent(content);
		}
		

		// Update
		public override void Update(GameTime gameTime)
		{
		}

		public virtual void OnMove()
		{
			Healthbar.Position = WorldPosition - new Vector2(0, Healthbar.Size.Y);
		}

		// Events
		public virtual void OnGeometryCollision(GeometryCollidable collider)
		{
			OnMove();
		}

	}
}
