using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using C3.MonoGame;

using EVCMonoGame.src.gui;
using EVCMonoGame.src.scenes;
using EVCMonoGame.src.animation;
using EVCMonoGame.src.collision;

namespace EVCMonoGame.src.characters
{
    // TODO: CombatArgs irgendwie initialisieren.

    public abstract class Character : scenes.IUpdateable, scenes.IDrawable, Collidable/* ,CombatCollidable */
    {
        #region Fields

        protected Vector2 worldPosition;
        protected Rectangle collisionBox;

        // Stats
        String name;
        protected int maxHp;
        protected int currentHp;
        protected int maxMp;
        protected int currentMp;
        protected int strength;
        protected int defense;
        protected int intelligence;
        protected int agility;
        protected float movementSpeed = 8;
        private Healthbar healthbar;


        protected Vector2 collisionBoxOffset = Vector2.Zero;
        protected AnimatedSprite sprite;
        protected CombatArgs combatArgs;

        #endregion

        #region Properties

        public bool DoUpdate
        {
            get; set;
        } = true;

        public Healthbar Healthbar
        {
            get { return healthbar; }
        }

        public int CurrentHp
        {
            get { return currentHp; }
            set
            {
                currentHp = value;
                Healthbar.CurrentHp = value;
            }
        }

        public int MaxHp
        {
            get { return maxHp; }
        }

        public bool IsAlive
        {
            get { return currentHp > 0; }
        }

        public AnimatedSprite Sprite
        {
            get { return sprite; }
        }

        public CombatArgs CombatArgs
        {
            get { return combatArgs; }
        }

        public bool HasActiveAttackBounds
        {
            get; protected set;
        }

        public bool HasActiveHurtBounds
        {
            get; protected set;
        }

        public Vector2 WorldPosition
        {
            set
            {
                worldPosition = value;

                collisionBox.Location = value.ToPoint();
                collisionBox.Location += collisionBoxOffset.ToPoint();

                sprite.Position = value;
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
                collisionBox = value;
                worldPosition = value.Location.ToVector2();
            }
            get
            {
                return collisionBox;
            }
        }

        #endregion

        public Character
        (
            String name,
            int maxHp,
            int currentHp,
            int maxMp,
            int currentMp,
            int strength,
            int defense,
            int intelligence,
            int agility,
            float movementSpeed,
            Vector2 position
        )
        {
            this.name = name;
            this.maxHp = maxHp;
            this.currentHp = currentHp;
            this.maxMp = maxMp;
            this.currentMp = currentMp;
            this.strength = strength;
            this.defense = defense;
            this.intelligence = intelligence;
            this.agility = agility;
            this.movementSpeed = movementSpeed;

            healthbar = new Healthbar(maxHp, currentHp, Vector2.Zero, new Vector2(100, 10));
			sprite = new AnimatedSprite(position, 5.0f);

            CollisionManager.AddCollidable(this, CollisionManager.obstacleCollisionChannel);

            WorldPosition = position;
        }

        public virtual void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
			healthbar.Position = sprite.Position - new Vector2(0, healthbar.Size.Y);

			// collisionBox = sprite.Bounds;
		}

		public virtual void OnMove()
		{
			healthbar.Position = sprite.Position - new Vector2(0, healthbar.Size.Y);
		}

        public virtual void LoadContent(ContentManager content)
        {
            sprite.LoadContent(content);
            //collisionBox = sprite.Bounds;

            healthbar.LoadContent(content);
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Draw(gameTime, spriteBatch);
            healthbar.Draw(gameTime, spriteBatch);
        }

		public virtual void Attack(Character target)
		{

		}

		public virtual void OnDamage(float ammount)
		{
			Healthbar.CurrentHp -= (int)ammount;
		}

		#region CombatCollidable
		//public virtual void OnCombatCollision(CombatArgs combatArgs)
		//{
		//    enemyHealthbar.CurrentHp -= combatArgs.damage;
		//    enemySprite.Position += combatArgs.knockBack;
		//}

		#endregion
	}
}
