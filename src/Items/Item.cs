﻿using System;
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
using EVCMonoGame.src.states;

namespace EVCMonoGame.src.Items
{
	public abstract class Item : scenes.IUpdateable, scenes.IDrawable, Collidable
	{
		public Vector2 worldPosition;
		public Rectangle geoHitbox;
        public AnimatedSprite sprite;
        public bool isPickedUp = false;
		public bool isInShop = false;
		public int shopPrice;
		public GameplayState.Lane lane;

		//AnimatedSprite
		public String anmConfigFile;
		public String idleAnim;

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

		public Item(Vector2 position, String anmConfigFile, String idleAnim, GameplayState.Lane lane)
		{
			// Sprite
            sprite = new AnimatedSprite(worldPosition);
            sprite.LoadAnimationsFromFile(anmConfigFile);
            sprite.SetAnimation(idleAnim);
			this.anmConfigFile = anmConfigFile;
			this.idleAnim = anmConfigFile;

			// Collision
            CollisionBox = new Rectangle(position.ToPoint(), new Point(50, 50));
			CollisionManager.AddCollidable(this, CollisionManager.itemCollisionChannel);

			this.lane = lane;
		}

		public abstract Item Copy();

		public abstract void PickUp(Player player);


		public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
            if(!isPickedUp)
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
