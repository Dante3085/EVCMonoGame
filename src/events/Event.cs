using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCMonoGame.src.events
{
	public class Event
	{
		public delegate void Notify();

		public static event Notify notify;

		public static void NotifyAll()
		{
			if (notify != null)
			{
				notify();
			}
		}

		
	}
}
