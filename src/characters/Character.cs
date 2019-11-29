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
        public enum CombatantType
        {
            PLAYER,
            ENEMY,
            MISSILE,
            UNDEFINED
        }

    public abstract class Character : scenes.IUpdateable, scenes.IDrawable, Collidable, CombatCollidable, ITranslatable
    {


        #region Fields

        public Vector2 worldPosition;
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
        public float movementSpeed = 8;
        private Healthbar healthbar;
        public CombatantType combatant;


        protected Vector2 collisionBoxOffset = Vector2.Zero;
        protected AnimatedSprite sprite;
        protected CombatArgs combatArgs;
        public bool flinching = false;

        private List<long> receivedAttackIds = new List<long>();

        #endregion

        #region Properties

        public int MaxHp { get { return maxHp; } }
        public int CurrentHp { get { return currentHp; } }
        public int MaxMp { get { return maxMp; } }
        public int CurrentMp { get { return currentMp; } }
        public int Strength { get { return strength; } }
        public int Defense { get { return defense; } }
        public int Intelligence { get { return intelligence; } }
        public int Agility { get { return agility; } }
        public float MovementSpeed { get { return movementSpeed; } }
        public CombatantType Combatant{ get { return combatant; } }

        public Vector2 Position
        {
            get { return worldPosition; }
            set { worldPosition = value; }
        }

        public bool FlaggedForRemove
        {
            get; set;
        } = false;

        public bool DoUpdate
        {
            get; set;
        } = true;

        public bool DrawHealthbar
        {
            get; set;
        } = true;

        public Healthbar Healthbar
        {
            get { return healthbar; }
        }

        public AnimatedSprite Sprite
        {
            get { return sprite; }
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

        public Rectangle HurtBounds
        {
            get { return sprite.CurrentHurtBounds; }
        }

        public Rectangle AttackBounds
        {
            get { return sprite.CurrentAttackBounds; }
        }

        public bool HasActiveAttackBounds
        {
            get; set;
        } = true;

        public bool HasActiveHurtBounds
        {
            get; set;
        } = true;

        public bool IsAlive
        {
            get { return currentHp > 0; }
        }

        public CombatArgs CombatArgs
        {
            get { return combatArgs; }
            set { combatArgs = value; }
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
            Vector2 position,
            CombatantType characterType
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
            this.combatant = characterType;
            healthbar = new Healthbar(maxHp, currentHp, Vector2.Zero, new Vector2(150, 10));
            sprite = new AnimatedSprite(position, 5.0f);

            CollisionManager.AddCollidable(this, CollisionManager.obstacleCollisionChannel);
            CollisionManager.AddCombatCollidable(this);

            WorldPosition = position;

            combatArgs = new CombatArgs(this, null, CombatantType.UNDEFINED);
            combatArgs.damage = 50;
            combatArgs.knockBack = new Vector2(50, 0);
        }

        public virtual void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
            healthbar.Position = sprite.Position - new Vector2(0, healthbar.Size.Y);
            // collisionBox = sprite.Bounds;
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

            if (DrawHealthbar)
            {
                healthbar.Draw(gameTime, spriteBatch);
            }
        }

        public virtual void OnCombatCollision(CombatArgs combatArgs)
        {
            //enemyHealthbar.CurrentHp -= combatArgs.damage;
            //enemySprite.Position += combatArgs.knockBack;

            Console.WriteLine("id: " + combatArgs.id);

            if (!receivedAttackIds.Contains(combatArgs.id) && combatArgs.victim == this && combatArgs.targetType==this.combatant)
            {
                currentHp -= combatArgs.damage;
                healthbar.CurrentHp = currentHp;

                sprite.Position += combatArgs.knockBack;

                receivedAttackIds.Add(combatArgs.id);

                if (currentHp <= 0.0f)
                    OnDeath();
            }
            else
            {
                // Reset this to true when the attack is over.
                // This is to prevent one attack be counted
                // as multiple attacks.
                HasActiveAttackBounds = false;
            }
        }

        public virtual void OnDeath()
        {

        }
    }
}
