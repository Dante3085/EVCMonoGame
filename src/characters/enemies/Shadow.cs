
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using EVCMonoGame.src.Items;

using EVCMonoGame.src.collision;
using EVCMonoGame.src.scenes;
using EVCMonoGame.src.Items;
using EVCMonoGame.src.statemachine.shadow;

namespace EVCMonoGame.src.characters.enemies
{
    public class Shadow : Enemy
    {
        public Shadow(Vector2 position)
            : base
            (
                  name: "Shadow",
                  maxHp: 300,
                  currentHp: 300,
                  maxMp: 0,
                  currentMp: 0,
                  strength: 5,
                  defense: 3,
                  intelligence: 0,
                  agility: 8,
                  movementSpeed: 4,
                  position: position,
                  exp: 50
            )
        {
            sprite.LoadAnimationsFromFile("Content/rsrc/spritesheets/configFiles/shadow.anm.txt");
            sprite.SetAnimation("WALK_DOWN_RIGHT");

            collisionBox.Size = new Point(50, 50);
            collisionBoxOffset = new Vector2(30, 30);
            stateManager = new StateManagerShadow(this);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
        }

        public override void DropLoot()
        {
            base.DropLoot();

            Random random = new Random();
            int rndNum;

            // 10% GodMissile
            rndNum = random.Next(0, 10);
            if (rndNum == 0)
            {
                GodImperatorMissle godMissile = new GodImperatorMissle(WorldPosition);

                Scene.drawablesToAdd.AddRange(new scenes.IDrawable[]
                {
                    godMissile,
                });

                Scene.updateablesToAdd.AddRange(new scenes.IUpdateable[]
                {
                    godMissile,
                });
            }

            // 40% SplitMissile
            else if (rndNum >= 0 && rndNum <= 3)
            {
                SplitMissle splitMissile = new SplitMissle(WorldPosition);

                Scene.drawablesToAdd.AddRange(new scenes.IDrawable[]
                {
                    splitMissile,
                });

                Scene.updateablesToAdd.AddRange(new scenes.IUpdateable[]
                {
                    splitMissile
                });
            }

            // 60% PenetrateMissile
            else if (rndNum >= 0 && rndNum <= 5)
            {
                PenetrateMissle penetrateMissile = new PenetrateMissle(WorldPosition);

                Scene.drawablesToAdd.AddRange(new scenes.IDrawable[]
                {
                    penetrateMissile,
                });

                Scene.updateablesToAdd.AddRange(new scenes.IUpdateable[]
                {
                    penetrateMissile
                });
            }

            // 80% BounceMissile
            else if (rndNum >= 0 && rndNum <= 7)
            {
                BounceMissle bounceMissile = new BounceMissle(WorldPosition);

                Scene.drawablesToAdd.AddRange(new scenes.IDrawable[]
                {
                    bounceMissile,
                });

                Scene.updateablesToAdd.AddRange(new scenes.IUpdateable[]
                {
                    bounceMissile
                });
            }
        }

        //public override void OnCombatCollision(CombatArgs combatArgs)
        //{
        //    base.OnCombatCollision(combatArgs);
        //    enemySprite.SetAnimation("FLINCH_LEFT");
        //}
        
    }
}
