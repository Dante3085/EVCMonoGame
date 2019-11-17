using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        private Vector2 worldPosition;
        private Rectangle collisionBox;

        // Stats
        String name;
        protected int strength;
        protected int defense;
        protected int intelligence;
        protected int agility;
        protected int maxHp;
        protected int currentHp;
        protected int maxMp;
        protected int currentMp;
        protected float movementSpeed = 8;
        private Healthbar healthbar;

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
                worldPosition.X = (int)value.X;
                worldPosition.Y = (int)value.Y;
                collisionBox.X = (int)value.X;
                collisionBox.Y = (int)value.Y;
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

        public Character(int maxHp, int currentHp, Vector2 position)
        {
            healthbar = new Healthbar(maxHp, currentHp, Vector2.Zero, new Vector2(100, 20));

			WorldPosition = position;
			CollisionBox = new Rectangle(WorldPosition.ToPoint(), new Point(50, 50));

            sprite = new AnimatedSprite(position, 5.0f);


            CollisionManager.AddCollidable(this, CollisionManager.obstacleCollisionChannel);
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
            collisionBox = sprite.Bounds;

            healthbar.LoadContent(content);
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Draw(gameTime, spriteBatch);
            healthbar.Draw(gameTime, spriteBatch);
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
