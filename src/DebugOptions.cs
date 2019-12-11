using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

using EVCMonoGame.src.input;

namespace EVCMonoGame.src
{
	public static class DebugOptions
	{

		// Menu
		public static bool skipMenu             = true;
		public static bool exitGameOnEscapeKey  = true;
		public static bool startWithLevelEditor = false;
		
		//	Collision
		public static bool showCollision		= true;
		public static bool showInteractable		= true;
		public static bool showHurtBounds		= false;

		//	Pathfind
		public static bool showNavgrid			= true;
		public static bool showPathfinding		= true;
		public static bool showRaycasts			= true;

		//	Attack
		public static bool showAttackRange		= false;
		public static bool showAttackBounds		= false;

		// Etc.
		public static bool showFpsCounter		= true;
		public static bool showItemFinder		= true;

		//Gameplay
		public static bool spawnWithStarterItems = true;

        public static void Update()
        {
            if (InputManager.OnKeyPressed(Keys.F1))
            {
                showFpsCounter = !showFpsCounter;
            }

            if (InputManager.OnKeyPressed(Keys.F2))
            {
                showCollision       = !showCollision;
                showNavgrid         = !showNavgrid;
                showPathfinding     = !showPathfinding;
                showRaycasts        = !showRaycasts;
                showAttackRange     = !showAttackRange;
            }

            if (InputManager.OnKeyPressed(Keys.F3))
            {
                showAttackBounds = !showAttackBounds;
            }

            if (InputManager.OnKeyPressed(Keys.F4))
            {
                showHurtBounds = !showHurtBounds;
            }

            if (InputManager.OnKeyPressed(Keys.F5))
            {
                showItemFinder = !showItemFinder;
            }
        }
	}
}
