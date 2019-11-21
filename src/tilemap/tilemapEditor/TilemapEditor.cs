using EVCMonoGame.src.input;
using EVCMonoGame.src.utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using EVCMonoGame.src.states;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

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

    // (Moritz): Ich habe den TilemapEditor extra nicht von scenes.Updateable erben und scenes.IDrawable implementieren lassen,
    //           weil er im Hauptmenü durch den TilemapEditorState erreichbar sein soll. Die Klassen im states Ordner haben nichts
    //           mit den Klassen in unserem scenes Ordner zu tun.

    public class TilemapEditor
    {
        #region Fields

        private TileSelection tileSelection;
        private DrawingArea drawingArea;

        private SpriteFont font;
        private String infoText = String.Empty;
        private Vector2 fontSize;
        private bool drawInfoText = false;

        private Viewport viewport;

        private ContentManager content;

        private StateManager stateManager;

        #endregion

        #region Properties

        public SpriteFont Font
        {
            get { return font; }
        }

        #endregion

        public TilemapEditor()
        {
            this.stateManager = stateManager;
            tileSelection = new TileSelection(new Vector2(0, 0), new Vector2(64, 64), 5, new Vector2(1, 1));
        }

        public void Update(GameTime gameTime)
        {
            tileSelection.Update(gameTime);
            drawingArea.Update(gameTime);

            CheckForSavingTilemapToFile();
            CheckForLoadingTilemapFromFile();
            CheckForLoadingTileSelectionFromFile();

            if (InputManager.OnKeyPressed(Microsoft.Xna.Framework.Input.Keys.I))
            {
                drawInfoText = !drawInfoText;
            }
        }

        #region UpdateHelper
        private void CheckForSavingTilemapToFile()
        {
            // Namespace muss hier voll spezifiziert sein wegen WindowsForms ambiguity.
            // Vielleicht in ConfigFileUtility verschieben ?
            if (InputManager.OnKeyCombinationPressed(Microsoft.Xna.Framework.Input.Keys.LeftControl,
                                                     Microsoft.Xna.Framework.Input.Keys.S))
            {
                String fileName = String.Empty;
                bool saveFileDialogCanceled = false;

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "Save Tilemap as example.tm.txt";

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

        private void CheckForLoadingTilemapFromFile()
        {
            if (InputManager.OnKeyCombinationPressed(Microsoft.Xna.Framework.Input.Keys.LeftControl,
                                                     Microsoft.Xna.Framework.Input.Keys.L))
            {
                String fileName = String.Empty;
                bool openFileDialogCanceled = false;

                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Select Tilemap file(example.tm.txt) to load...";

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

        private void CheckForLoadingTileSelectionFromFile()
        {
            if (InputManager.OnKeyCombinationPressed(Microsoft.Xna.Framework.Input.Keys.LeftControl, 
                                                     Microsoft.Xna.Framework.Input.Keys.T))
            {
                String fileName = String.Empty;
                bool openFileDialogCanceled = false;

                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Select TileSelection file(example.ts.txt) to load...";

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
                    tileSelection.ReadTilesFromFile(fileName, content);
                    drawingArea.GridCellSize = (int)tileSelection.TileSize.X;
                }
            }
        }

        #endregion

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            drawingArea.Draw(gameTime, spriteBatch);

            if (!tileSelection.Hidden)
            {
                tileSelection.Draw(gameTime, spriteBatch);
            }

            if (drawInfoText)
            {
                Vector2 textPosition = viewport.Bounds.Size.ToVector2() * new Vector2(0.5f, 0.5f) -
                                       (fontSize * new Vector2(0.5f, 0.5f));

                infoText = "General\n" +
                           "MousePos: " + drawingArea.ZoomedMousePosition + "\n" +
                           "Hold MiddleMouseButton: Drag screen\n" +
                           "Hold LeftMouseButton: Rectangle selection\n" +
                           "MouseWheel: Zoom in/out\n" +
                           "Hold Arrow Keys: Move selection slowly\n" +
                           "Hotkeys\n" +
                           "-------\n" +
                           "STRG+S: Save\n" +
                           "STRG+L: Load\n" +
                           "STRG+T: Load TileSelection\n" +
                           "STRG+C: Copy Selection\n" +
                           "STRG+X: Cut Selection\n" +
                           "STRG+V: Paste Copy/Cut to Mouse Position\n" +
                           "STRG+G: Toggle Grid\n" +
                           "STRG+H: Show/Hide TileSelection\n" +
                           "Delete/Entf: Delete selected Tiles\n" +
                           "DrawingArea\n" +
                           "-----------\n" +
                           "NumTiles: " + drawingArea.Tiles.Count + "\n" +
                           "NumTilesSelection: " + drawingArea.NumTilesSelection + "\n" +
                           "NumTilesCopyBuffer: " + drawingArea.NumTilesCopyBuffer + "\n" +
                           "CurrentTileInfo: \n" + 
                           drawingArea.SelectionInfo;

                spriteBatch.Begin();

                spriteBatch.DrawString(font, infoText, textPosition, Color.Red);

                spriteBatch.End();
            }
        }

        public void LoadContent(ContentManager content, Viewport viewport)
        {
            this.content = content;

            this.viewport = viewport;

            infoText =     "General\n" +
                           "MousePos: \n" +
                           "Hold MiddleMouseButton: Drag screen\n" +
                           "Hold LeftMouseButton: Rectangle selection\n" +
                           "MouseWheel: Zoom in/out\n" +
                           "Hold Arrow Keys: Move selection slowly\n" +
                           "Hotkeys\n" +
                           "-------\n" +
                           "STRG+S: Save\n" +
                           "STRG+L: Load\n" +
                           "STRG+T: Load TileSelection\n" +
                           "STRG+C: Copy Selection\n" +
                           "STRG+X: Cut Selection\n" +
                           "STRG+V: Paste Copy/Cut to Mouse Position\n" +
                           "STRG+G: Toggle Grid\n" +
                           "STRG+H: Show/Hide TileSelection\n" +
                           "Delete/Entf: Delete selected Tiles\n" +
                           "DrawingArea\n" +
                           "-----------\n" +
                           "NumTiles: \n" +
                           "NumTilesSelection: \n" +
                           "NumTilesCopyBuffer: \n" +
                           "CurrentTileInfo: \n";

            font = content.Load<SpriteFont>("rsrc/fonts/DefaultFont");
            fontSize = font.MeasureString(infoText);

            drawingArea = new DrawingArea(viewport.Bounds, tileSelection);
            tileSelection.ReadTilesFromFile("Content/rsrc/tilesets/configFiles/tileSelections/ff6Tiles_InsideCastle.ts.txt", content);

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
            List<Rectangle> collisionBoxes = new List<Rectangle>();

            while ((line = reader.ReadLine()) != null)
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
                    }
                    else if (line.Contains("COLLISION_BOX"))
                    {
                        line = Utility.ReplaceWhitespace(reader.ReadLine(), ""); // Remove Whitespace
                        line = line.Remove(0, 17); // Remove 'COLLISION_BOUNDS='
                        collisionBoxes.Add(Utility.StringToRectangle(line));
                    }
                }
                else if (line.Contains("TILESET"))
                {
                    line = Utility.ReplaceWhitespace(line, "");
                    tileSetName = line.Substring(8); // Read everything after 'TILESET='
                }
            }

            drawingArea.Tiles = tiles;
            drawingArea.CollisionBoxes = collisionBoxes;

            // Figure out how to deal with new/old tileset.
            // Load Tileset if it has never been loaded and
            // just set it if it has been loaded before.
            // tileSelection.TileSet = tilese

            reader.Close();
        }
    }
}
