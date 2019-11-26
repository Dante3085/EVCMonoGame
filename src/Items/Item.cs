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
using EVCMonoGame.src.animation;

namespace EVCMonoGame.src.Items
{
	public abstract class Item : scenes.IUpdateable, scenes.IDrawable, Collidable
	{
		public Vector2 worldPosition;
		public Rectangle geoHitbox;
        public AnimatedSprite sprite;

		public bool DoUpdate { get; set; }

        // GeometryCollidable
        #region Properties

        public bool FlaggedForRemove
        {
            get; set;
        } = false;

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
            sprite = new AnimatedSprite(worldPosition);
            sprite.LoadAnimationsFromFile("Content/rsrc/spritesheets/configFiles/coin.anm.txt");
            sprite.SetAnimation("COIN");
			CollisionBox = bounds;
			CollisionManager.AddCollidable(this, CollisionManager.itemCollisionChannel);
		}
		public abstract void PickUp(Player player);


		public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
            sprite.Draw(gameTime, spriteBatch);
		}

		public virtual void LoadContent(ContentManager content)
		{
            sprite.LoadContent(content);
		}

		public virtual void Update(GameTime gameTime)
		{
            sprite.WorldPosition = worldPosition;
            sprite.Update(gameTime);
		}

	}
}
