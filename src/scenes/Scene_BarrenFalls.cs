using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using EVCMonoGame.src.tilemap;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.states;
using EVCMonoGame.src.collision;
using EVCMonoGame.src.input;

namespace EVCMonoGame.src.scenes
{
    public class Scene_BarrenFalls : Scene
    {
        private Credits credits;

        public Scene_BarrenFalls(SceneManager sceneManager)
            : base(sceneManager)
        {

        }

        public override void OnEnterScene()
        {
            base.OnEnterScene();

            tilemap = new Tilemap(Vector2.Zero, "Content/rsrc/tilesets/configFiles/tilemaps/other/barrenFalls.tm.txt");

            sora.WorldPosition = new Vector2(1677, 2142);
            riku.WorldPosition = new Vector2(2080, 1990);

            camera.SetCameraToPosition(Vector2.Zero, Screenpoint.UP_LEFT_EDGE);
            camera.Zoom = 0.5f;

            credits = new Credits(new Vector2(1100, -1300), sceneManager.GraphicsDevice.Viewport, 100, sceneManager, 60,
                      new string[]
            {
                "Programmers",
                "      Manuel Rottmann",
                "      Dennis Frac",
                "      Moritz Schaefer",

                "Libraries",
                "      MonoGame.Primitives2D",
                "      Robert Penner's Easing Functions",

                "Assets",
                "      Final Fantasy 6 Tilesets",
                "      Kingdom Hearts - Chain of Memories(GBA) Spritesheets",
				"	   Chests Sprites by hiddenone, Kadokawa, & Avery",
			});

            updateables.AddRange(new IUpdateable[]
            {
                credits,
            });

            drawables.AddRange(new IDrawable[]
            {
                credits,
            });
        }
    }
}
