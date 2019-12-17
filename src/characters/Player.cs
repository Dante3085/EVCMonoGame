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
using EVCMonoGame.src.collision;
using EVCMonoGame.src.states;
using EVCMonoGame.src.gui;
using EVCMonoGame.src.animation;

namespace EVCMonoGame.src.characters
{
    public abstract class Player : Character
    {
		// Inventorymanagement
        private ItemFinder itemFinder;
        protected Inventory inventory;

		public int exp;
		public bool isPhaseMode = false; // Erlaubt das durchlaufen von Enemys und andere Spieler

        public ExperienceBar expBar;

		// Font
		protected SpriteFont font;

		// Gold
		private bool drawGold;
        private double showGoldTime;
        public AnimatedSprite goldSprite;
		private int fakeGoldOffset;

		public bool hidePlayer = false;

		public Orientation playerOrientation = Orientation.RIGHT;
		private PlayerIndex playerIndex;
		public GameplayState.Lane lane;

		public Inventory PlayerInventory
        {
            get { return inventory; }
            set { inventory = value; }
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
				PlayerIndex playerIndex,
				GameplayState.Lane lane
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
			this.lane = lane;
			itemFinder = new ItemFinder(this);

            expBar = new ExperienceBar(100, 20, Vector2.Zero, new Vector2(150, 10));

            // Sprite
            goldSprite = new AnimatedSprite(WorldPosition);
            goldSprite.LoadAnimationsFromFile("Content/rsrc/spritesheets/configFiles/coin.anm.txt");
            goldSprite.SetAnimation("COIN");
        }

		public override void LoadContent(ContentManager content)
		{
			base.LoadContent(content);
			inventory.LoadContent(content);

            expBar.LoadContent(content);
            goldSprite.LoadContent(content);

			font = content.Load<SpriteFont>("rsrc/fonts/DefaultFont");
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
            if (!IsAlive)
            {
                sprite.overlayColorOverTime(Color.DarkGray, new TimeSpan(0, 0, 0, 0, 10));
            }
            itemFinder.Update(gameTime);

			//Navigate UsableItems
			if (InputManager.OnKeyPressed(Keys.Q))
				inventory.NavigateItems(gameTime, Inventory.Direction.LEFT);
			if (InputManager.OnKeyPressed(Keys.E))
				inventory.NavigateItems(gameTime, Inventory.Direction.RIGHT);

			//Navigate Weapons
			if (InputManager.OnKeyPressed(Keys.Y))
				inventory.NavigateWeapons(gameTime, Inventory.Direction.LEFT);
			if (InputManager.OnKeyPressed(Keys.X))
				inventory.NavigateWeapons(gameTime, Inventory.Direction.RIGHT);

			// Use Special Attack
			if (InputManager.OnKeyPressed(Keys.B) && playerIndex == PlayerIndex.One)
				inventory.ActivateSpecialAttack(gameTime);

			// Use Item or Interact
			if (InputManager.OnKeyPressed(Keys.F) || InputManager.OnButtonPressed(Buttons.A, playerIndex))
			{
				if (CollisionManager.IsInteractableCollision(this))
					CollisionManager.GetNearestInteractable(this).Interact(this);
				else
					inventory.UseActiveUsableItem(gameTime);
			}

            expBar.Position = Healthbar.Position - new Vector2(0, expBar.Size.Y);

            if (drawGold)
            {
                goldSprite.Scale = 1f;
                goldSprite.WorldPosition = WorldPosition + new Vector2(10, -125);
                goldSprite.Update(gameTime);

                showGoldTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
                if (showGoldTime < 0)
                {
                    ShowGold(false);
					fakeGoldOffset = 0;
                }
            }
            else
                goldSprite.Scale = 0f;

		
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
			if (!hidePlayer)
			{
				base.Draw(gameTime, spriteBatch);

				itemFinder.Draw(gameTime, spriteBatch);

				expBar.Draw(gameTime, spriteBatch);

				goldSprite.Draw(gameTime, spriteBatch);

				if (drawGold)
					spriteBatch.DrawString(font, "x" + (PlayerInventory.Gold + fakeGoldOffset).ToString(), WorldPosition + new Vector2(70, -110), Color.White);
			}
           
		}

		public void ShowGold(bool showGold, double time = 0)
        {
			if (fakeGoldOffset == 0)
			{
				drawGold = showGold;
				showGoldTime = time;
			}
        }

		public void ShowFakeGold(bool showGold, int fakeGoldOffset, double time = 0)
		{
			this.fakeGoldOffset = fakeGoldOffset;
			drawGold = showGold;
			showGoldTime = time;
		}

		public abstract void CheckLevelUp();
    }
}
