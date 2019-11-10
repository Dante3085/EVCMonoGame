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
using C3.MonoGame;

namespace EVCMonoGame.src
{
	public class Character : Updateable, scenes.IDrawable, Collision
	{
		#region Variables
		public Vector2 worldPosition;
		public Rectangle geoHitbox;
		public Rectangle attackHitbox;
		public Vector2 movementDirection;

		// Stats
		protected float movementSpeed;
		protected float attackSpeed = 1000.0f; // in mili
		protected float attackDmg = 10;
		protected float cooldownOnAttack = 0.0f; // in mili
		protected bool isAttackOnCooldown = false;
		#endregion

		public Healthbar Healthbar { get; set; }


		public Character(Rectangle bounds)
		{
			CollisionBox = bounds;
		}

		#region Properties
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
		
		public Rectangle CollisionBox {
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

		public Rectangle AttackHitbox
		{
			get
			{
				Rectangle bounds = new Rectangle(0, 0, (int)(CollisionBox.Width * 1.2), (int)(CollisionBox.Height * 1.2));
				bounds.X = geoHitbox.Center.X - bounds.Width / 2;
				bounds.Y = geoHitbox.Center.Y - bounds.Height / 2;
				return bounds;
			}
		}
		#endregion

		// Draw
		public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			Healthbar.Draw(gameTime, spriteBatch);
			Primitives2D.DrawRectangle(spriteBatch, AttackHitbox, Color.DarkRed);
		}
		public virtual void LoadContent(ContentManager content)
		{
			Healthbar = new Healthbar(100, 80, WorldPosition - new Vector2(0, 35), new Vector2(100, 10));
			Healthbar.LoadContent(content);
		}
		

		// Update
		public override void Update(GameTime gameTime)
		{
			if (isAttackOnCooldown)
			{
				cooldownOnAttack -= gameTime.ElapsedGameTime.Milliseconds;
				if (cooldownOnAttack <= 0.0)
					isAttackOnCooldown = false;
			}
		}

		public virtual void OnMove()
		{
			Healthbar.Position = WorldPosition - new Vector2(0, Healthbar.Size.Y);
		}

		// Events
		public virtual void OnGeometryCollision(IGeometryCollision collider)
		{
			OnMove();
		}

		public virtual void Attack(Character target)
		{

		}

		// To Do - DMGInvestigatior, aber geht kein Charakter falls Fallen DMG aufrufen
		public virtual void OnDamage(float ammount)
		{
			Healthbar.CurrentHp -= (int)ammount;
		}

	}
}
