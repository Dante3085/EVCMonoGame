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
    public class HadesMissile : MagicMissile
    {


        public HadesMissile(Vector2 position, Vector2 directionVector, bool left, float movementSpeed = 10)
        {
            this.movementSpeed = movementSpeed;
            sprite = new AnimatedSprite(position, 5);
            WorldPosition = position;
            sprite.Position = (WorldPosition);
            sprite.LoadAnimationsFromFile("Content/rsrc/spritesheets/configFiles/HadesMissile.anm.txt", true);
            if (left)
            {
                sprite.SetAnimation("HADESMISSILE_LEFT");
            }
            else
            {
                sprite.SetAnimation("HADESMISSILE_RIGHT");
            }
            CollisionBox = AttackBounds;
            movementVector = Utility.ScaleVectorTo(directionVector, movementSpeed);

            combatArgs = new CombatArgs(this, null, CombatantType.PLAYER);
            combatArgs.damage = 150;
            combatArgs.atackOrientation = Orientation.DOWN;
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
            WorldPosition += movementVector;
            sprite.WorldPosition = WorldPosition;
            collisionBox.Location = (WorldPosition).ToPoint();
            sprite.Update(gameTime);

            CollisionManager.CheckCombatCollisions(this);


            if (CollisionManager.IsCollisionWithWall(this) || FlaggedForRemove)
            {
                FlaggedForRemove = true;
                CollisionManager.RemoveCollidable(this, CollisionManager.projectileCollisionChannel);
                CollisionManager.RemoveCombatCollidable(this);
            }
        }


        public override void OnCombatCollision(CombatArgs combatArgs)
        {
            Console.WriteLine("Combat args id:" + combatArgs.id);
            FlaggedForRemove = true;
        }
    }
}
