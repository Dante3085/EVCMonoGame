using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using EVCMonoGame.src.scenes;

namespace EVCMonoGame.src.characters.enemies
{
    public class Hades : Enemy
    {

        public Hades(Vector2 position)
            : base
            (
                  name: "Hades",
                  maxHp: 2450,
                  currentHp: 2450,
                  maxMp: 100,
                  currentMp: 100,
                  strength: 15,
                  defense: 12,
                  intelligence: 9,
                  agility: 8,
                  movementSpeed: 6,
                  position: position,
                  exp: 3000
            )
        {
            sprite.LoadAnimationsFromFile("Content/rsrc/spritesheets/configFiles/hades.anm.txt");
            sprite.SetAnimation("RAGE_STRIKE_LEFT");

            collisionBox.Size = new Point(120, 120);
            collisionBoxOffset = new Vector2(50, 110);
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

        //public override void OnCombatCollision(CombatArgs combatArgs)
        //{
        //    base.OnCombatCollision(combatArgs);
        //    enemySprite.SetAnimation("FLINCH_LEFT");
        //}
    }
}
