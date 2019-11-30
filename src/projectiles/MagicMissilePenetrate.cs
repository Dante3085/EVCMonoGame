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
    public class MagicMissilePenetrate : MagicMissile
    {

        private int penetrationCounter = 0;
        private int maxPenetrations;
        private List<CombatCollidable> hittedVictims = new List<CombatCollidable>();

        public MagicMissilePenetrate(Vector2 position, Orientation orientation, 
            float movementSpeed = 10, int maxPenetrations = 3)
        {
            this.maxPenetrations = maxPenetrations;
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
            if (CollisionManager.IsCollisionWithWall(this))
            {
                FlaggedForRemove = true;
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
            sprite.SetAnimation("MAGIC_MISSILE_LEFT");
            switch (orientation)
            {
                case Orientation.LEFT:
                    sprite.SetAnimation("MAGIC_MISSILE_LEFT");
                    break;
                case Orientation.UP_LEFT:
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
                    break;
                case Orientation.DOWN:
                    sprite.SetAnimation("MAGIC_MISSILE_DOWN");
                    break;
                case Orientation.DOWN_LEFT:
                    break;
            }
        }

        public override void LoadContent(ContentManager content)
        {
            sprite.LoadContent(content);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Draw(gameTime, spriteBatch);
        }
        public override void Update(GameTime gameTime)
        {
            WorldPosition += movementVector;
            sprite.WorldPosition = WorldPosition;
            collisionBox.Location = (WorldPosition + collisionBoxOffset).ToPoint();
            sprite.Update(gameTime);

            CollisionManager.CheckCombatCollisions(this);


            if (CollisionManager.IsCollisionWithWall(this) || FlaggedForRemove || 
                penetrationCounter>=maxPenetrations)
            {
                FlaggedForRemove = true;
                CollisionManager.RemoveCollidable(this, CollisionManager.projectileCollisionChannel);
                CollisionManager.RemoveCombatCollidable(this);
            }
        }


        public override void OnCombatCollision(CombatArgs combatArgs)
        {
            Console.WriteLine("Combat args id:" + combatArgs.id);
            if(!hittedVictims.Exists((a)=> { return a == combatArgs.victim; }))
            {
                hittedVictims.Add(combatArgs.victim);
                penetrationCounter++;
            }
            
        }
    }
}