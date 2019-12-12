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
    public class MagicMissileBounce : MagicMissile
    {
        private int bounceCounter = 0;
        private int maxBounces;

        public MagicMissileBounce(Vector2 position, Orientation orientation, float movementSpeed = 10, int maxBounces = 5)
        {
            this.maxBounces = maxBounces;
            this.movementSpeed = movementSpeed;
            this.orientation = orientation;
            sprite = new AnimatedSprite(position, 3);
            setCollisionBoxOffset();
            WorldPosition = position - collisionBoxOffset;
            sprite.Position = (WorldPosition);
            sprite.LoadAnimationsFromFile("Content/rsrc/spritesheets/configFiles/magic_missile_green.anm.txt", true);
            setAnimation();
            CollisionBox = new Rectangle((sprite.WorldPosition + (collisionBoxOffset)).ToPoint(),
                new Point(20 * (int)sprite.Scale, 20 * (int)sprite.Scale));


            setMovementVector(movementSpeed, this.orientation);

            combatArgs = new CombatArgs(this, null, CombatantType.ENEMY);
            combatArgs.damage = 50;
            combatArgs.atackOrientation = orientation;
            if (CollisionManager.IsCollisionWithWall(this))
            {
                FlaggedForRemove = true;
                doDraw = false;
            }
            else
            {
                CollisionManager.AddCollidable(this, CollisionManager.projectileCollisionChannel);
                CollisionManager.AddCombatCollidable(this);
            }
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
                    collisionBoxOffset = new Vector2(25, 19) * sprite.Scale;
                    break;
                case Orientation.UP:
                    collisionBoxOffset = new Vector2(10, 10) * sprite.Scale;
                    break;
                case Orientation.UP_RIGHT:
                    collisionBoxOffset = new Vector2(27, 12) * sprite.Scale;
                    break;
                case Orientation.RIGHT:
                    collisionBoxOffset = new Vector2(44, 11) * sprite.Scale;
                    break;
                case Orientation.DOWN_RIGHT:
                    collisionBoxOffset = new Vector2(16, 20) * sprite.Scale;
                    break;
                case Orientation.DOWN:
                    collisionBoxOffset = new Vector2(17, 20) * sprite.Scale;
                    break;
                case Orientation.DOWN_LEFT:
                    collisionBoxOffset = new Vector2(19, 19) * sprite.Scale;
                    break;
            }
        }

        public void setAnimation()
        {
            sprite.SetAnimation("MAGIC_MISSILE_RIGHT");
            switch (orientation)
            {
                case Orientation.LEFT:
                    sprite.SetAnimation("MAGIC_MISSILE_LEFT");
                    break;
                case Orientation.UP_LEFT:
                    sprite.SetAnimation("MAGIC_MISSILE_UP_LEFT");
                    break;
                case Orientation.UP:
                    sprite.SetAnimation("MAGIC_MISSILE_UP");
                    break;
                case Orientation.UP_RIGHT:
                    sprite.SetAnimation("MAGIC_MISSILE_UP_RIGHT");
                    break;
                case Orientation.RIGHT:
                    sprite.SetAnimation("MAGIC_MISSILE_RIGHT");
                    break;
                case Orientation.DOWN_RIGHT:
                    sprite.SetAnimation("MAGIC_MISSILE_DOWN_RIGHT");
                    break;
                case Orientation.DOWN:
                    sprite.SetAnimation("MAGIC_MISSILE_DOWN");
                    break;
                case Orientation.DOWN_LEFT:
                    sprite.SetAnimation("MAGIC_MISSILE_DOWN_LEFT");
                    break;
            }
        }

        public override void LoadContent(ContentManager content)
        {
            sprite.LoadContent(content);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (doDraw)
            {
                sprite.Draw(gameTime, spriteBatch);
            }
        }
        public override void Update(GameTime gameTime)
        {
            PreviousWorldPosition = WorldPosition;
            WorldPosition += movementVector;
            sprite.WorldPosition = WorldPosition;
            collisionBox.Location = (WorldPosition + collisionBoxOffset).ToPoint();
            sprite.Update(gameTime);

            CollisionManager.CheckCombatCollisions(this);


            if (CollisionManager.IsCollisionWithWall(this))
            {
                
                bounceCounter++;
                this.orientation=GetBounceOrientation(CollisionManager.GetCollidingWall(this));
                setMovementVector(movementSpeed, orientation);
                setAnimation();
                Vector2 previousCollisionBoxPosition = collisionBox.Location.ToVector2();
                Vector2 prevToNowWorldPosi = worldPosition - PreviousWorldPosition;
                setCollisionBoxOffset();
                collisionBox.Location = (worldPosition + collisionBoxOffset).ToPoint();
                WorldPosition += (previousCollisionBoxPosition-collisionBox.Location.ToVector2());
                WorldPosition -= prevToNowWorldPosi;
                WorldPosition += prevToNowWorldPosi;
                CollisionManager.ResolveCollisionWithWall(this);
                combatArgs.atackOrientation = orientation;
            }
            if (bounceCounter >= maxBounces || FlaggedForRemove)
            {
                FlaggedForRemove = true;
                CollisionManager.RemoveCollidable(this, CollisionManager.projectileCollisionChannel);
                CollisionManager.RemoveCombatCollidable(this);
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
                    }
                    else if (distanceHorizontal < distanceVertical)
                    {
                        return Orientation.DOWN_LEFT;
                    }
                    return Orientation.DOWN_RIGHT;
                    break;
                case Orientation.UP:
                    return Orientation.DOWN;
                    break;
                case Orientation.UP_RIGHT:
                    distanceVertical = Math.Abs(this.CollisionBox.Right - bounceOff.Left);
                    distanceHorizontal = Math.Abs(this.CollisionBox.Top - bounceOff.Bottom);
                    if (distanceVertical < distanceHorizontal)
                    {
                        return Orientation.UP_LEFT;
                    }
                    else if (distanceHorizontal < distanceVertical)
                    {
                        return Orientation.DOWN_RIGHT;
                    }
                    return Orientation.DOWN_LEFT;
                    break;
                case Orientation.RIGHT:
                    return Orientation.LEFT;
                    break;
                case Orientation.DOWN_RIGHT:
                    distanceVertical = Math.Abs(this.CollisionBox.Right - bounceOff.Left);
                    distanceHorizontal = Math.Abs(this.CollisionBox.Bottom - bounceOff.Top);
                    if (distanceVertical < distanceHorizontal)
                    {
                        return Orientation.DOWN_LEFT;
                    }
                    else if (distanceHorizontal < distanceVertical)
                    {
                        return Orientation.UP_RIGHT;
                    }
                    return Orientation.UP_LEFT;
                    break;
                case Orientation.DOWN:
                    return Orientation.UP;
                    break;
                case Orientation.DOWN_LEFT:
                    distanceVertical = Math.Abs(this.CollisionBox.Left - bounceOff.Right);
                    distanceHorizontal = Math.Abs(this.CollisionBox.Bottom - bounceOff.Top);
                    if (distanceVertical < distanceHorizontal)
                    {
                        return Orientation.DOWN_RIGHT;
                    }
                    else if (distanceHorizontal < distanceVertical)
                    {
                        return Orientation.UP_LEFT;
                    }
                    return Orientation.UP_RIGHT;
                    break;
            }
            return Orientation.LEFT;
        }

        public override void OnCombatCollision(CombatArgs combatArgs)
        {
            Console.WriteLine("Combat args id:" + combatArgs.id);

            FlaggedForRemove = true;

        }
    }
}