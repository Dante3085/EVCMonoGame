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
		public static bool exitGameOnEscapeKey  = false;
		public static bool startWithLevelEditor = false;
		
		//	Collision
		public static bool showCollision		= false;
		public static bool showInteractable		= false;
		public static bool showHurtBounds		= false;

		//	Pathfind
		public static bool showNavgrid			= false;
		public static bool showPathfinding		= false;
		public static bool showRaycasts			= false;

		//	Attack
		public static bool showAttackRange		= false;
		public static bool showAttackBounds		= false;

		// Etc.
		public static bool showFpsCounter		= false;
		public static bool showItemFinder		= false;

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
				showInteractable	= !showInteractable;
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
