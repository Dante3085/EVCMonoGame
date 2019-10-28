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
    public class AnimatedSprite : Updateable, scenes.IDrawable, GeometryCollidable, ITranslatable
    {
        struct Animation
        {
            // TODO: Offsets für frames, weil sonst Verschiebung der Figur/Ankerpunkt.

            public Rectangle[] Frames
            {
                get; set;
            }

            public int FrameDelay
            {
                get; set;
            }

            public bool Mirrored
            {
                get; set;
            }

            public bool IsLooping
            {
                get; set;
            }

            public Animation(Rectangle[] frames, int frameDelay, bool mirrored = false, 
                             bool isLooping = false)
            {
                Frames = frames;
                FrameDelay = frameDelay;
                Mirrored = mirrored;
                IsLooping = isLooping;
            }
        }

        #region InternalVariables

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
                Animation currentAnim = animations[currentAnimation];
                return (frameIndex == currentAnim.Frames.Length - 1)
                     && !currentAnim.IsLooping;
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

        public override void Update(GameTime gameTime)
        {
            elapsedMillis += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            Animation animation = animations[currentAnimation];
            if (elapsedMillis >= animation.FrameDelay)
            {
                elapsedMillis = 0;

                int frameCount = animation.Frames.Length;
                if (!animation.IsLooping)
                {
                    frameIndex = (frameIndex + 1) == frameCount ? frameCount - 1 : ++frameIndex;
                }
                else
                {
                    frameIndex = (frameIndex + 1) == frameCount ? 0 : ++frameIndex;
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Animation currentAnim = animations[currentAnimation];

            spriteBatch.Draw(spritesheet, position, currentAnim.Frames[frameIndex], Color.White,
                0, Vector2.Zero, scale, currentAnim.Mirrored ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1);
        }

        public void LoadContent(ContentManager content)
        {
            spritesheet = content.Load<Texture2D>(spritesheetName);
        }

        public void AddAnimation(String name, Rectangle[] frames, int frameDelay, bool mirrored = false, 
                                 bool isLooping = false)
        {
            if (animations.ContainsKey(name))
            {
                Console.WriteLine("Animation '" + name + "' has just been overriden by an Animation with the same name.");
            }
            animations[name] = new Animation(frames, frameDelay, mirrored, isLooping);

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
        public void AddAnimation(String name, int frameWidth, int frameHeight, int xColumn, int yRow, int numFrames, int frameDelay)
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
            previousAnimation = currentAnimation;
            currentAnimation = name;
            elapsedMillis = 0;
            frameIndex = 0;
        }

        public void LoadFromFile(String configFilePath)
        {
            System.IO.StreamReader file = new System.IO.StreamReader(configFilePath);

            string line;
            while((line = file.ReadLine()) != null)
            {
                // Include char '=' to avoid confusion with other keywords that have
                // this keyword as a substring.
                if (line.Contains("SPRITESHEET="))
                {
                    spritesheetName = line.Substring(12, line.Length - 12);
                }
                else if (line.Contains("ANIMATION="))
                {
                    line = Utility.ReplaceWhitespace(line, "");

                    String animName = "";
                    String frameDelayMillis = "";
                    String isMirrored = "";
                    String isLooping = "";

                    List<Rectangle> frames = new List<Rectangle>();

                    bool readAnimName = false;
                    bool readFrameDelay = false;
                    bool readIsMirrored = false;
                    bool readIsLooping = false;
                    bool readFrame = false;

                    for (int i = 0; i < line.Length; ++i)
                    {
                        // Check for start of Animation name.
                        if (line[i] == '=') 
                        { 
                            readAnimName = true;
                            continue;
                        }

                        if (readAnimName)
                        {
                            // Check if Animation name has been completely read.
                            if (line[i] == ',')
                            {
                                readAnimName = false;
                                readFrameDelay = true;
                                continue;
                            }
                            animName += line[i];
                        }
                        else if (readFrameDelay)
                        {
                            // Check if Frame Delay has been completely read.
                            if (line[i] == ',')
                            {
                                readFrameDelay = false;
                                readIsMirrored = true;
                                continue;
                            }
                            frameDelayMillis += line[i];
                        }
                        else if (readIsMirrored)
                        {
                            if (line[i] == ',')
                            {
                                readIsMirrored = false;
                                readIsLooping = true;
                                continue;
                            }
                            isMirrored += line[i];
                        }
                        else if (readIsLooping)
                        {
                            if (line[i] == ',')
                            {
                                readIsLooping = false;
                                readFrame = true;
                                continue;
                            }
                            isLooping += line[i];
                        }
                        else if (readFrame)
                        {
                            int indexClosingBracket = line.IndexOf(')', i);

                            String frameString = line.Substring(i, indexClosingBracket - (i - 1));
                            frames.Add(ReadFrame(frameString));
                            i = indexClosingBracket + 1;

                        }
                    }
                    AddAnimation(animName, frames.ToArray(), int.Parse(frameDelayMillis), bool.Parse(isMirrored),
                                 bool.Parse(isLooping));
                }
            }
            file.Close();
        }

        private Rectangle ReadFrame(String str)
        {
            Rectangle frame = new Rectangle();

            int indexFirstComma = str.IndexOf(',');
            int indexSecondComma = str.IndexOf(',', indexFirstComma + 1);
            int indexThirdComma = str.IndexOf(',', indexSecondComma + 1);

            frame.X      = int.Parse(str.Substring(1, indexFirstComma - 1));
            frame.Y      = int.Parse(str.Substring(indexFirstComma  + 1,      (indexSecondComma - 1)  - indexFirstComma));
            frame.Width  = int.Parse(str.Substring(indexSecondComma + 1,      (indexThirdComma  - 1)  - indexSecondComma));
            frame.Height = int.Parse(str.Substring(indexThirdComma  + 1, str.Length - (indexThirdComma + 2)));

            return frame;
        }
    }
}
