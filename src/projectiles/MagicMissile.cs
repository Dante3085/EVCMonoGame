using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using EVCMonoGame.src.gui;
using EVCMonoGame.src.input;
using EVCMonoGame.src.scenes;
using EVCMonoGame.src.collision;
using EVCMonoGame.src.animation;
using EVCMonoGame.src.utility;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.states;
using EVCMonoGame.src.statemachine.sora;

namespace EVCMonoGame.src.projectiles
{
    public abstract class MagicMissile : Collidable, scenes.IDrawable, scenes.IUpdateable, CombatCollidable
    {
        protected Orientation orientation;
        protected CombatArgs combatArgs;
        protected Vector2 collisionBoxOffset;
        public static ContentManager content;
        protected Vector2 movementVector;
        protected float movementSpeed;
        protected Vector2 worldPosition;
        public Vector2 WorldPosition
        {
            get
            {
                return worldPosition;
            }
            set
            {
                PreviousWorldPosition = worldPosition;
                collisionBox.Location = (worldPosition + collisionBoxOffset).ToPoint();
                worldPosition = value;
            }
        }
        public Vector2 PreviousWorldPosition { get; set; }
        public Rectangle collisionBox;
        public CombatantType combatant = CombatantType.MISSILE;
        public Rectangle CollisionBox { get { return collisionBox; } set { collisionBox = value; } }
        public bool DoUpdate { get; set; }
        public CombatantType Combatant { get { return combatant; } }
        public bool FlaggedForRemove
        {
            get; set;
        } = false;

        public Rectangle HurtBounds { get { return Rectangle.Empty; } }

        public Rectangle AttackBounds { get { return sprite.CurrentAttackBounds; } }

        public bool HasActiveAttackBounds { get { return true; } }

        public bool HasActiveHurtBounds { get { return false; } }

        public bool IsAlive { get { return true; } }

        public CombatArgs CombatArgs
        {
            get { return combatArgs; }
        }

        public AnimatedSprite sprite;

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public abstract void LoadContent(ContentManager content);

        public abstract void Update(GameTime gameTime);

        public abstract void OnCombatCollision(CombatArgs combatArgs);
    }
}
