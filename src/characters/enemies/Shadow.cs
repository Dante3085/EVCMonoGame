﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using EVCMonoGame.src.collision;

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
