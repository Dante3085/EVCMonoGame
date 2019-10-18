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
    // TODO: Optimize LoadFromFile() and HelperMethods. ~20ms is a bit long
    // TODO: Implement frameOffsets.

    public class AnimatedSprite : Updateable, scenes.IDrawable, GeometryCollidable
    {
        #region AnimationStruct
        class  Animation
        {
            public Rectangle[] Frames
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

            public Animation(Rectangle[] frames, Dictionary<int, int> frameDelays, Dictionary<int, Vector2> frameOffsets, 
                             bool isMirrored, bool isLooped)
            {
                Frames = frames;
                FrameDelays = frameDelays;
                FrameOffsets = frameOffsets;
                IsMirrored = isMirrored;
                IsLooped = isLooped;

                isFinished = false;
            }
        }
        #endregion
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

        #region UpdateableMethods
        public override void Update(GameTime gameTime)
        {
            elapsedMillis += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            Animation animation = animations[currentAnimation];
            if (elapsedMillis >= animation.FrameDelays[frameIndex])
            {
                elapsedMillis = 0;

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
        #region IDrawableMethods
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Animation currentAnim = animations[currentAnimation];

            spriteBatch.Draw(spritesheet, position + currentAnim.FrameOffsets[frameIndex], currentAnim.Frames[frameIndex], Color.White,
                0, Vector2.Zero, scale, currentAnim.IsMirrored ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1);

            Console.WriteLine(currentAnim.FrameOffsets[frameIndex]);
        }

        public void LoadContent(ContentManager content)
        {
            spritesheet = content.Load<Texture2D>(spritesheetName);
        }
        #endregion
        #region AnimatedSpriteMethods
        public void AddAnimation(String name, Rectangle[] frames, Dictionary<int, int> frameDelays, 
                                 Dictionary<int, Vector2> frameOffsets, bool isMirrored, bool isLooped)
        {
            if (animations.ContainsKey(name))
            {
                Console.WriteLine("Animation '" + name + "' has just been overriden by an Animation with the same name.");
            }
            animations[name] = new Animation(frames, frameDelays, frameOffsets, isMirrored, isLooped);

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

            String name;
            List<Rectangle> frames = new List<Rectangle>();
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


                        // Process FrameDelays
                        line = Utility.ReplaceWhitespace(file.ReadLine(), "");
                        line = line.Remove(0, 13); // Remove 'FRAME_DELAYS='
                        frameDelays = ReadFrameDelays(line, frames.Count);


                        // Process FrameOffsets
                        line = Utility.ReplaceWhitespace(file.ReadLine(), "");
                        line = line.Remove(0, 14);
                        frameOffsets = ReadFrameOffsets(line, frames.Count);


                        // Process IsMirrored
                        line = Utility.ReplaceWhitespace(file.ReadLine(), "");
                        line = line.Remove(0, 12);
                        isMirrored = bool.Parse(line);


                        // Process IsLooped
                        line = Utility.ReplaceWhitespace(file.ReadLine(), "");
                        line = line.Remove(0, 10);
                        isLooped = bool.Parse(line);

                        AddAnimation(name, frames.ToArray(), frameDelays, frameOffsets, isMirrored, isLooped);
                    }
                }

                // Process Spritesheet
                else if (line.Contains("SPRITESHEET"))
                {
                    int equalsIndex = line.IndexOf('=');
                    spritesheetName = line.Substring(equalsIndex + 1, (line.Length - 1) - equalsIndex);
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
                frames.Add(ReadFrame(line.Substring(i, indexClosingBracket - (i - 1))));
                i = indexClosingBracket + 1;
            }

            return frames;
        }

        private Rectangle ReadFrame(String frameString)
        {
            Rectangle frame = new Rectangle();

            int indexFirstComma = frameString.IndexOf(',');
            int indexSecondComma = frameString.IndexOf(',', indexFirstComma + 1);
            int indexThirdComma = frameString.IndexOf(',', indexSecondComma + 1);

            frame.X = int.Parse(frameString.Substring(1, indexFirstComma - 1));
            frame.Y = int.Parse(frameString.Substring(indexFirstComma + 1, (indexSecondComma - 1) - indexFirstComma));
            frame.Width = int.Parse(frameString.Substring(indexSecondComma + 1, (indexThirdComma - 1) - indexSecondComma));
            frame.Height = int.Parse(frameString.Substring(indexThirdComma + 1, frameString.Length - (indexThirdComma + 2)));

            return frame;
        }

        private Dictionary<int, int> ReadFrameDelays(String line, int numFrames)
        {
            Dictionary<int, int> frameDelays = new Dictionary<int, int>();

            // Multiple frameDelays separated with commas.
            if (line.Contains(','))
            {
                String[] frameDelayStrings = line.Split(',');

                if (frameDelayStrings.Length != numFrames)
                {
                    throw new ArgumentException("numFrames = " + numFrames + " != numFrameDelays = " + frameDelayStrings.Length);
                }

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
                    if (numFrames > 1)
                    {
                        throw new ArgumentException("numFrames = " + numFrames + " != numFrameDelays = " + 1);
                    }
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
                    if (numFrames > 1)
                    {
                        throw new ArgumentException("numFrames = " + numFrames + " != numFrameOffsets = " + 1);
                    }
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
        #endregion

        #endregion
    }
}
