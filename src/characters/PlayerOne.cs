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

// TODO: Setze flinch boolean flag OnCombatCollision für TransitionOnFlinchAttack.

namespace EVCMonoGame.src.characters
{
    public class PlayerOne : Player
    {
        #region Fields

        public StateManagerSora stateManager;
        public bool isAttacking;
        public float runThreshold;

        public Keys[] keyboardControls;

        public Vector2 movementVector;
        public Vector2 previousMovementVector;
        

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
            sprite.spritesheet = PlayerSpriteSheets.RedGlow;
            // playerPortrait.LoadContent(content);
            stateManager = new StateManagerSora();
        }

        public override void Update(GameTime gameTime)
        {
            if (BlockInput)
                return;

            base.Update(gameTime);
            stateManager.Update(gameTime);

            if (InputManager.OnButtonPressed(Buttons.DPadLeft, PlayerIndex.One))
            {
                inventory.NavigateItems(gameTime, Inventory.Direction.LEFT);
            }
            else if (InputManager.OnButtonPressed(Buttons.DPadRight, PlayerIndex.One))
            {
                inventory.NavigateItems(gameTime, Inventory.Direction.RIGHT);
            }
            else if (InputManager.OnButtonPressed(Buttons.RightStick, PlayerIndex.One))
            {
                inventory.UseActiveUsableItem(gameTime);
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
    }
}
