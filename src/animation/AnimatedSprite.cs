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
        public class Animation
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
        private Color overlayColor = Color.White;
        private String spritesheetName;
        public Texture2D spritesheet;
        private Dictionary<String, Animation> animations;
        private String currentAnimation;
        private String previousAnimation;
        private int frameIndex;
        private int elapsedMillis;
        private TimeSpan overlayStart;
        private bool isOverlaying = false;
        private TimeSpan overlayDuration;

        private Vector2 position;
        private Vector2 previousPosition;
        private float scale;

        private Rectangle bounds;

        #endregion
        #region Properties

        public Dictionary<String, Animation> Animations
        {
            get
            {
                return animations;
            }
        }

        public float Scale { get { return scale; } }

        public bool FlaggedForRemove
        {
            get; set;
        } = false;

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
            int frameCount = animation.Frames.Length;

            if (elapsedMillis >= animation.FrameDelays[frameIndex])
            {
                // Reset elapsed time
                elapsedMillis = 0;

                // Aktuelle Animation looped => Einfach auf nächstes Frame gehen.
                if (animation.IsLooped)
                {
                    frameIndex = ++frameIndex % frameCount;
                }

                // Aktuelle Animation looped nicht => Auf nächstes Frame gehen und gucken, ob die Animation vorbei ist.
                else
                {
                    if (!animation.IsFinished)
                    {
                        if (frameIndex + 1 == frameCount)
                        {
                            animation.IsFinished = true;
                        }
                        else
                        {
                            ++frameIndex;
                        }
                    }
                }
            }
        }

        #endregion
        #region IDrawable

        public void overlayColorOverTime(Color color, TimeSpan duration)
        {
            overlayColor = color;
            overlayDuration = duration;
            isOverlaying = true;
            overlayStart = Game1.totalGametime;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Animation currentAnim = animations[currentAnimation];
            if (isOverlaying)
            {
                if ((Game1.totalGametime - overlayStart) > overlayDuration)
                {
                    overlayColor = Color.White;
                    isOverlaying = false;
                }
            }
            spriteBatch.Draw(spritesheet, position + currentAnim.FrameOffsets[frameIndex], currentAnim.Frames[frameIndex], overlayColor,
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

        public void LoadAnimationsFromFile(String configFilePath, bool scaleOffsets = false)
        {
            AnimationDatasPlusSpriteSheet animDatasPlusSpriteSheet = ConfigFileUtility.ReadAnimationFile(configFilePath);

            animations.Clear();

            spritesheetName = animDatasPlusSpriteSheet.spriteSheet;
            foreach (AnimationData aD in animDatasPlusSpriteSheet.animationDatas)
            {
                AddAnimation(aD.animationName, aD.frames, aD.hurtBounds, aD.attackBounds,
                             aD.frameDelays, aD.frameOffsets, aD.isMirrored, aD.isLooped);
            }
            if (scaleOffsets)
            {
                List<String> animationNames = new List<string>();
                foreach (String animName in animations.Keys)
                {
                    animationNames.Add(animName);
                }
                for (int animationIndex = 0; animationIndex < animationNames.Count(); animationIndex++)
                {
                    for (int offsetIndex = 0;
                        offsetIndex < animations[animationNames[animationIndex]].FrameOffsets.Count();
                        offsetIndex++)
                    {
                        animations[animationNames[animationIndex]].FrameOffsets[offsetIndex] *= this.scale;
                    }
                }
                for (int animationIndex = 0; animationIndex < animationNames.Count(); animationIndex++)
                {
                    for (int offsetIndex = 0;
                        offsetIndex < animations[animationNames[animationIndex]].AttackBounds.Count();
                        offsetIndex++)
                    {
                        Rectangle r = animations[animationNames[animationIndex]].AttackBounds[offsetIndex];
                        Vector2 v = r.Location.ToVector2() * this.scale;
                        r.Location = v.ToPoint();
                        animations[animationNames[animationIndex]].AttackBounds[offsetIndex] = r;
                    }
                }
            }
        }

        /// <summary>
        /// If the given Animation is non-looping and is finished
        /// </summary>
        //public void ResetAnimation(String animName)
        //{

        //}

        #endregion
    }
}
