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
        private int bounceCounter = 0;
        Orientation orientation;
        private CombatArgs combatArgs;
        Vector2 collisionBoxOffset;
        public static ContentManager content;
        private Vector2 movementVector;
        private float movementSpeed;
        public Vector2 WorldPosition { get; set; }
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

        public bool IsAlive => throw new NotImplementedException();

        public CombatArgs CombatArgs
        {
            get { return combatArgs; }
        }

        AnimatedSprite sprite;

        public MagicMissileRed(Vector2 position, Orientation orientation, float movementSpeed = 10)
        {
            this.movementSpeed = movementSpeed;
            this.orientation = orientation;
            sprite = new AnimatedSprite(position, 3);
            setCollisionBoxOffset();
            WorldPosition = position - collisionBoxOffset;
            sprite.Position = (WorldPosition);
            sprite.LoadAnimationsFromFile("Content/rsrc/spritesheets/configFiles/magic_missile_red.anm.txt", true);
            setAnimation();
            CollisionBox = new Rectangle((sprite.WorldPosition + (collisionBoxOffset)).ToPoint(),
                new Point(20 * (int)sprite.Scale, 20 * (int)sprite.Scale));

            CollisionManager.AddCollidable(this, CollisionManager.projectileCollisionChannel);
            CollisionManager.AddCombatCollidable(this);

            setMovementVector(movementSpeed, this.orientation);

            combatArgs = new CombatArgs(this, null, CombatantType.ENEMY);
            combatArgs.damage = 50;
        }

        public void setMovementVector(float movementSpeed, Orientation orientation)
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
        public void setCollisionBoxOffset()
        {
            collisionBoxOffset = new Vector2(44, 11) * sprite.Scale;
            switch (orientation)
            {
                case Orientation.LEFT:
                    collisionBoxOffset = new Vector2(20, 11) * sprite.Scale;
                    break;
                case Orientation.UP_LEFT:
                    break;
                case Orientation.UP:
                    collisionBoxOffset = new Vector2(10, 10) * sprite.Scale;
                    break;
                case Orientation.UP_RIGHT:
                    break;
                case Orientation.RIGHT:
                    collisionBoxOffset = new Vector2(44, 11) * sprite.Scale;
                    break;
                case Orientation.DOWN_RIGHT:
                    break;
                case Orientation.DOWN:
                    collisionBoxOffset = new Vector2(17, 20) * sprite.Scale;
                    break;
                case Orientation.DOWN_LEFT:
                    break;
            }
        }

        public void setAnimation()
        {
            sprite.SetAnimation("MAGIC_MISSILE_RED_LEFT");
            switch (orientation)
            {
                case Orientation.LEFT:
                    sprite.SetAnimation("MAGIC_MISSILE_RED_LEFT");
                    break;
                case Orientation.UP_LEFT:
                    break;
                case Orientation.UP:
                    sprite.SetAnimation("MAGIC_MISSILE_RED_UP");
                    break;
                case Orientation.UP_RIGHT:
                    break;
                case Orientation.RIGHT:
                    sprite.SetAnimation("MAGIC_MISSILE_RED_RIGHT");
                    break;
                case Orientation.DOWN_RIGHT:
                    break;
                case Orientation.DOWN:
                    sprite.SetAnimation("MAGIC_MISSILE_RED_DOWN");
                    break;
                case Orientation.DOWN_LEFT:
                    break;
            }
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
            collisionBox.Location = (WorldPosition + collisionBoxOffset).ToPoint();
            sprite.Update(gameTime);

            CollisionManager.CheckCombatCollisions(this);


            if (CollisionManager.IsCollisionWithWall(this) || FlaggedForRemove|| CollisionManager.IsCollisionInChannel(this, CollisionManager.enemyCollisionChannel))
            {
                CollisionManager.IsCollisionAfterMove(this, true, false);
                this.orientation=GetBounceOrientation(CollisionManager.GetCollidingWall(this));
                setMovementVector(movementSpeed, orientation);
                setAnimation();
                setCollisionBoxOffset();
                this.combatArgs.NewId();
                if (++bounceCounter >= 10)
                {
                    FlaggedForRemove = true;
                    CollisionManager.RemoveCollidable(this, CollisionManager.projectileCollisionChannel);
                    CollisionManager.RemoveCombatCollidable(this);
                }
            }
        }

        public Orientation GetBounceOrientation(Rectangle bounceOff)
        {
            float distanceVertical;
            float distanceHorizontal;
            switch (orientation)
            {
                case Orientation.LEFT:
                    return Orientation.RIGHT;
                    break;
                case Orientation.UP_LEFT:
                    distanceVertical = Math.Abs(this.CollisionBox.Left - bounceOff.Right);
                    distanceHorizontal = Math.Abs(this.CollisionBox.Top - bounceOff.Bottom);
                    if (distanceVertical < distanceHorizontal)
                    {
                        return Orientation.UP_RIGHT; 
                    }else if(distanceHorizontal < distanceVertical)
                    {
                        return Orientation.DOWN_LEFT;
                    }
                    return Orientation.DOWN_RIGHT;
                    break;
                case Orientation.UP:
                    return movementVector * (-1);
                    break;
                case Orientation.UP_RIGHT:
                    break;
                case Orientation.RIGHT:
                    return movementVector * (-1);
                    break;
                case Orientation.DOWN_RIGHT:
                    break;
                case Orientation.DOWN:
                    return movementVector * (-1);
                    break;
                case Orientation.DOWN_LEFT:
                    break;
            }
            return Vector2.Zero;
        }

        public void OnCombatCollision(CombatArgs combatArgs)
        {
            Console.WriteLine("Combat args id:" + combatArgs.id);

            //FlaggedForRemove = true;

        }
    }
}
