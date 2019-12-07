using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.collision;
using Microsoft.Xna.Framework;

namespace EVCMonoGame.src
{
	public interface Interactable
	{
		Rectangle InteractableBounds { get; set; }

		void Interact(Player player);
	}
}
