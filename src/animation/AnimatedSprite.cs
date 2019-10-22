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

namespace EVCMonoGame.src.animation
{
    // TODO: Optimize LoadFromFile() and HelperMethods. ~20ms is a bit long
    // TODO: Implement frameOffsets.

    public class AnimatedSprite : Updateable, scenes.IDrawable, GeometryCollidable
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

        #endregion
        #region Properties

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
                Rectangle bounds = new Rectangle();
                bounds.Location = position.ToPoint();
                bounds.Size = animations["IDLE_DOWN"].Frames[0].Size;
                bounds.Width *= (int)(scale);
                bounds.Height *= (int)(scale);

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
        public override void Update(GameTime gameTime)
        {
            elapsedMillis += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            Animation animation = animations[currentAnimation];
            if (elapsedMillis >= animation.FrameDelays[frameIndex])
            {
                // Reset elapsed time
                elapsedMillis = 0;

                // Increase frameIndex depending on if the Animation is looped or not
                int frameCount = animation.Frames.Length;
                if (!animation.IsLooped)
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

        public void LoadFromFile(String configFilePath)
        {
            System.IO.StreamReader file = new System.IO.StreamReader(configFilePath);

            // Prepare Variables for all the things that will be read.
            String name;
            List<Rectangle> frames = new List<Rectangle>();
            Dictionary<int, Rectangle> hurtBounds = new Dictionary<int, Rectangle>();
            Dictionary<int, Rectangle> attackBounds = new Dictionary<int, Rectangle>();
            Dictionary<int, int> frameDelays = new Dictionary<int, int>();
            Dictionary<int, Vector2> frameOffsets = new Dictionary<int, Vector2>();
            bool isMirrored;
            bool isLooped;

            string line;
            while ((line = file.ReadLine()) != null)
            {
                // Find section
                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    // Determine specific section
                    if (line.Contains("ANIMATION"))
                    {
                        // Process Name
                        line = Utility.ReplaceWhitespace(file.ReadLine(), "");
                        name = line.Remove(0, 5);

                        // Process Frames
                        line = Utility.ReplaceWhitespace(file.ReadLine(), "");
                        line = line.Remove(0, 7); // Remove 'FRAMES='
                        frames = ReadFrames(line);

                        // Process HurtBounds.
                        line = Utility.ReplaceWhitespace(file.ReadLine(), "");
                        line = line.Remove(0, 12); // Remove 'HURT_BOUNDS='
                        hurtBounds = ReadHurtBounds(line, frames);

                        if (hurtBounds.Count != frames.Count)
                        {
                            throw new ArgumentException("Animation: " + name + ", numFrames = " + frames.Count + 
                                                        " != numHurtBounds = " + hurtBounds.Count);
                        }

                        // Process AttackBounds.
                        line = Utility.ReplaceWhitespace(file.ReadLine(), "");
                        line = line.Remove(0, 14); // Remove 'ATTACK_BOUNDS='
                        attackBounds = ReadAttackBounds(line, frames.Count);

                        if (attackBounds.Count != frames.Count)
                        {
                            throw new ArgumentException("Animation: " + name + ", numFrames = " + frames.Count +
                                                        " != numAttackBounds = " + attackBounds.Count);
                        }

                        // Process FrameDelays
                        line = Utility.ReplaceWhitespace(file.ReadLine(), "");
                        line = line.Remove(0, 13); // Remove 'FRAME_DELAYS='
                        frameDelays = ReadFrameDelays(line, frames.Count);

                        if (frameDelays.Count != frames.Count)
                        {
                            throw new ArgumentException("Animation: " + name + ", numFrames = " + frames.Count + 
                                                        " != numFrameDelays = " + frameDelays.Count);
                        }

                        // Process FrameOffsets
                        line = Utility.ReplaceWhitespace(file.ReadLine(), "");
                        line = line.Remove(0, 14);
                        frameOffsets = ReadFrameOffsets(line, frames.Count);

                        if (frameOffsets.Count != frames.Count)
                        {
                            throw new ArgumentException("Animation: " + name + ", numFrames = " + frames.Count + 
                                                        " != numFrameOffsets = " + frameOffsets.Count);
                        }

                        // Process IsMirrored
                        line = Utility.ReplaceWhitespace(file.ReadLine(), "");
                        line = line.Remove(0, 12);
                        isMirrored = bool.Parse(line);

                        // Process IsLooped
                        line = Utility.ReplaceWhitespace(file.ReadLine(), "");
                        line = line.Remove(0, 10);
                        isLooped = bool.Parse(line);

                        AddAnimation(name, frames.ToArray(), hurtBounds, attackBounds, frameDelays, frameOffsets, isMirrored, isLooped);
                    }
                }

                // Process Spritesheet
                else if (line.Contains("SPRITESHEET"))
                {
                    line = Utility.ReplaceWhitespace(line, "");
                    spritesheetName = line.Substring(12);
                }
            }
            file.Close();
        }

        #region LoadFromFileHelperMethods
        private List<Rectangle> ReadFrames(String line)
        {
            List<Rectangle> frames = new List<Rectangle>();

            // Parse all the frames.
            for (int i = 0; i < line.Length; ++i)
            {
                int indexClosingBracket = line.IndexOf(')', i);
                frames.Add(StringToRectangle(line.Substring(i, indexClosingBracket - (i - 1))));
                i = indexClosingBracket + 1;
            }

            return frames;
        }

        // TODO: Similar to ReadFrameDelays()
        private Dictionary<int, Rectangle> ReadHurtBounds(String line, List<Rectangle> frames)
        {
            Dictionary<int, Rectangle> hurtBounds = new Dictionary<int, Rectangle>();

            // No HurtBounds is interpreted as every frame having a basically non-existing hurtBound(no size and at orign).
            if (line == "NONE")
            {
                for (int i = 0; i < frames.Count; ++i)
                {
                    hurtBounds.Add(i, new Rectangle(0, 0, 0, 0));
                }
            }

            // Multiple hurtBounds separated by commas.
            else if (line.Contains("),"))
            {
                int hurtBoundCounter = 0;
                for (int i = 0; i < line.Length; ++i)
                {
                    int indexClosingBracket = line.IndexOf(')', i);
                    hurtBounds.Add(hurtBoundCounter++, StringToRectangle(line.Substring(i, indexClosingBracket - (i - 1))));
                    i = indexClosingBracket + 1;
                }
            }

            // Only one hurtBound.
            else
            {
                // Each frame gets hurtBound (0, 0, frame.width, frame.height).
                if (line == "SAME_AS_FRAME")
                {
                    Rectangle hurtBound;
                    for (int i = 0; i < frames.Count; ++i)
                    {
                        hurtBound = frames[i];
                        hurtBound.X = 0;
                        hurtBound.Y = 0;

                        hurtBounds.Add(i, hurtBound);
                    }
                }

                // One hurtBound for all frames
                else if (line.EndsWith("@all"))
                {
                    Rectangle hurtBound = StringToRectangle(line.Substring(0, line.IndexOf("@all")));

                    for (int i = 0; i < frames.Count; ++i)
                    {
                        hurtBounds.Add(i, hurtBound);
                    }
                }

                // One hurtBound for one frame
                else
                {
                    hurtBounds.Add(0, StringToRectangle(line));
                }
            }
            return hurtBounds;
        }

        private Dictionary<int, Rectangle> ReadAttackBounds(String line, int numFrames)
        {
            Dictionary<int, Rectangle> attackBounds = new Dictionary<int, Rectangle>();

            // No Attackbounds is interpreted as every frame having a basically non-existing AttackBound(no size and at orign).
            if (line == "NONE")
            {
                for (int i = 0; i < numFrames; ++i)
                {
                    attackBounds.Add(i, new Rectangle(0, 0, 0, 0));
                }
            }

            // Multiple attackBounds separated by commas.
            else if (line.Contains("),"))
            {
                int attackBoundCounter = 0;
                for (int i = 0; i < line.Length; ++i)
                {
                    int indexClosingBracket = line.IndexOf(')', i);
                    attackBounds.Add(attackBoundCounter++, StringToRectangle(line.Substring(i, indexClosingBracket - (i - 1))));
                    i = indexClosingBracket + 1;
                }
            }

            // Only one attackBound.
            else
            {
                // One attackBound for all frames
                if (line.EndsWith("@all"))
                {
                    Rectangle attackBound = StringToRectangle(line.Substring(0, line.IndexOf("@all")));

                    for (int i = 0; i < numFrames; ++i)
                    {
                        attackBounds.Add(i, attackBound);
                    }
                }

                // One attackBound for one frame
                else
                {
                    attackBounds.Add(0, StringToRectangle(line));
                }
            }

            return attackBounds;
        }

        private Dictionary<int, int> ReadFrameDelays(String line, int numFrames)
        {
            Dictionary<int, int> frameDelays = new Dictionary<int, int>();

            // Multiple frameDelays separated with commas.
            if (line.Contains(','))
            {
                String[] frameDelayStrings = line.Split(',');

                for (int i = 0; i < frameDelayStrings.Length; ++i)
                {
                    frameDelays.Add(i, int.Parse(frameDelayStrings[i]));
                }
            }

            // Only one frameDelay.
            else
            {

                // One frameDelay for all frames.
                if (line.EndsWith("@all"))
                {
                    int frameDelay = int.Parse(line.Substring(0, line.IndexOf("@all")));

                    for (int i = 0; i < numFrames; ++i)
                    {
                        frameDelays.Add(i, frameDelay);
                    }
                }

                // One frameDelay for one frame.
                else
                {
                    frameDelays.Add(0, int.Parse(line));
                }
            }

            return frameDelays;
        }

        private Dictionary<int, Vector2> ReadFrameOffsets(String line, int numFrames)
        {
            Dictionary<int, Vector2> frameOffsets = new Dictionary<int, Vector2>();

            // Multiple frameOffsets separated by commas.
            if (line.Contains("),"))
            {
                int frameCounter = 0;
                for (int i = 0; i < line.Length; ++i)
                {
                    int indexClosingBracket = line.IndexOf(')', i);
                    frameOffsets.Add(frameCounter++, ReadFrameOffset(line.Substring(i, indexClosingBracket - (i - 1))));
                    i = indexClosingBracket + 1;
                }

            }

            // Only one frameOffset.
            else
            {

                // One frameOffset for all frames.
                if (line.EndsWith("@all"))
                {
                    Vector2 frameOffset = ReadFrameOffset(line.Substring(0, line.IndexOf("@all")));

                    for (int i = 0; i < numFrames; ++i)
                    {
                        frameOffsets.Add(i, frameOffset);
                    }
                }

                // One frameOffset for one frame.
                else
                {
                    frameOffsets.Add(0, ReadFrameOffset(line));
                }
            }

            return frameOffsets;
        }

        private Vector2 ReadFrameOffset(String frameOffsetString)
        {
            int indexComma = frameOffsetString.IndexOf(',');

            Vector2 frameOffset = Vector2.Zero;
            frameOffset.X = int.Parse(frameOffsetString.Substring(1, indexComma - 1));
            frameOffset.Y = int.Parse(frameOffsetString.Substring(indexComma + 1,
                                      (frameOffsetString.Length - 2) - (indexComma)));

            return frameOffset;
        }

        /// <summary>
        /// Converts a String of format "(x, y, width, height)" to a Rectangle instance.
        /// </summary>
        /// <param name="recString"></param>
        /// <returns></returns>
        private Rectangle StringToRectangle(String recString)
        {
            Rectangle rectangle = new Rectangle();

            int indexFirstComma = recString.IndexOf(',');
            int indexSecondComma = recString.IndexOf(',', indexFirstComma + 1);
            int indexThirdComma = recString.IndexOf(',', indexSecondComma + 1);

            rectangle.X = int.Parse(recString.Substring(1, indexFirstComma - 1));
            rectangle.Y = int.Parse(recString.Substring(indexFirstComma + 1, (indexSecondComma - 1) - indexFirstComma));
            rectangle.Width = int.Parse(recString.Substring(indexSecondComma + 1, (indexThirdComma - 1) - indexSecondComma));
            rectangle.Height = int.Parse(recString.Substring(indexThirdComma + 1, recString.Length - (indexThirdComma + 2)));

            return rectangle;
        }
        #endregion

        #endregion
    }
}
