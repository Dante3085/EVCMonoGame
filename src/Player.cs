using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using EVCMonoGame.src.gui;
using EVCMonoGame.src.input;
using EVCMonoGame.src.screens;

namespace EVCMonoGame.src
{
    public class Player : Updateable, IDrawable
    {
        private AnimatedSprite playerSprite;
        private Healthbar playerHealthbar;

        public Player()
        {
            playerSprite = new AnimatedSprite("rsrc/spritesheets/CronoTransparentBackground",
                new Vector2(100, 100), 6.0f);

            // Frames sind leicht falsch(Abgeschnittene Ecken).
            playerSprite.AddAnimation("IDLE", new Rectangle[]
            {
                new Rectangle(59, 14, 15, 34), new Rectangle(79, 14, 15, 34), new Rectangle(99, 14, 15, 34)
            }, 0.8f);
            playerSprite.AddAnimation("WALK_UP", new Rectangle[]
            {
                new Rectangle(130, 59, 17, 32), new Rectangle(152, 60, 17, 31), new Rectangle(174, 57, 15, 34),
                new Rectangle(193, 57, 15, 34), new Rectangle(213, 60, 17, 31), new Rectangle(235, 59, 17, 32),
            }, 0.15f);
            playerSprite.AddAnimation("WALK_LEFT", new Rectangle[]
            {
                new Rectangle(34, 683, 14, 33), new Rectangle(56, 684, 13, 32), new Rectangle(75, 685, 21, 31),
                new Rectangle(103, 683, 13, 33), new Rectangle(125, 684, 14, 32), new Rectangle(145, 685, 20, 32)
            }, 0.15f);
            playerSprite.AddAnimation("WALK_DOWN", new Rectangle[]
            {
                new Rectangle(130, 15, 15, 33), new Rectangle(150, 17, 16, 31), new Rectangle(171, 14, 17, 34),
                new Rectangle(193, 15, 15, 33), new Rectangle(213, 17, 16, 31),
            }, 0.15f);
            playerSprite.AddAnimation("WALK_RIGHT", new Rectangle[]
            {
                new Rectangle(126, 100, 19, 31), new Rectangle(151, 99, 14, 32), new Rectangle(174, 98, 13, 33),
                new Rectangle(194, 100, 21, 31), new Rectangle(221, 99, 13, 32), new Rectangle(242, 98, 14, 33),
            }, 0.15f);
            playerSprite.SetAnimation("WALK_RIGHT");

            playerHealthbar = new Healthbar(2345, 1234, new Vector2(300, 100), new Vector2(200, 30));
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            playerHealthbar.Draw(gameTime, spriteBatch);
            playerSprite.Draw(gameTime, spriteBatch);
        }

        public void LoadContent(ContentManager content)
        {
            playerHealthbar.LoadContent(content);
        }

        public void UnloadContent()
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            // TODO: playerSprite steuern(Animationen ändern und bewegen)

            playerSprite.Update(gameTime);
        }
    }
}
