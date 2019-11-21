using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C3.MonoGame;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.collision;
using EVCMonoGame.src.scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EVCMonoGame.src.Items
{
	public abstract class Item : scenes.IUpdateable, scenes.IDrawable, Collidable
	{
		public Vector2 worldPosition;
		public Rectangle geoHitbox;

		public bool DoUpdate { get; set; }

		// GeometryCollidable
		#region Properties
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
		#endregion

		public Item(Rectangle bounds)
		{
			CollisionBox = bounds;
			CollisionManager.AddCollidable(this, CollisionManager.itemCollisionChannel);
		}
		public abstract void PickUp(Player player);


		public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{

		}

		public virtual void LoadContent(ContentManager content)
		{

		}

		public virtual void Update(GameTime gameTime)
		{
		}

	}
}
