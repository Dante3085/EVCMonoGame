﻿
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
    // TODO: Fix moving Tiles around in DrawingArea.
    // TODO: Maybe make DrawingArea Mouse Tile hovering more performant by using Spacial-Partitioning(Grid, BSPTree).
    // TODO: Besides hoveredTile, create the notion of currentTile in DrawingArea(Red Marker).
    // TODO: Display relevant infomation of currentTile in DrawingArea.
    // TODO: Display number of Tiles in DrawingArea.
    // TODO: Rectangle selection of many Tiles and move them around as a unit.

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
        private List<Tile> tiles                         = new List<Tile>();
        private Vector2 currentMousePosition             = Vector2.Zero;
        private bool drawTileSelectionCurrentTileOnMouse = false;
        private Rectangle destinationRectangle           = Rectangle.Empty;
        private Tile tileHoveredByMouse                  = null;
        private Rectangle tileHoveredByMouseMarker       = Rectangle.Empty;
        private TileSelection tileSelection;

        private bool isAnyTileHoveredByMouse             = false;
        private bool moveTileHoveredByMouse              = false;

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
                // ---- Bis hier alles korrekt.

                if (!tileSelectionIsHoveredByMouse && InputManager.HasMouseMoved)
                {
                    if (!moveTileHoveredByMouse)
                    {
                        // Check for Mouse-Hover on one of the already placed Tiles in the DrawingArea and
                        // and detecting if the hovered Tile needs to be moved around.
                        foreach (Tile tile in tiles)
                        {
                            isAnyTileHoveredByMouse = false;

                            if (tile.screenBounds.Contains(currentMousePosition))
                            {
                                isAnyTileHoveredByMouse = true;
                                tileHoveredByMouseMarker = tile.screenBounds;
                                tileHoveredByMouse = tile;

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
                        tileHoveredByMouse.screenBounds.Location +=
                                (currentMousePosition - InputManager.PreviousMousePosition()).ToPoint();
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

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

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

            spriteBatch.End();
        }

        public void LoadContent(ContentManager content)
        {

        }
    }
}