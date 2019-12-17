using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using EVCMonoGame.src.scenes;
using EVCMonoGame.src.statemachine.hades;
using EVCMonoGame.src.animation;

namespace EVCMonoGame.src.characters.enemies
{
    public class Hades : Enemy
    {

        public Hades(Vector2 position)
            : base
            (
                  name: "Hades",
                  maxHp: 400,//32000,
                  currentHp: 400,//32000,
                  maxMp: 100,
                  currentMp: 100,
                  strength: 20,
                  defense: 12,
                  intelligence: 9,
                  agility: 8,
                  movementSpeed: 6,
                  position: position,
                  exp: 10000
            )
        {
            sprite = new AnimatedSprite(position, 5.0f, true);
            sprite.LoadAnimationsFromFile("Content/rsrc/spritesheets/configFiles/hades.anm.txt");
            sprite.SetAnimation("RAGE_STRIKE_LEFT");

            collisionBox.Size = new Point(120, 120);
            collisionBoxOffset = new Vector2(50, 110);
            stateManager = new StateManagerHades(this);
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
