using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

using EVCMonoGame.src.scenes;
using EVCMonoGame.src.Items;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using EVCMonoGame.src.input;
using Microsoft.Xna.Framework.Input;

namespace EVCMonoGame.src.characters
{
    public abstract class Player : Character, scenes.IDrawable
    {
        private ItemFinder itemFinder;
        private Inventory inventory;

		public int gold;
		public int exp;


		public Orientation playerOrientation = Orientation.RIGHT;
		private PlayerIndex playerIndex;

		public Inventory PlayerInventory
        {
            get { return inventory; }
            set { }
        }
		public PlayerIndex PlayerIndex
		{
			get { return playerIndex; }
			set { }
		}

		public bool BlockInput
        {
            get; set;
        } = false;

		public Player
			(
				String name,
				int maxHp,
				int currentHp,
				int maxMp,
				int currentMp,
				int strength,
				int defense,
				int intelligence,
				int agility,
				int movementSpeed,
				Vector2 position,
				PlayerIndex playerIndex
            )
            : base
            (
                  name: name,
                  maxHp: maxHp,
                  currentHp: currentHp,
                  maxMp: maxMp,
                  currentMp: currentMp,
                  strength: strength,
                  defense: defense,
                  intelligence: intelligence,
                  agility: agility,
                  movementSpeed: movementSpeed,
                  position: position,
                  characterType: CombatantType.PLAYER
            )
		{
			this.playerIndex = playerIndex;
            this.combatant = CombatantType.PLAYER;
            this.combatArgs.targetType = CombatantType.ENEMY;
			inventory = new Inventory(this);
			itemFinder = new ItemFinder(this);
		}

		public override void LoadContent(ContentManager content)
		{
			base.LoadContent(content);
			inventory.LoadContent(content);
		}

		/*
				public override void LoadContent(ContentManager content)
				{
					base.LoadContent(content);
					PlayerSpriteSheets.Load(content);
					playerPortrait.LoadContent(content);
					sprite.spritesheet = PlayerSpriteSheets.RedGlow;
				}
		*/
		public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            itemFinder.Update(gameTime);

			if (InputManager.IsKeyPressed(Keys.Q))
				inventory.NavigateItems(gameTime, Inventory.Direction.LEFT);
			if (InputManager.IsKeyPressed(Keys.E))
				inventory.NavigateItems(gameTime, Inventory.Direction.RIGHT);

		}

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            itemFinder.Draw(gameTime, spriteBatch);
        }
    }
}
