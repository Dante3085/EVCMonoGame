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
            // public Rectangle[] frames;
            // public int frameDelay;
            // public bool mirrored;

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

            public Animation(Rectangle[] frames, int frameDelay, bool mirrored = false)
            {
                Frames = frames;
                FrameDelay = frameDelay;
                Mirrored = mirrored;
            }
        }

        #region InternalVariables

        private String spritesheetName;
        private Texture2D spritesheet;
        private Dictionary<String, Animation> animations;
        private String currentAnimation;
        private int frameIndex;
        private int elapsedMillis;

        private Vector2 position;
        private Vector2 previousPosition;
        private float scale;

        private SpriteFont debugFont;

        #endregion
        #region Properties

        public Rectangle Bounds
        {
            get
            {
                Rectangle bounds = new Rectangle();
                bounds.Location = position.ToPoint();
                bounds.Size = animations[currentAnimation].Frames[frameIndex].Size;
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
                frameIndex = (frameIndex + 1) == animation.Frames.Length ? 0 : ++frameIndex;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Animation currentAnim = animations[currentAnimation];

            spriteBatch.Draw(spritesheet, position, currentAnim.Frames[frameIndex], Color.White,
                0, Vector2.Zero, scale, currentAnim.Mirrored ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1);

            // spriteBatch.DrawString(debugFont, "frame: " + frameIndex, position, Color.White);
            spriteBatch.DrawString(debugFont, "elapsed: " + elapsedMillis, position, Color.White);
        }

        public void LoadContent(ContentManager content)
        {
            spritesheet = content.Load<Texture2D>(spritesheetName);
            debugFont = content.Load<SpriteFont>("rsrc/fonts/DefaultFont");
        }

        public void AddAnimation(String name, Rectangle[] frames, int frameDelay, bool mirrored = false)
        {
            animations[name] = new Animation(frames, frameDelay, mirrored);
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

        public void SetAnimation(String name, bool mirrored = false)
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
            Animation currentAnim = animations[currentAnimation];
            currentAnim.Mirrored = mirrored;
            animations[currentAnimation] = currentAnim;
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
                    List<Rectangle> frames = new List<Rectangle>();

                    bool readAnimName = false;
                    bool readFrameDelay = false;
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
                                readFrame = true;
                                continue;
                            }
                            frameDelayMillis += line[i];
                        }
                        else if (readFrame)
                        {
                            int indexClosingBracket = line.IndexOf(')', i);

                            String frameString = line.Substring(i, indexClosingBracket - (i - 1));
                            frames.Add(ReadFrame(frameString));
                            i = indexClosingBracket + 1;

                        }
                    }
                    AddAnimation(animName, frames.ToArray(), int.Parse(frameDelayMillis));
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
