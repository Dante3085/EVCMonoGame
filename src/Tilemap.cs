﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using EVCMonoGame.src.scenes;
using Microsoft.Xna.Framework.Content;

namespace EVCMonoGame.src
{
    public class Tilemap : scenes.IDrawable
    {
        private Texture2D tilemapImage;
        private String tilemapImagePath;
        private Vector2 position;
        private Vector2 tileSize;
        private Dictionary<char, Rectangle> charToTextureBounds;
        private List<List<char>> tilemap;
        private Rectangle tileScreenBounds;

        // RegularExpression for removing Whitespace from lines of Tilemap txt file.
        private static readonly Regex sWhitespace = new Regex(@"\s+");

        public Tilemap(String tilemapFile, Vector2 position)
        {
            this.position = position;
            charToTextureBounds = new Dictionary<char, Rectangle>();
            tilemap = new List<List<char>>();
            tileScreenBounds = new Rectangle();

            readFromFile(tilemapFile);
        }

        private void readFromFile(String tilemapFile)
        {
            string line;

            // Read the file and display it line by line.  
            System.IO.StreamReader file = new System.IO.StreamReader(tilemapFile);

            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains("TILE_MAP_IMAGE="))
                {
                    tilemapImagePath = line.Substring(15, line.Length - 15);
                }
                else if (line.Contains("TILE_SIZE="))
                {
                    // Remove all Whitespace.
                    line = ReplaceWhitespace(line, "");

                    int indexComma = line.IndexOf(',');

                    int tileWidth = int.Parse(line.Substring(10, indexComma - 10));
                    int tileHeight = int.Parse(line.Substring(indexComma + 1, line.Length - (indexComma + 1)));

                    tileSize = new Vector2(tileWidth, tileHeight);
                    tileScreenBounds.Size = tileSize.ToPoint();
                }
                else if (line.Contains("TILE="))
                {
                    // Remove all Whitespace.
                    line = ReplaceWhitespace(line, "");

                    int indexFirstComma  = line.IndexOf(',');
                    int indexSecondComma = line.IndexOf(',', indexFirstComma + 1);
                    int indexThirdComma  = line.IndexOf(',', indexSecondComma + 1);
                    int indexFourthComma = line.IndexOf(',', indexThirdComma + 1);

                    char name          = line.Substring(5, 1).ToCharArray()[0];
                    int  textureX      = int.Parse(line.Substring(indexFirstComma  + 1, indexSecondComma - (indexFirstComma  + 1)));
                    int  textureY      = int.Parse(line.Substring(indexSecondComma + 1, indexThirdComma  - (indexSecondComma + 1)));
                    int  textureWidth  = int.Parse(line.Substring(indexThirdComma  + 1, indexFourthComma - (indexThirdComma  + 1)));
                    int  textureHeight = int.Parse(line.Substring(indexFourthComma + 1, line.Length      - (indexFourthComma + 1)));

                    charToTextureBounds[name] = new Rectangle(textureX, textureY, textureWidth, textureHeight);
                }
                else if (line.Contains("TILE_MAP_DEFINITION"))
                {
                    // Read all lines of Tilemap Definition.
                    while ((line = file.ReadLine()) != null)
                    {
                        // New Line for Tilemap.
                        List<char> newTilemapLine = new List<char>();

                        // Fill new Line of Tilemap.
                        foreach (char c in line)
                        {
                            if (!charToTextureBounds.ContainsKey(c))
                            {
                                throw new ArgumentException("In tilemap file '" + tilemapFile + "' char '" + c +
                                                            "' was used in segment TILE_MAP_DEFINITION to place a Tile," +
                                                            " but it was not registered in a TILE segment.");
                            }
                            else
                            {
                                newTilemapLine.Add(c);
                            }
                        }

                        // Add new Line to Tilemap.
                        tilemap.Add(newTilemapLine);
                    }
                }
            }
            file.Close();
        }
        
        public static string ReplaceWhitespace(string input, string replacement) 
        { 
            return sWhitespace.Replace(input, replacement); 
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < tilemap.Count; ++i)
            {
                tileScreenBounds.Y = (int)(position.Y + (i * tileSize.Y));
                for (int j = 0; j < tilemap[i].Count; ++j)
                {
                    tileScreenBounds.X = (int)(position.X + (j * tileSize.X));
                    spriteBatch.Draw(tilemapImage, tileScreenBounds, charToTextureBounds[tilemap[i][j]], Color.White);
                }
            }
        }

        public void LoadContent(ContentManager content)
        {
            tilemapImage = content.Load<Texture2D>(tilemapImagePath);
        }
    }
}
