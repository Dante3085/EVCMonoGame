using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using EVCMonoGame.src.scenes;
using EVCMonoGame.src.collision;

namespace EVCMonoGame.src
{
    public class AnimatedSprite : Updateable, scenes.IDrawable, GeometryCollidable
    {
        struct Animation
        {
            public Rectangle[] frames;
            public float frameDelay;

            public Animation(Rectangle[] frames, float frameDelay)
            {
                this.frames = frames;
                this.frameDelay = frameDelay;
            }
        }

        #region InternalVariables

        private String spritesheetName;
        private Texture2D spritesheet;
        private Dictionary<String, Animation> animations;
        private String currentAnimation;
        private int frameIndex;
        private float elapsedSeconds;

        private Vector2 position;
        private Vector2 previousPosition;
        private float scale;

        #endregion
        #region Properties

        public Rectangle Bounds
        {
            get
            {
                Rectangle bounds = new Rectangle();
                bounds.Location = position.ToPoint();
                bounds.Size = animations[currentAnimation].frames[frameIndex].Size;
                bounds.Width *= (int)scale;
                bounds.Height *= (int)scale;

                return bounds;
            }
        }

        public Vector2 Position
        {
            get { return position; }
            set 
            {
                previousPosition = position;
                position = value; 
            }
        }

        public Vector2 PreviousPosition
        {
            get { return previousPosition; }
        }

        #endregion

        public AnimatedSprite(String spritesheetName, Vector2 position, float scale = 1.0f)
        {
            this.spritesheetName = spritesheetName;
            animations = new Dictionary<string, Animation>();
            currentAnimation = "NONE";
            frameIndex = 0;
            elapsedSeconds = 0;

            this.position = position;
            previousPosition = position;
            this.scale = scale;
        }

        public override void Update(GameTime gameTime)
        {
            elapsedSeconds += (float)gameTime.ElapsedGameTime.TotalSeconds;

            Animation animation = animations[currentAnimation];
            if (elapsedSeconds >= animation.frameDelay)
            {
                elapsedSeconds = 0;
                frameIndex = (frameIndex + 1) == animation.frames.Length ? 0 : ++frameIndex;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spritesheet, position, animations[currentAnimation].frames[frameIndex], Color.White,
                0, Vector2.Zero, scale, SpriteEffects.None, 1);
        }

        public void LoadContent(ContentManager content)
        {
            spritesheet = content.Load<Texture2D>(spritesheetName);
        }

        public void AddAnimation(String name, Rectangle[] frames, float frameDelay)
        {
            animations[name] = new Animation(frames, frameDelay);
            currentAnimation = name;
        }

        // TODO: Problem bei 2. Überladung von AddAnimation(). Kann erst aufgerufen werden, nachdem LoadContent() aufgerufen wurde,
        // weil vorher die Spritesheet Texture noch nicht geladen wurde. Globaler ContentManager ?

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="frameSize"></param>
        /// <param name="firstFramePosition">x=column, y=row</param>
        /// <param name="numFrames"></param>
        /// <param name="frameDelay"></param>
        public void AddAnimation(String name, int frameWidth, int frameHeight, int xColumn, int yRow, int numFrames, float frameDelay)
        {
            Rectangle[] frames = new Rectangle[numFrames];

            int spritesheetWidht = spritesheet.Width;
            int frame = 0;

            // frame++ wird zu früh inkrementiert.
            while(frame < numFrames)
            {
                if (xColumn * frameWidth == spritesheetWidht)
                {
                    ++yRow;
                    xColumn = 0;
                }
                frames[frame++] = new Rectangle(xColumn++ * frameWidth, yRow * frameHeight, frameWidth, frameHeight);
            }

            AddAnimation(name, frames, frameDelay);
        }

        public void SetAnimation(String name)
        {
            // Do nothing if the given Animation is already set.
            if (currentAnimation == name)
            {
                return;
            }

            if (!animations.ContainsKey(name))
            {
                throw new ArgumentException("@SetAnimation(" + name + "): This AnimatedSprite does not know" +
                    " the given Animation.");
            }
            currentAnimation = name;
            elapsedSeconds = 0;
            frameIndex = 0;
        }
    }
}
