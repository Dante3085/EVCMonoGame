using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using EVCMonoGame.src.gui;
using EVCMonoGame.src.input;
using EVCMonoGame.src.scenes;
using EVCMonoGame.src.collision;
using EVCMonoGame.src.animation;
using EVCMonoGame.src.utility;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.states;
using EVCMonoGame.src.statemachine.sora;
using EVCMonoGame.src.projectiles;
using EVCMonoGame.src.Items;

// TODO: Setze flinch boolean flag OnCombatCollision für TransitionOnFlinchAttack.

namespace EVCMonoGame.src.characters
{

    public class PlayerOne : Player
    {
        #region Fields

        // public StateManagerSora stateManager;
        public bool isAttacking;
        public float runThreshold;

        public Keys[] keyboardControls;

        public Vector2 movementVector;
        public Vector2 previousMovementVector;

        private bool controllingWeaponInventory = false;

        public AuraWeapon weapon = new AuraWeapon(Vector2.Zero, "rsrc/spritesheets/singleImages/pumpkinBottle",
                                       "Content/rsrc/spritesheets/configFiles/coin.anm.txt", "COIN", EAura.NORMAL);

        #endregion
        #region Properties

        /// <summary>
        /// ONLY FOR DEBUGGING PURPOSES. REMOVE LATER.
        /// </summary>
        public bool DoesUpdateMovement
        {
            get; set;
        }

        #endregion

        public PlayerOne(Vector2 position, Keys[] controls)
             : base
            (
                  name: "Sora",
                  maxHp: 800,
                  currentHp: 800,
                  maxMp: 30,
                  currentMp: 30,
                  strength: 5,
                  defense: 3,
                  intelligence: 5,
                  agility: 4,
                  movementSpeed: 7,
                  position: position,
				  playerIndex: PlayerIndex.One,
				  lane: GameplayState.Lane.LaneOne
            )
        {

			PlayerInventory = new Inventory(this);

			isAttacking = false;
            runThreshold = 0.65f;

            movementSpeed = 7.5f;

            CollisionBox = new Rectangle(position.ToPoint(), new Point(140, 230));

            sprite.LoadAnimationsFromFile("Content/rsrc/spritesheets/configFiles/sora.anm.txt");
            sprite.SetAnimation("RUN_RIGHT");
            playerOrientation = Orientation.RIGHT;

            // Der Parameter controls ist nicht final. Nur, um mehrere Player Instanzen anders steuern zu können.
            if (controls.Length != 4)
            {
                throw new ArgumentException("Nur 4 Bewegungstasten");
            }
            this.keyboardControls = controls;

            movementVector = Vector2.Zero;
            previousMovementVector = movementVector;
            DoesUpdateMovement = true;
            flinching = false;

            inventory.AddWeapon(weapon);
            CollisionManager.RemoveCollidable(weapon, CollisionManager.itemCollisionChannel);

            expBar.Level = 14;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            
            // playerPortrait.Draw(gameTime, spriteBatch);
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            MagicMissile.content = content;
            PlayerSpriteSheets.Load(content);
            //sprite.spritesheet = PlayerSpriteSheets.RedGlow;
            // playerPortrait.LoadContent(content);
            stateManager = new StateManagerSora();
        }

        public override void Update(GameTime gameTime)
        {
            if (BlockInput)
                return;

			stateManager.Update(gameTime);

			base.Update(gameTime);

			if (InputManager.OnKeyPressed(Keys.E))
				Console.WriteLine("Location: " + WorldPosition);

            if (InputManager.OnButtonPressed(Buttons.DPadUp, PlayerIndex.One) ||
                InputManager.OnButtonPressed(Buttons.DPadDown, PlayerIndex.One))
            {
                controllingWeaponInventory = !controllingWeaponInventory;
            }

            if (controllingWeaponInventory)
            {
                if (InputManager.OnButtonPressed(Buttons.DPadLeft, PlayerIndex.One))
                {
                    inventory.NavigateWeapons(gameTime, Inventory.Direction.LEFT);
                    inventory.ActivateSpecialAttack(gameTime, inventory.weapons[inventory.GetNextWeaponPos()]);
                }
                else if (InputManager.OnButtonPressed(Buttons.DPadRight, PlayerIndex.One))
                {
                    inventory.NavigateWeapons(gameTime, Inventory.Direction.RIGHT);
                    inventory.ActivateSpecialAttack(gameTime, inventory.weapons[inventory.GetPrevWeaponPos()]);
                }
            }
            else
            {
                if (InputManager.OnButtonPressed(Buttons.DPadRight, PlayerIndex.One))
                {
                    inventory.NavigateItems(gameTime, Inventory.Direction.RIGHT);
                }

                else if (InputManager.OnButtonPressed(Buttons.DPadLeft, PlayerIndex.One))
                {
                    inventory.NavigateItems(gameTime, Inventory.Direction.LEFT);
                }

                else if (InputManager.OnButtonPressed(Buttons.RightStick, PlayerIndex.One))
                {
                    inventory.UseActiveUsableItem(gameTime);
                }
            }
        }

        public void SetGlow(EAura aura){
            
            switch(aura){
                case EAura.WHITE:
                        sprite.spritesheet = PlayerSpriteSheets.WhiteGlow;
                    break;
                case EAura.GREEN:
                        sprite.spritesheet = PlayerSpriteSheets.GreenGlow;
                        break;
                case EAura.RED:
                    sprite.spritesheet = PlayerSpriteSheets.RedGlow;
                    break;
                case EAura.BLUE:
                    sprite.spritesheet = PlayerSpriteSheets.BlueGlow;
                    break;
                case EAura.YELLOW:
                    sprite.spritesheet = PlayerSpriteSheets.YellowGlow;
                    break;
                case EAura.GOD:
                    sprite.spritesheet = PlayerSpriteSheets.GodModeGlow;
                    break;
                case EAura.NORMAL:
                    sprite.spritesheet = PlayerSpriteSheets.NoGlow;
                    break;
            }
            
        }

        public /* override */ void OnCombatCollision(CombatArgs combatArgs)
        {
            //sprite.Position += combatArgs.knockBack;

            //switch (playerOrientation)
            //{
            //    case Orientation.DOWN: sprite.SetAnimation("FLINCH_LEFT"); break;
            //    case Orientation.DOWN_LEFT: sprite.SetAnimation("FLINCH_LEFT"); break;
            //    case Orientation.LEFT: sprite.SetAnimation("FLINCH_LEFT"); break;
            //    case Orientation.UP_LEFT: sprite.SetAnimation("FLINCH_LEFT"); break;
            //    case Orientation.UP: sprite.SetAnimation("FLINCH_LEFT"); break;

            //    case Orientation.UP_RIGHT: sprite.SetAnimation("FLINCH_RIGHT"); break;
            //    case Orientation.RIGHT: sprite.SetAnimation("FLINCH_RIGHT"); break;
            //    case Orientation.DOWN_RIGHT: sprite.SetAnimation("FLINCH_RIGHT"); break;
            //}
            //flinching = true;
        }

        public override void CheckLevelUp()
        {
            if (expBar.LevelUp)
            {
                switch(expBar.Level)
                {
                    case 15:
                        AuraWeapon redAura = new AuraWeapon(Vector2.Zero, "rsrc/spritesheets/singleImages/cBottle",
                            "Content/rsrc/spritesheets/configFiles/coin.anm.txt", "COIN", EAura.RED,
                            strength: 30);
                        CollisionManager.RemoveCollidable(redAura, CollisionManager.itemCollisionChannel);
                        inventory.AddWeapon(redAura);

                        redAura.LoadContent(GameplayState.globalContentManager);
                        break;

                    case 30:
                        AuraWeapon greenAura = new AuraWeapon(Vector2.Zero, "rsrc/spritesheets/singleImages/gBottle",
                            "Content/rsrc/spritesheets/configFiles/coin.anm.txt", "COIN", EAura.GREEN,
                            knockbackMultiplier: 2);
                        CollisionManager.RemoveCollidable(greenAura, CollisionManager.itemCollisionChannel);
                        inventory.AddWeapon(greenAura);

                        greenAura.LoadContent(GameplayState.globalContentManager);
                        break;

                    case 45:
                        AuraWeapon blueAura = new AuraWeapon(Vector2.Zero, "rsrc/spritesheets/singleImages/exBottle",
                            "Content/rsrc/spritesheets/configFiles/coin.anm.txt", "COIN", EAura.BLUE,
                            speed: 10);
                        CollisionManager.RemoveCollidable(blueAura, CollisionManager.itemCollisionChannel);
                        inventory.AddWeapon(blueAura);

                        blueAura.LoadContent(GameplayState.globalContentManager);
                        break;

                    case 60:
                        AuraWeapon yellowAura = new AuraWeapon(Vector2.Zero, "rsrc/spritesheets/singleImages/lightBluePotion",
                            "Content/rsrc/spritesheets/configFiles/coin.anm.txt", "COIN", EAura.YELLOW,
                            strength: 25, knockbackMultiplier: 2);
                        CollisionManager.RemoveCollidable(yellowAura, CollisionManager.itemCollisionChannel);
                        inventory.AddWeapon(yellowAura);

                        yellowAura.LoadContent(GameplayState.globalContentManager);
                        break;

                    case 75:
                        AuraWeapon whiteAura = new AuraWeapon(Vector2.Zero, "rsrc/spritesheets/singleImages/greenGel",
                            "Content/rsrc/spritesheets/configFiles/coin.anm.txt", "COIN", EAura.WHITE,
                            speed: 10, strength: 35);
                        CollisionManager.RemoveCollidable(whiteAura, CollisionManager.itemCollisionChannel);
                        inventory.AddWeapon(whiteAura);

                        whiteAura.LoadContent(GameplayState.globalContentManager);
                        break;

                    case 90:
                        AuraWeapon godAura = new AuraWeapon(Vector2.Zero, "rsrc/spritesheets/singleImages/bluePotion",
                                       "Content/rsrc/spritesheets/configFiles/coin.anm.txt", "COIN", EAura.GOD,
                                       strength: 100, defense:100, speed: 15, knockbackMultiplier: 5);


                        CollisionManager.RemoveCollidable(godAura, CollisionManager.itemCollisionChannel);

                        inventory.AddWeapon(godAura);


                        godAura.LoadContent(GameplayState.globalContentManager);
                        break;
                }
            }
        }
    }
}
