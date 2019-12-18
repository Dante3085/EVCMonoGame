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
using EVCMonoGame.src.states;
using EVCMonoGame.src.animation;
using EVCMonoGame.src.projectiles;
using C3.MonoGame;

namespace EVCMonoGame.src.characters.enemies
{
    public class Hades : Enemy
    {
        public float attackRangeMeteor;
        public float outerAttackRangeFireBlast;
        public float innerAttackRangeFireBlast;
        public List<HadesMissile> missiles = new List<HadesMissile>();
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
                  exp: 10000,
				  spawn: GameplayState.Lane.LaneBoth
			)
        {
            attackRangeMeteor = sightRange * 2;
            outerAttackRangeFireBlast = 550;
            innerAttackRangeFireBlast = 400;
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
            foreach (HadesMissile m in missiles)
            {
                m.Update(gameTime);
            }
            
            missiles.RemoveAll((a) => { return a.FlaggedForRemove; });

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            foreach (MagicMissile m in missiles)
            {
                m.Draw(gameTime, spriteBatch);
            }
            if (false)
            {
                Primitives2D.DrawCircle(spriteBatch, WorldPosition, attackRange, 16, Color.Red, 3);
                Primitives2D.DrawCircle(spriteBatch, WorldPosition, attackRangeMeteor, 16, Color.Orange, 3);
                Primitives2D.DrawCircle(spriteBatch, WorldPosition, innerAttackRangeFireBlast, 16, Color.OrangeRed, 3);
                Primitives2D.DrawCircle(spriteBatch, WorldPosition, outerAttackRangeFireBlast, 16, Color.OrangeRed, 3);
            }
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
