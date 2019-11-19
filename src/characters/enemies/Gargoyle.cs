using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace EVCMonoGame.src.characters.enemies
{
    public class Gargoyle : Enemy
    {
        public Gargoyle(Vector2 position)
            : base
            (
                  name: "Gargoyle",
                  maxHp: 500,
                  currentHp: 500,
                  maxMp: 0,
                  currentMp: 0,
                  strength: 5,
                  defense: 4,
                  intelligence: 1,
                  agility: 3,
                  movementSpeed: 5,
                  position: position,
                  exp: 8
            )
        {
            sprite.LoadAnimationsFromFile("Content/rsrc/spritesheets/configFiles/gargoyle.anm.txt");
            sprite.SetAnimation("FLINCH_RIGHT");

            collisionBoxOffset = new Vector2(100, 100);
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
