using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C3.MonoGame;
using EVCMonoGame.src.collision;
using EVCMonoGame.src.scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EVCMonoGame.src.Items
{
	public class Item : Updateable, scenes.IDrawable, Collision
	{
		public Vector2 worldPosition;
		public Rectangle geoHitbox;
		
		// GeometryCollidable
		public Vector2 WorldPosition
		{
			set
			{
				worldPosition.X = (int)value.X;
				worldPosition.Y = (int)value.Y;
				geoHitbox.X = (int)value.X;
				geoHitbox.Y = (int)value.Y;
			}
			get
			{
				return worldPosition;
			}
		}

		public Vector2 PreviousWorldPosition { get; set; }

		public Rectangle CollisionBox
		{
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

		public Item(Rectangle bounds)
		{
			CollisionBox = bounds;
		}

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			//Primitives2D.DrawRectangle(spriteBatch, GeoHitbox, Color.Green);

		}

		public void LoadContent(ContentManager content)
		{

		}

		public override void Update(GameTime gameTime)
		{
		}

	}
}
