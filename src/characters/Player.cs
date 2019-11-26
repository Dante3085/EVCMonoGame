using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

using EVCMonoGame.src.scenes;
using EVCMonoGame.src.Items;
using Microsoft.Xna.Framework.Graphics;

namespace EVCMonoGame.src.characters
{
    public abstract class Player : Character, scenes.IDrawable
    {
        private ItemFinder itemFinder;
        private Inventory inventory;
        

        public Orientation playerOrientation = Orientation.RIGHT;

        public Inventory PlayerInventory
        {
            get { return inventory; }
            set { inventory = value; }
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
                Vector2 position
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
                  position: position
            )
        {
            itemFinder = new ItemFinder(this);
            inventory = new Inventory(this);
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
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            inventory.Draw(gameTime, spriteBatch);
            itemFinder.Draw(gameTime, spriteBatch);
        }
    }
}
