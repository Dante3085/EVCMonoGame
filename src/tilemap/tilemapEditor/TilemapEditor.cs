using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using EVCMonoGame.src.input;
using EVCMonoGame.src.gui;
using EVCMonoGame.src.utility;
using EVCMonoGame.src.states;

namespace EVCMonoGame.src.tilemap.tilemapEditor
{
    // TODO: Resolve resolution problems.
    // TODO: Load TileSet and possibly other content in TilemapEditor class.
    // TODO: Properly implement Button.
    // TODO: Der Fakt, dass die Funktionen zum Laden/Speichern über mehrere Klassen verteilt sind ist komisch.

    // TODO: Tile being a class instead of a struct could create problems. 
    //       Struct value type might be smarter if we need to pass Tiles around.

    // TODO: Create a SaveAs Button that opens a FileDialog on being pressed.
    //       After choosing a name and location for the file that shall contain
    //       the current information of the TilemapEditor, the TilemapEditor creates
    //       that file in the previously mentioned FileFormat.

    // TODO: Create a LoadTileSelection Button that loads a TileSet with pre-defined Tiles into
    //       the TileSelection.

    // TODO: Create a LoadTilemap Button that loads a Tilemap that was previously created with
    //       TilemapEditor into the TilemapEditor to work on it further.


    public class Tile
    {
        public String name;
        public Rectangle textureBounds;
        public Rectangle screenBounds;

        public Tile(String name, Rectangle textureBounds, Rectangle screenBounds)
        {
            this.name = name;
            this.textureBounds = textureBounds;
            this.screenBounds = screenBounds;
        }

        public Tile(Tile otherTile)
        {
            this.name = otherTile.name;
            this.textureBounds = otherTile.textureBounds;
            this.screenBounds = otherTile.screenBounds;
        }

        public override string ToString()
        {
            return "TILE{" + name + ", " + Utility.RectangleToString(textureBounds) + ", " + 
                   Utility.RectangleToString(screenBounds) + "}"; 
        }
    }

    // (Moritz): Ich habe den TilemapEditor extra nicht von scenes.Updateable erben und scenes.IDrawable implementieren lassen,
    //           weil er im Hauptmenü durch den TilemapEditorState erreichbar sein soll. Die Klassen im states Ordner haben nichts
    //           mit den Klassen in unserem scenes Ordner zu tun.
    public class TilemapEditor
    {
        #region Fields

        private TileSelection tileSelection;
        private DrawingArea drawingArea;

        private SpriteFont font;
        private Vector2 fontSize;
        private bool drawInfoText = false;

        private Viewport viewport;

        #endregion

        #region Properties

        public SpriteFont Font
        {
            get { return font; }
        }

        #endregion

        public TilemapEditor()
        {
            this.viewport = viewport;

            tileSelection = new TileSelection(new Vector2(0, 0), new Vector2(100, 100), 3, new Vector2(1, 1),
                               "Content/rsrc/tilesets/configFiles/overworld_tiles.ts.txt");
        }

        public void Update(GameTime gameTime)
        {
            tileSelection.Update(gameTime);
            drawingArea.Update(gameTime);

            CheckForSavingToFile();
            CheckForLoadingFromFile();

            if (InputManager.OnKeyPressed(Microsoft.Xna.Framework.Input.Keys.I))
            {
                drawInfoText = !drawInfoText;
            }
        }

        #region UpdateHelper
        private void CheckForSavingToFile()
        {
            // Namespace muss hier voll spezifiziert sein wegen WindowsForms ambiguity.
            // Vielleicht in ConfigFileUtility verschieben ?
            if (InputManager.OnKeyPressed(Microsoft.Xna.Framework.Input.Keys.S))
            {
                String fileName = String.Empty;
                bool saveFileDialogCanceled = false;
                SaveFileDialog saveFileDialog = new SaveFileDialog();

                Thread dialogThread = new Thread(() =>
                {
                    DialogResult dialogResult = saveFileDialog.ShowDialog();

                    if (dialogResult == DialogResult.OK)
                    {
                        saveFileDialogCanceled = false;
                        fileName = saveFileDialog.FileName;
                    }
                    else if (dialogResult == DialogResult.Cancel)
                    {
                        saveFileDialogCanceled = true;
                    }
                });
                dialogThread.SetApartmentState(ApartmentState.STA);
                dialogThread.Start();

                // Wait for the SaveFileDialog to close.
                dialogThread.Join();

                if (!saveFileDialogCanceled)
                {
                    drawingArea.SaveToFile(fileName);
                }
            }
        }

        private void CheckForLoadingFromFile()
        {
            if (InputManager.OnKeyPressed(Microsoft.Xna.Framework.Input.Keys.L))
            {
                String fileName = String.Empty;
                bool openFileDialogCanceled = false;
                OpenFileDialog openFileDialog = new OpenFileDialog();

                Thread dialogThread = new Thread(() =>
                {
                    DialogResult dialogResult = openFileDialog.ShowDialog();

                    if (dialogResult == DialogResult.OK)
                    {
                        openFileDialogCanceled = false;
                        fileName = openFileDialog.FileName;
                    }
                    else if (dialogResult == DialogResult.Cancel)
                    {
                        openFileDialogCanceled = true;
                    }
                });
                dialogThread.SetApartmentState(ApartmentState.STA);
                dialogThread.Start();

                // Wait for the SaveFileDialog to close.
                dialogThread.Join();

                if (!openFileDialogCanceled)
                {
                    ReadTilemapFile(fileName);
                }
            }
        }
        #endregion

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            drawingArea.Draw(gameTime, spriteBatch);
            tileSelection.Draw(gameTime, spriteBatch);

            if (drawInfoText)
            {
                Vector2 textPosition = viewport.Bounds.Size.ToVector2() * new Vector2(0.5f, 0.5f) - 
                                       (fontSize * new Vector2(0.5f, 0.5f));

                spriteBatch.Begin();

                spriteBatch.DrawString(font, "Hotkeys\n" +
                                             "-------\n" + 
                                             "S: Save\n" + 
                                             "L: Load\n",
                                             textPosition, Color.White);

                spriteBatch.End();
            }
        }

        public void LoadContent(ContentManager content, Viewport viewport)
        {
            this.viewport = viewport;

            font = content.Load<SpriteFont>("rsrc/fonts/DefaultFont");
            fontSize = font.MeasureString("TEST");

            drawingArea = new DrawingArea(viewport.Bounds, tileSelection);

            tileSelection.LoadContent(content);
            drawingArea.LoadContent(content);
        }

        private void ReadTilemapFile(String path)
        {
            if (!path.EndsWith(".tm.txt"))
            {
                throw new ArgumentException("Given file '" + path + "' is not an tm(Tilemap)File.\n" +
                    "Provide a file that ends with '.tm.txt'.");
            }

            System.IO.StreamReader reader = new System.IO.StreamReader(path);
            String line = String.Empty;

            // Variables for things that will be read.
            String tileSetName = String.Empty;
            String tileName = String.Empty;
            Rectangle textureBounds = Rectangle.Empty;
            Rectangle screenBounds = Rectangle.Empty;
            List<Tile> tiles = new List<Tile>();

            while((line = reader.ReadLine()) != null)
            {
                // Find section
                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    // Determine specific section
                    if (line.Contains("TILE"))
                    {
                        line = Utility.ReplaceWhitespace(reader.ReadLine(), ""); // Remove Whitespace
                        tileName = line.Remove(0, 5); // Remove 'NAME='

                        line = Utility.ReplaceWhitespace(reader.ReadLine(), "");
                        line = line.Remove(0, 15); // Remove 'TEXTURE_BOUNDS='
                        textureBounds = Utility.StringToRectangle(line);

                        line = Utility.ReplaceWhitespace(reader.ReadLine(), "");
                        line = line.Remove(0, 14); // Remove 'SCREEN_BOUDNDS='
                        screenBounds = Utility.StringToRectangle(line);

                        tiles.Add(new Tile(tileName, textureBounds, screenBounds));

                        //if (numTilesInLastRow == numTilesPerRow)
                        //{
                        //    tiles.Add(new List<Tile>());
                        //    numTilesInLastRow = 0;
                        //}
                        //tiles[tiles.Count - 1].Add(new Tile(tileName, tileBounds, Rectangle.Empty));
                        //++numTilesInLastRow;
                    }
                }
                else if (line.Contains("TILESET"))
                {
                    line = Utility.ReplaceWhitespace(line, "");
                    tileSetName = line.Substring(8); // Read everything after 'TILESET='
                }
            }

            drawingArea.Tiles = tiles;

            // Figure out how to deal with new/old tileset.
            // Load Tileset if it has never been loaded and
            // just set it if it has been loaded before.
            // tileSelection.TileSet = tilese

            reader.Close();
        }
    }
}
