﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using EVCMonoGame.src.collision;
using EVCMonoGame.src.scenes;
using EVCMonoGame.src.statemachine.defender;

namespace EVCMonoGame.src.characters.enemies
{
    public class Defender : Enemy
    {
        public Defender(Vector2 position)
            : base
            (
                  name: "Defender",
                  maxHp: 1000,
                  currentHp: 1000,
                  maxMp: 0,
                  currentMp: 0,
                  strength: 8,
                  defense: 15,
                  intelligence: 0,
                  agility: 3,
                  movementSpeed: 3,
                  position: position,
                  exp: 150
            )
        {
            sprite.LoadAnimationsFromFile("Content/rsrc/spritesheets/configFiles/defender.anm.txt");
            sprite.SetAnimation("RUN_RIGHT");

            collisionBoxOffset = new Vector2(100, 100);
            stateManager = new StateManagerDefender(this);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (sprite.AnimationFinished)
            {
                sprite.SetAnimation("IDLE_RIGHT");
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
        }

        public override void OnCombatCollision(CombatArgs combatArgs)
        {
            base.OnCombatCollision(combatArgs);

            if (combatArgs.victim == this && combatArgs.causesFlinch)
            {
                sprite.SetAnimation("FLINCH_RIGHT");
            }
        }
    }
}
