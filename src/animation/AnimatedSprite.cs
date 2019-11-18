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
using EVCMonoGame.src.utility;

namespace EVCMonoGame.src.animation
{
    // TODO: Optimize LoadFromFile() and HelperMethods. ~20ms is a bit long
    // TODO: Implement frameOffsets.

    public class AnimatedSprite : scenes.IUpdateable, scenes.IDrawable, Collidable, ITranslatable
    {
        class  Animation
        {
            public Rectangle[] Frames
            {
                get; set;
            }

            public Dictionary<int, Rectangle> HurtBounds
            {
                get; set;
            }

            public Dictionary<int, Rectangle> AttackBounds
            {
                get; set;
            }

            public Dictionary<int, int> FrameDelays
            {
                get; set;
            }

            public Dictionary<int, Vector2> FrameOffsets
            {
                get; set;
            }

            public bool IsMirrored
            {
                get; set;
            }

            public bool IsLooped
            {
                get; set;
            }

            private bool isFinished;
            public bool IsFinished
            {
                get
                {
                    return IsLooped ? false : isFinished;
                }
                set
                {
                    isFinished = value;
                }
            }

            public Animation
            (
                Rectangle[] frames, 
                Dictionary<int, Rectangle> hurtBounds, 
                Dictionary<int, Rectangle> attackBounds,
                Dictionary<int, int> frameDelays, 
                Dictionary<int, Vector2> frameOffsets, 
                bool isMirrored, 
                bool isLooped)
            {
                Frames = frames;
                HurtBounds = hurtBounds;
                AttackBounds = attackBounds;
                FrameDelays = frameDelays;
                FrameOffsets = frameOffsets;
                IsMirrored = isMirrored;
                IsLooped = isLooped;

                isFinished = false;
            }
        }
        #region Fields

        private String spritesheetName;
        private Texture2D spritesheet;
        private Dictionary<String, Animation> animations;
        private String currentAnimation;
        private String previousAnimation;
        private int frameIndex;
        private int elapsedMillis;

        private Vector2 position;
        private Vector2 previousPosition;
        private float scale;

        private Rectangle bounds;

        #endregion
        #region Properties

        public bool DoUpdate
        {
            get; set;
        } = true;

        public Rectangle CurrentHurtBounds
        {
            get
            {
                Rectangle currentHurtBounds = animations[currentAnimation].HurtBounds[frameIndex];
                currentHurtBounds.Location += position.ToPoint();
                currentHurtBounds.Size *= new Point((int)scale, (int)scale);

                return currentHurtBounds;
            }
        }

        public Rectangle CurrentAttackBounds
        {
            // TODO: Define AttackBox for each frame of Attack Animation
            get
            {
                Rectangle currentAttackBounds = animations[currentAnimation].AttackBounds[frameIndex];
                currentAttackBounds.Location += position.ToPoint();
                currentAttackBounds.Size *= new Point((int)scale, (int)scale);

                return currentAttackBounds;
            }
        }

        public Rectangle Bounds
        {
            get
            {
                // Rectangle bounds = new Rectangle();
                bounds.Location = position.ToPoint();

                // TODO: Nicht einfach Animationsnamen eintragen. Irgendwie generisch berechnen.
                bounds.Size = animations["IDLE_LEFT"].Frames[0].Size;
                bounds.Width *= (int)(scale);
                bounds.Height *= (int)(scale);

                bounds.Inflate(-20, -100);

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

        /// <summary>
        /// Returns true if the current non-looping Animation has finished(Is at it's last frame), otherwise false.
        /// </summary>
        public bool AnimationFinished
        {
            get
            {
                return animations[currentAnimation].IsFinished;
            }
        }

        public Vector2 PreviousPosition
        {
            get { return previousPosition; }
        }

        public String CurrentAnimation
        {
            get { return currentAnimation; }
        }

        public String PreviousAnimation
        {
            get { return previousAnimation; }
        }

        public int ElapsedMillis
        {
            get { return elapsedMillis; }
        }

        public int FrameIndex
        {
            get { return frameIndex; }
        }

        public Vector2 WorldPosition
        {
            set
            {
                PreviousWorldPosition = position;

                position.X = (int)value.X;
                position.Y = (int)value.Y;
                bounds.X = (int)value.X;
                bounds.Y = (int)value.Y;
            }

            get
            {
                return position;
            }
        }

        public Vector2 PreviousWorldPosition { get; set; }

        public Rectangle CollisionBox
        {
            set
            {
                bounds = value;
                position = value.Location.ToVector2();
            }
            get
            {
                Rectangle bounds = new Rectangle();
                bounds.Location = position.ToPoint();

                // TODO: Nicht einfach Animationsnamen eintragen. Irgendwie generisch berechnen.
                bounds.Size = animations["IDLE_LEFT"].Frames[0].Size;
                bounds.Width *= (int)(scale);
                bounds.Height *= (int)(scale);

                bounds.Inflate(-20, -100);

                return bounds;
            }
        }

        #endregion

        #region Constructors
        public AnimatedSprite(String spritesheetName, Vector2 position, float scale = 1.0f)
        {
            this.spritesheetName = spritesheetName;
            animations = new Dictionary<string, Animation>();
            currentAnimation = "NONE";
            frameIndex = 0;
            elapsedMillis = 0;

            this.position = position;
            previousPosition = position;
            this.scale = scale;
        }

        public AnimatedSprite(Vector2 position, float scale = 1.0f)
        {
            animations = new Dictionary<string, Animation>();
            currentAnimation = "NONE";
            frameIndex = 0;
            elapsedMillis = 0;

            this.position = position;
            previousPosition = position;
            this.scale = scale;
        }
        #endregion

        #region Updateable
        public void Update(GameTime gameTime)
        {
            elapsedMillis += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            Animation animation = animations[currentAnimation];
            if (elapsedMillis >= animation.FrameDelays[frameIndex])
            {
                // Reset elapsed time
                elapsedMillis = 0;

                // Increase frameIndex depending on if the Animation is looped or not
                int frameCount = animation.Frames.Length;
                if (!animation.IsLooped && !animation.IsFinished)
                {
                    if ((frameIndex + 1) == frameCount)
                    {
                        animation.IsFinished = true;
                    }
                    else
                    {
                        ++frameIndex;
                    }
                }
                else
                {
                    frameIndex = ++frameIndex % frameCount;
                }
            }
        }

        #endregion
        #region IDrawable
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Animation currentAnim = animations[currentAnimation];

            spriteBatch.Draw(spritesheet, position + currentAnim.FrameOffsets[frameIndex], currentAnim.Frames[frameIndex], Color.White,
                0, Vector2.Zero, scale, currentAnim.IsMirrored ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1);
        }

        public void LoadContent(ContentManager content)
        {
            spritesheet = content.Load<Texture2D>(spritesheetName);
        }
        #endregion
        #region AnimatedSprite
        public void AddAnimation
        (
            String name, Rectangle[] frames,
            Dictionary<int, Rectangle> hurtBounds,
            Dictionary<int, Rectangle> attackBounds,
            Dictionary<int, int> frameDelays,
            Dictionary<int, Vector2> frameOffsets,
            bool isMirrored,
            bool isLooped
        )
        {
            if (animations.ContainsKey(name))
            {
                Console.WriteLine("Animation '" + name + "' has just been overriden by an Animation with the same name.");
            }
            animations[name] = new Animation(frames, hurtBounds, attackBounds, frameDelays, frameOffsets, isMirrored, isLooped);

            previousAnimation = currentAnimation;
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
        //public void AddAnimation(String name, int frameWidth, int frameHeight, int xColumn, int yRow, int numFrames, int frameDelay)
        //{
        //    Rectangle[] frames = new Rectangle[numFrames];

        //    int spritesheetWidht = spritesheet.Width;
        //    int frame = 0;

        //    // frame++ wird zu früh inkrementiert.
        //    while (frame < numFrames)
        //    {
        //        if (xColumn * frameWidth == spritesheetWidht)
        //        {
        //            ++yRow;
        //            xColumn = 0;
        //        }
        //        frames[frame++] = new Rectangle(xColumn++ * frameWidth, yRow * frameHeight, frameWidth, frameHeight);
        //    }

        //    AddAnimation(name, frames, frameDelay);
        //}

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
            previousAnimation = currentAnimation;
            currentAnimation = name;
            animations[currentAnimation].IsFinished = false;
            elapsedMillis = 0;
            frameIndex = 0;
        }

        public void LoadAnimationsFromFile(String configFilePath)
        {
            AnimationDatasPlusSpriteSheet animDatasPlusSpriteSheet = ConfigFileUtility.ReadAnimationFile(configFilePath);

            spritesheetName = animDatasPlusSpriteSheet.spriteSheet;
            foreach (AnimationData aD in animDatasPlusSpriteSheet.animationDatas)
            {
                AddAnimation(aD.animationName, aD.frames, aD.hurtBounds, aD.attackBounds,
                             aD.frameDelays, aD.frameOffsets, aD.isMirrored, aD.isLooped);
            }
        }

        /// <summary>
        /// If the given Animation is non-looping and is finished
        /// </summary>
        public void ResetAnimation(String animName)
        {

        }

        #endregion
    }
}
