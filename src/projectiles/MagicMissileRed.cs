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
    public class MagicMissileRed : Collidable, scenes.IDrawable, scenes.IUpdateable, CombatCollidable
    {
        public static ContentManager content;
        private Vector2 movementVector;
        public Vector2 WorldPosition { get; set; }
        public Vector2 PreviousWorldPosition { get; set; }
        public Rectangle collisionBox;
        public Rectangle CollisionBox { get { return collisionBox; } set { collisionBox = value; } }
        public bool DoUpdate { get; set; }
        public bool FlaggedForRemove
        {
            get; set;
        } = false;

        public Rectangle HurtBounds { get { return Rectangle.Empty; } }

        public Rectangle AttackBounds { get { return sprite.CurrentAttackBounds; } }

        public bool HasActiveAttackBounds { get { return true; } }

        public bool HasActiveHurtBounds { get { return false; } }

        public bool IsAlive => throw new NotImplementedException();

        public CombatArgs CombatArgs => throw new NotImplementedException();

        AnimatedSprite sprite;

        public MagicMissileRed(Vector2 position, Orientation orientation, int movementSpeed = 10)
        {
            WorldPosition = position;
            sprite = new AnimatedSprite(position, 3);
            sprite.LoadAnimationsFromFile("Content/rsrc/spritesheets/configFiles/magic_missile_red.anm.txt", true);
            sprite.SetAnimation("MAGIC_MISSILE_RED_RIGHT");
            CollisionBox = new Rectangle((sprite.WorldPosition + (new Vector2(44, 11) * sprite.Scale)).ToPoint(),
                new Point(20 * (int)sprite.Scale, 20 * (int)sprite.Scale));
            CollisionManager.AddCollidable(this, CollisionManager.obstacleCollisionChannel);
            CollisionManager.AddCombatCollidable(this);
            setMovementVector(orientation, movementSpeed);
        }

        public void setMovementVector(Orientation orientation, int movementSpeed)
        {
            switch (orientation)
            {
                case Orientation.LEFT:
                    movementVector = new Vector2(-10, 0);
                    break;
                case Orientation.UP_LEFT:
                    movementVector = new Vector2(-10, -10);
                    break;
                case Orientation.UP:
                    movementVector = new Vector2(0, -10);
                    break;
                case Orientation.UP_RIGHT:
                    movementVector = new Vector2(10, -10);
                    break;
                case Orientation.RIGHT:
                    movementVector = new Vector2(10, 0);
                    break;
                case Orientation.DOWN_RIGHT:
                    movementVector = new Vector2(10, 10);
                    break;
                case Orientation.DOWN:
                    movementVector = new Vector2(0, 10);
                    break;
                case Orientation.DOWN_LEFT:
                    movementVector = new Vector2(-10, 10);
                    break;
            }
            movementVector = Utility.ScaleVectorTo(movementVector, movementSpeed);
        }

        public void LoadContent(ContentManager content)
        {
            sprite.LoadContent(content);
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Draw(gameTime, spriteBatch);
        }
        public void Update(GameTime gameTime)
        {
            PreviousWorldPosition = WorldPosition;
            WorldPosition += movementVector;
            sprite.WorldPosition = WorldPosition;
            collisionBox.Location = (WorldPosition + new Vector2(44, 11) * sprite.Scale).ToPoint();
            sprite.Update(gameTime);
            if (CollisionManager.IsCollisionAfterMove(this, false, false))
            {
                FlaggedForRemove = true;
            }
        }

        public void OnCombatCollision(CombatArgs combatArgs)
        {
            throw new NotImplementedException();
        }
    }
}
