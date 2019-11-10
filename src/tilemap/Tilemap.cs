
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using EVCMonoGame.src.utility;

namespace EVCMonoGame.src.tilemap
{
    // TODO: Maybe rethink Tilemap to work better with TilemapEditor.
    // TODO: Make Tilemap able to load the 'tilemap.ts.txt' FileFormat.

    public class Tilemap : scenes.IDrawable
    {
        private List<Tile> tiles;
        private Vector2 position;

        public Tilemap(Vector2 position, String tilemapFile)
        {
            this.position = position;

        }
    }
}
