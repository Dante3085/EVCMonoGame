using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCMonoGame.src
{
	public static class DebugOptions
	{

		// Menu
		public static bool SkipMenu = true;
		public static bool ExitGameOnEscapeKey = true;
		public static bool StartWithLevelEditor = false;


		// Draw
		public static bool ShowCollision = true;
		public static bool ShowNavgrid = true;
		public static bool ShowPathfinding = true;
		public static bool ShowRaycasts = true;
	}
}
