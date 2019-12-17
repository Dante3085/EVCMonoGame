using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using C3.MonoGame;
using EVCMonoGame.src.scenes;
using EVCMonoGame.src.statemachine.gargoyle;
using EVCMonoGame.src.animation;
using EVCMonoGame.src.states;
using EVCMonoGame.src.Items;
using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.Media;

namespace EVCMonoGame.src.characters.enemies
{
    public class Gargoyle : Enemy
    {
        public SoundEffect scream;
        public Gargoyle(Vector2 position, GameplayState.Lane spawn = GameplayState.Lane.LaneBoth)
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
                  movementSpeed: 6,
                  position: position,
                  exp: 8,
				  spawn: spawn
			)
        {
            sprite = new AnimatedSprite(position, 5.0f, true);
            sprite.LoadAnimationsFromFile("Content/rsrc/spritesheets/configFiles/gargoyle.anm.txt");
            sprite.SetAnimation("BATTLE_CRY_LEFT");
            this.attackRange = 450;
            collisionBoxOffset = new Vector2(100, 100);
            stateManager = new StateManagerGargoyle(this);
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
            scream = AssetManager.GetSoundEffect(ESoundEffect.GARGOYLE);
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
				coin.gold = random.Next(5, 15);

				Vector2 rndExpBottlePosition = WorldPosition + new Vector2(random.Next(-250, 0), random.Next(-250, 0));
				InstantConsumable expBottle = new InstantConsumable(rndExpBottlePosition,
																		"Content/rsrc/spritesheets/configFiles/exp.anm.txt",
																		"EXP",
																		GameplayState.Lane.LaneBoth,
																		0.5f);
				expBottle.exp = random.Next(15, 25);

				Vector2 rndHealthorbPosition = WorldPosition + new Vector2(random.Next(-250, 250), random.Next(-250, 250));
				InstantConsumable healthorb = new InstantConsumable(rndHealthorbPosition, "Content/rsrc/spritesheets/configFiles/healthorb.anm.txt",
																	"IDLE", GameplayState.Lane.LaneBoth, 3);
				healthorb.heal = random.Next(20, 25);

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
			}
		}

		//public override void OnCombatCollision(CombatArgs combatArgs)
		//{
		//    base.OnCombatCollision(combatArgs);
		//    enemySprite.SetAnimation("FLINCH_LEFT");
		//}
	}
}
