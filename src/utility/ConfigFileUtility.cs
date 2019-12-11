using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

using EVCMonoGame.src.tilemap;

namespace EVCMonoGame.src.utility
{
    public struct AnimationData
    {
        public String animationName;
        public Rectangle[] frames;
        public Dictionary<int, Rectangle> hurtBounds;
        public Dictionary<int, Rectangle> attackBounds;
        public Dictionary<int, int> frameDelays;
        public Dictionary<int, Vector2> frameOffsets;
        public bool isMirrored;
        public bool isLooped;

        public AnimationData
        (
            String animationName, 
            Rectangle[] frames, 
            Dictionary<int, Rectangle> hurtBounds,
            Dictionary<int, Rectangle> attackBounds,
            Dictionary<int, int> frameDelays,
            Dictionary<int, Vector2> frameOffsets,
            bool isMirrored,
            bool isLooped
        )
        {
            this.animationName = animationName;
            this.frames        = frames;
            this.hurtBounds    = hurtBounds;
            this.attackBounds  = attackBounds;
            this.frameDelays   = frameDelays;
            this.frameOffsets  = frameOffsets;
            this.isMirrored    = isMirrored;
            this.isLooped      = isLooped;
        }
    }

    // TODO: Use Animation instead of AnimationData to avoid this extra class.
    public class AnimationDatasPlusSpriteSheet
    {
        public List<AnimationData> animationDatas = new List<AnimationData>();
        public String spriteSheet = String.Empty;
    }

    public struct TileSelectionData
    {
        public List<List<Tile>> tiles;
        public String tileSetName;

        public TileSelectionData(List<List<Tile>> tiles, String tileSetName)
        {
            this.tiles = tiles;
            this.tileSetName = tileSetName;
        }
    }

    public struct TilemapData
    {

    }

    public static class ConfigFileUtility
    {
        /// <summary>
        /// Processes an Animation File(.anm.txt) and returns it's contents.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static AnimationDatasPlusSpriteSheet ReadAnimationFile(String path)
        {
            if (!path.EndsWith(".anm.txt"))
            {
                throw new ArgumentException("Given file '" + path + "' is not an anm(Animation)File.\n" +
                    "Provide a file that ends with '.anm.txt'.");
            }

            
            
            System.IO.StreamReader file = new System.IO.StreamReader(path);

            AnimationDatasPlusSpriteSheet animDataPlusSpriteSheet = new AnimationDatasPlusSpriteSheet();

            // Prepare Variables for all the things that will be read.
            String spriteSheetName = String.Empty;
            String name = String.Empty;
            List<Rectangle> frames = new List<Rectangle>();
            Dictionary<int, Rectangle> hurtBounds = new Dictionary<int, Rectangle>();
            Dictionary<int, Rectangle> attackBounds = new Dictionary<int, Rectangle>();
            Dictionary<int, int> frameDelays = new Dictionary<int, int>();
            Dictionary<int, Vector2> frameOffsets = new Dictionary<int, Vector2>();
            bool isMirrored = false;
            bool isLooped   = false;

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
                        attackBounds = ReadAttackBounds(line, frames);

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

                        animDataPlusSpriteSheet.animationDatas.Add
                        (
                            new AnimationData
                            (
                              name,
                              frames.ToArray(),
                              hurtBounds,
                              attackBounds,
                              frameDelays,
                              frameOffsets,
                              isMirrored,
                              isLooped
                            )
                        );
                    }
                }

                // Process Spritesheet
                else if (line.Contains("SPRITESHEET"))
                {
                    line = Utility.ReplaceWhitespace(line, "");
                    animDataPlusSpriteSheet.spriteSheet = line.Substring(12);
                }
            }
            file.Close();

            return animDataPlusSpriteSheet;
        }

        #region ReadAnimationFileHelper
        private static List<Rectangle> ReadFrames(String line)
        {
            List<Rectangle> frames = new List<Rectangle>();

            // Parse all the frames.
            for (int i = 0; i < line.Length; ++i)
            {
                int indexClosingBracket = line.IndexOf(')', i);
                frames.Add(Utility.StringToRectangle(line.Substring(i, indexClosingBracket - (i - 1))));
                i = indexClosingBracket + 1;
            }

            return frames;
        }

        // TODO: Similar to ReadFrameDelays()
        private static Dictionary<int, Rectangle> ReadHurtBounds(String line, List<Rectangle> frames)
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
                    hurtBounds.Add(hurtBoundCounter++, Utility.StringToRectangle(line.Substring(i, indexClosingBracket - (i - 1))));
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
                    Rectangle hurtBound = Utility.StringToRectangle(line.Substring(0, line.IndexOf("@all")));

                    for (int i = 0; i < frames.Count; ++i)
                    {
                        hurtBounds.Add(i, hurtBound);
                    }
                }

                // One hurtBound for one frame
                else
                {
                    hurtBounds.Add(0, Utility.StringToRectangle(line));
                }
            }
            return hurtBounds;
        }

        private static Dictionary<int, Rectangle> ReadAttackBounds(String line, List<Rectangle> frames)
        {
            Dictionary<int, Rectangle> attackBounds = new Dictionary<int, Rectangle>();

            // No Attackbounds is interpreted as every frame having a basically non-existing AttackBound(no size and at orign).
            if (line == "NONE")
            {
                for (int i = 0; i < frames.Count; ++i)
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
                    attackBounds.Add(attackBoundCounter++, Utility.StringToRectangle(line.Substring(i, indexClosingBracket - (i - 1))));
                    i = indexClosingBracket + 1;
                }
            }

            // Only one attackBound.
            else
            {
                // Each frame gets hurtBound (0, 0, frame.width, frame.height).
                if (line == "SAME_AS_FRAME")
                {
                    Rectangle attackBound;
                    for (int i = 0; i < frames.Count; ++i)
                    {
                        attackBound = frames[i];
                        attackBound.X = 0;
                        attackBound.Y = 0;

                        attackBounds.Add(i, attackBound);
                    }
                }

                // One attackBound for all frames
                else if (line.EndsWith("@all"))
                {
                    Rectangle attackBound = Utility.StringToRectangle(line.Substring(0, line.IndexOf("@all")));

                    for (int i = 0; i < frames.Count; ++i)
                    {
                        attackBounds.Add(i, attackBound);
                    }
                }

                // One attackBound for one frame
                else
                {
                    attackBounds.Add(0, Utility.StringToRectangle(line));
                }
            }

            return attackBounds;
        }

        private static Dictionary<int, int> ReadFrameDelays(String line, int numFrames)
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

        private static Dictionary<int, Vector2> ReadFrameOffsets(String line, int numFrames)
        {
            Dictionary<int, Vector2> frameOffsets = new Dictionary<int, Vector2>();

            // Multiple frameOffsets separated by commas.
            if (line.Contains("),"))
            {
                int frameCounter = 0;
                for (int i = 0; i < line.Length; ++i)
                {
                    int indexClosingBracket = line.IndexOf(')', i);
                    frameOffsets.Add(frameCounter++, Utility.StringToVector2(line.Substring(i, indexClosingBracket - (i - 1))));
                    i = indexClosingBracket + 1;
                }

            }

            // Only one frameOffset.
            else
            {

                // One frameOffset for all frames.
                if (line.EndsWith("@all"))
                {
                    Vector2 frameOffset = Utility.StringToVector2(line.Substring(0, line.IndexOf("@all")));

                    for (int i = 0; i < numFrames; ++i)
                    {
                        frameOffsets.Add(i, frameOffset);
                    }
                }

                // One frameOffset for one frame.
                else
                {
                    frameOffsets.Add(0, Utility.StringToVector2(line));
                }
            }

            return frameOffsets;
        }
        #endregion

        public static TileSelectionData ReadTileSelectionFile(String path, int numTilesPerRow)
        {
            if (!path.EndsWith(".ts.txt"))
            {
                throw new ArgumentException("Given file '" + path + "' is not a ts(TileSelection)File.\n" +
                    "Provide a file that ends with '.ts.txt'.");
            }

            System.IO.StreamReader reader = new System.IO.StreamReader(path);
            String line = String.Empty;

            // Variables for storing the information that will be read.
            List<List<Tile>> tiles = new List<List<Tile>>();
            tiles.Add(new List<Tile>());

            String tileSetName = String.Empty;

            while ((line = reader.ReadLine()) != null)
            {
                // Find section
                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    // Determine specific section
                    if (line.Contains("TILE"))
                    {
                        String tileName = String.Empty;
                        Rectangle textureBounds = Rectangle.Empty;

                        line = Utility.ReplaceWhitespace(reader.ReadLine(), ""); // Remove Whitespace
                        tileName = line.Remove(0, 5); // Remove 'NAME='

                        line = Utility.ReplaceWhitespace(reader.ReadLine(), "");
                        line = line.Remove(0, 15); // Remove 'TEXTURE_BOUNDS='
                        textureBounds = Utility.StringToRectangle(line);

                        if (tiles[tiles.Count - 1].Count == numTilesPerRow)
                        {
                            tiles.Add(new List<Tile>());
                        }
                        tiles[tiles.Count - 1].Add(new Tile(tileName, textureBounds, Rectangle.Empty));
                    }
                }
                else if (line.Contains("TILESET"))
                {
                    line = Utility.ReplaceWhitespace(line, "");
                    tileSetName = line.Substring(8); // Read everything after 'TILESET='
                }

                // Read all Tiles of a specified size in a specified region.
                else if (line.Contains("AUTO_TILES"))
                {
                    line = Utility.ReplaceWhitespace(line, "");
                    line = line.Remove(0, 11); // Remove 'AUTO_TILES='

                    Vector2 tileSize = Utility.StringToVector2(line.Substring(0, line.IndexOf("@")));
                    Rectangle region = Utility.StringToRectangle(line.Substring(line.IndexOf("@") + 1));
                    int row = 0;
                    int column = 0;
                    int numAutoTiles = 0;
                    Rectangle textureBounds = Rectangle.Empty;

                    while((region.Location.ToVector2().Y + (row * tileSize.Y)) < region.Bottom)
                    {
                        if ((region.Location.ToVector2().X + (column * tileSize.X)) >= region.Right)
                        {
                            ++row;
                            column = 0;
                        }

                        textureBounds = new Rectangle((region.Location.ToVector2() + new Vector2(column++ * tileSize.X, row * tileSize.Y)).ToPoint(),
                                                      tileSize.ToPoint());

                        if (tiles[tiles.Count - 1].Count == numTilesPerRow)
                        {
                            tiles.Add(new List<Tile>());
                        }
                        tiles[tiles.Count - 1].Add(new Tile("auto_tile_" + numAutoTiles++.ToString(), 
                                                   textureBounds, Rectangle.Empty));
                    }
                }
            }
            reader.Close();

            return new TileSelectionData(tiles, tileSetName);
        }
    }
}
