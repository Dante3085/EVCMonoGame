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
using EVCMonoGame.src.projectiles;
using EVCMonoGame.src.statemachine.riku;

namespace EVCMonoGame.src.characters
{
    public class PlayerTwo : Player
    {
        #region Fields

        public StateManagerRiku stateManager;

        public float runThreshold;

        public Keys[] keyboardControls;

        public Vector2 movementVector;
        public Vector2 previousMovementVector;

        public List<MagicMissile> missiles = new List<MagicMissile>();
        public List<MagicMissile> missilesToBeAdded = new List<MagicMissile>();

        #endregion
        #region Properties

        /// <summary>
        /// ONLY FOR DEBUGGING PURPOSES. REMOVE LATER.
        /// </summary>
        public bool DoesUpdateMovement
        {
            get; set;
        }

        public bool BlockInput
        {
            get; set;
        } = false;

        #endregion

        #region Constructors
        public PlayerTwo(Vector2 position, Keys[] controls)
             : base
            (
                  name: "Riku",
                  maxHp: 900,
                  currentHp: 800,
                  maxMp: 30,
                  currentMp: 30,
                  strength: 3,
                  defense: 3,
                  intelligence: 5,
                  agility: 4,
                  movementSpeed: 7,
                  position: position,
				  playerIndex: PlayerIndex.Two
			)
        {
            runThreshold = 0.65f;

            movementSpeed = 7.5f;

            CollisionBox = new Rectangle(position.ToPoint(), new Point(140, 230));

            sprite.LoadAnimationsFromFile("Content/rsrc/spritesheets/configFiles/riku.anm.txt");
            sprite.SetAnimation("ROUND_SWING_LEFT");
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
        #endregion
        #region IDrawable
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            foreach (MagicMissile m in missiles)
            {
                m.Draw(gameTime, spriteBatch);
            }
            // playerPortrait.Draw(gameTime, spriteBatch);
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            MagicMissile.content = content;
            stateManager = new StateManagerRiku();
            // playerPortrait.LoadContent(content);
        }

        #endregion
        #region Updateable
        public override void Update(GameTime gameTime)
        {
            if (BlockInput)
                return;

            base.Update(gameTime);
            foreach (MagicMissile m in missiles)
            {
                m.Update(gameTime);
            }
            missiles.AddRange(missilesToBeAdded);
            missilesToBeAdded.Clear();
            missiles.RemoveAll((a) => { return a.FlaggedForRemove; });
            stateManager.Update(gameTime);

            // flinch = zurückweichen
            
            // playerPortrait.Update(gameTime);
        }

        

        #endregion
        #region CombatCollidable


        public /* override */ void OnCombatCollision(CombatArgs combatArgs)
        {
            sprite.Position += combatArgs.knockBack;

            
            flinching = true;
        }
        #endregion
    }
}
