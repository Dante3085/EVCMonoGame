using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C3.MonoGame;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.collision;
using EVCMonoGame.src.scenes;
using EVCMonoGame.src.states;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EVCMonoGame.src.Items
{
	public class Healthpotion : UsableItem
	{
		public int heals = 40;

		public Healthpotion(Vector2 position, String inventoryIconPath) 
			: base(
				  position,
				  inventoryIconPath,
				  GameplayState.Lane.LaneBoth,
				  "Healthpotion"
				  )
		{
			shopPrice = 5;
		}

		public override Item Copy()
		{
			return new Healthpotion(WorldPosition, inventoryIconPath);
		}

		public override void PickUp(Player player)
		{
			base.PickUp(player);
		}

		public override void Use(Player player)
		{
			base.Use(player);

			player.CurrentHp += heals;
		}
	}
}
