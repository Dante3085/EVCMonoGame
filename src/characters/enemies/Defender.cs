using System;
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
using EVCMonoGame.src.states;
using EVCMonoGame.src.Items;

namespace EVCMonoGame.src.characters.enemies
{
    public class Defender : Enemy
    {
		public Defender(Vector2 position, GameplayState.Lane spawn = GameplayState.Lane.LaneBoth,
			            int hpBonus = 0)
			: base
			(
				  name: "Defender",
				  maxHp: 1000 + hpBonus,
				  currentHp: 1000 + hpBonus,
				  maxMp: 0,
				  currentMp: 0,
				  strength: 8,
				  defense: 15,
				  intelligence: 0,
				  agility: 3,
				  movementSpeed: 3,
				  position: position,
				  exp: 150,
				  spawn: spawn
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

		public override void DropLoot()
		{
			Random random = new Random();

			for (int i = 0; i < 10; i++)
			{

				Vector2 rndCoinPosition = WorldPosition + new Vector2(random.Next(250), random.Next(250));
				InstantConsumable coin = new InstantConsumable(rndCoinPosition,
																"Content/rsrc/spritesheets/configFiles/coin.anm.txt",
																"COIN",
																GameplayState.Lane.LaneBoth);
				coin.gold = random.Next(10, 23);

				Vector2 rndExpBottlePosition = WorldPosition + new Vector2(random.Next(-250, 0), random.Next(-250, 0));
				InstantConsumable expBottle = new InstantConsumable(rndExpBottlePosition,
																		"Content/rsrc/spritesheets/configFiles/exp.anm.txt",
																		"EXP",
																		GameplayState.Lane.LaneBoth,
																		0.5f);
				expBottle.exp = random.Next(25, 35);

				Vector2 rndHealthorbPosition = WorldPosition + new Vector2(random.Next(-250, 250), random.Next(-250, 250));
				InstantConsumable healthorb = new InstantConsumable(rndHealthorbPosition, "Content/rsrc/spritesheets/configFiles/healthorb.anm.txt",
																	"IDLE", GameplayState.Lane.LaneBoth, 3);
				healthorb.heal = random.Next(30, 45);

				if (random.Next(1, 100) < 30)
				{
					Vector2 rndHealthpotionPosition = WorldPosition + new Vector2(random.Next(-100, 100), random.Next(-100, 100));
					Healthpotion healthpotion = new Healthpotion(rndHealthpotionPosition);
					Scene.drawablesToAdd.Add(healthpotion);
					Scene.updateablesToAdd.Add(healthpotion);
				}

				Scene.drawablesToAdd.AddRange(new scenes.IDrawable[]
				{
					coin,
					expBottle,
					healthorb,
				});

				Scene.updateablesToAdd.AddRange(new scenes.IUpdateable[]
				{
					coin,
					expBottle,
					healthorb,
				});
			}

			if (spawn != GameplayState.Lane.LaneOne)
			{
				DropMissiles();
				DropMissiles();
				DropMissiles();
			}
		}
		

	}
}
