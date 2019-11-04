
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using C3.MonoGame;

using EVCMonoGame.src.input;

namespace EVCMonoGame.src.tilemap.tilemapEditor
{
    // TODO: Maybe make DrawingArea Mouse Tile hovering more performant by using Spacial-Partitioning(Grid, BSPTree).
    // TODO: Besides hoveredTile, create the notion of currentTile in DrawingArea(Red Marker).
    // TODO: Display relevant infomation of currentTile in DrawingArea.
    // TODO: Display number of Tiles in DrawingArea.
    // TODO: Rectangle selection of many Tiles and move them around as a unit.
    // TODO: Make lining up Tiles when moving a Tile easy.

    // TODO: Make it possible to scale the currentTile of DrawingArea by placing Mouse at bottom right corner, 
    //       pressing LeftMouseButton and moving the Mouse around.

    // TODO: Make moving of DrawingArea possible so that a DrawingArea bigger than the Screen is still accessible.
    //       Make it impossible that the whole DrawingArea goes outside the screen.
    //       An alternative is using the Camera class, but that maybe presents other problems.

    // TODO: When moving a Tile in the DrawingArea, only move the Tile when the Mouse has already traveled a certain
    //       distance in one direction. That makes it possible to place the Tiles more precisely, like on a Grid.

    public class DrawingArea
    {
        private Rectangle bounds;
        private List<Tile> tiles = new List<Tile>();
        private Vector2 currentMousePosition = Vector2.Zero;
        private bool drawTileSelectionCurrentTileOnMouse = false;
        private Rectangle destinationRectangle = Rectangle.Empty;
        private Tile tileHoveredByMouse = null;
        private Rectangle tileHoveredByMouseMarker = Rectangle.Empty;
        private TileSelection tileSelection;

        private bool isAnyTileHoveredByMouse = false;
        private bool moveTileHoveredByMouse = false;

        private Vector2 mouseTravel = Vector2.Zero;

        private SpriteFont font;
        private String currentTileInfo = String.Empty;

        private float zoom = 1;

        public DrawingArea(Rectangle bounds, TileSelection tileSelection)
        {
            this.bounds = bounds;
            this.tileSelection = tileSelection;
        }

        public void Update(GameTime gameTime)
        {
            currentMousePosition = InputManager.CurrentMousePosition();

            bool tileSelectionHasCurrentTile = tileSelection.CurrentTile != null;
            bool tileSelectionIsHoveredByMouse = tileSelection.IsHoveredByMouse;

            if (bounds.Contains(currentMousePosition))
            {
                // Check for drawing the currentTile of the TileSelection on the Mouse and
                // placing the currentTile of TileSelection in the DrawingArea.
                if (tileSelectionHasCurrentTile)
                {
                    if (!tileSelectionIsHoveredByMouse)
                    {
                        drawTileSelectionCurrentTileOnMouse = true;

                        if (InputManager.OnLeftMouseButtonClicked())
                        {
                            Tile tsCurrentTile = tileSelection.CurrentTile;
                            Tile newTile = new Tile(tsCurrentTile.name, tsCurrentTile.textureBounds,
                            new Rectangle(currentMousePosition.ToPoint(), tsCurrentTile.screenBounds.Size));

                            tiles.Add(newTile);
                        }


                    }
                    else
                    {
                        drawTileSelectionCurrentTileOnMouse = false;
                    }
                }

                if (!tileSelectionIsHoveredByMouse &&
                    !drawTileSelectionCurrentTileOnMouse &&
                    InputManager.HasMouseMoved)
                {
                    if (!moveTileHoveredByMouse)
                    {
                        // Check for Mouse-Hover on one of the already placed Tiles in the DrawingArea and
                        // and detecting if the hovered Tile needs to be moved around.
                        foreach (Tile tile in tiles)
                        {
                            isAnyTileHoveredByMouse = false;
                            tileHoveredByMouse = null;

                            if (tile.screenBounds.Contains(currentMousePosition))
                            {
                                isAnyTileHoveredByMouse = true;
                                tileHoveredByMouseMarker = tile.screenBounds;
                                tileHoveredByMouse = tile;

                                // Das ist nicht das currentTile. Nur zum Test.
                                currentTileInfo = tileHoveredByMouse.ToString();

                                break;
                            }
                        }
                    }

                    if (!isAnyTileHoveredByMouse)
                    {
                        tileHoveredByMouseMarker = Rectangle.Empty;
                    }

                    if (moveTileHoveredByMouse)
                    {
                        mouseTravel = currentMousePosition - InputManager.PreviousMousePosition();

                        //if (mouseTravel.Length() > 50)
                        //{
                        //    tileHoveredByMouse.screenBounds.Location += (mouseTravel).ToPoint();
                        //    mouseTravel = Vector2.Zero;
                        //}

                        tileHoveredByMouse.screenBounds.Location += mouseTravel.ToPoint();
                    }
                }

                if (moveTileHoveredByMouse && InputManager.OnLeftMouseButtonReleased())
                {
                    moveTileHoveredByMouse = false;
                    tileHoveredByMouseMarker = tileHoveredByMouse.screenBounds;
                }

                if (isAnyTileHoveredByMouse && InputManager.OnLeftMouseButtonClicked())
                {
                    moveTileHoveredByMouse = true;
                    tileHoveredByMouseMarker = Rectangle.Empty;
                }
            }
            else
            {
                drawTileSelectionCurrentTileOnMouse = false;
            }
        }
        private Matrix getZoom()
        {
            if (InputManager.CurrentScrollWheel() != InputManager.PreviousScrollWheel())
            {
                Console.WriteLine(InputManager.CurrentScrollWheel());
            }
            if (InputManager.CurrentScrollWheel() < InputManager.PreviousScrollWheel() && zoom > 0.001f)
            {
                zoom -= 0.01f + (0.04f * zoom);
            }
            else if (InputManager.CurrentScrollWheel() > InputManager.PreviousScrollWheel())
            {
                zoom += 0.01f+(0.1f*zoom);
            }
            Vector2 point = (-zoom) * new Vector2(bounds.Width * 0.5f, bounds.Height * 0.5f) + new Vector2(bounds.Width * 0.5f, bounds.Height * 0.5f);
            return new Matrix(
                     new Vector4(zoom, 0, 0, 0),
                     new Vector4(0, zoom, 0, 0),
                     new Vector4(0, 0, 1, 0),
                     new Vector4(point.X, point.Y, 0, 1));
            //return Matrix.Identity;
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: getZoom());

            Primitives2D.DrawRectangle(spriteBatch, bounds, Color.DarkRed);

            foreach (Tile tile in tiles)
            {
                destinationRectangle.Location = tile.screenBounds.Location;
                destinationRectangle.Size = tile.screenBounds.Size;

                spriteBatch.Draw(tileSelection.TileSet, destinationRectangle, tile.textureBounds, Color.White);
            }

            Primitives2D.DrawRectangle(spriteBatch, tileHoveredByMouseMarker, Color.AliceBlue, 5);

            if (drawTileSelectionCurrentTileOnMouse)
            {
                spriteBatch.Draw(tileSelection.TileSet,
                new Rectangle(currentMousePosition.ToPoint(), tileSelection.CurrentTile.screenBounds.Size),
                tileSelection.CurrentTile.textureBounds, Color.White);
            }

            // Draw currentTileInfo
            if (tileHoveredByMouse != null)
            {
                spriteBatch.DrawString(font, currentTileInfo, new Vector2((bounds.X + bounds.Width) * 0.25f, bounds.Top), Color.White);
            }

            spriteBatch.End();
        }

        public void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("rsrc/fonts/DefaultFont");
        }
    }
}
