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
using EVCMonoGame.src.Traps;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EVCMonoGame.src.Items
{
	public class BearTrapItem : UsableItem
	{
		public int heals = 100;

		public BearTrapItem(Vector2 position) 
			: base(
				  position,
				  inventoryIconPath: "rsrc/spritesheets/singleImages/bear_trap",
				  anmConfigFile: "Content/rsrc/spritesheets/configFiles/bear_trap.anm.txt",
				  idleAnim: "COOLDOWN",
				  lane: GameplayState.Lane.LaneOne,
				  itemName: "BearTrap"
				  )
		{
			shopPrice = 20;
			sprite.Scale = 4;
		}

		public override Item Copy()
		{
			return new BearTrapItem(WorldPosition);
		}

		public override void PickUp(Player player)
		{
			base.PickUp(player);
		}

		public override void Use(Player player)
		{
			base.Use(player);

			BearTrap bearTrapItem = new BearTrap(player.WorldPosition + new Vector2(0, 100));
			bearTrapItem.LoadContent(GameplayState.globalContentManager);
			Scene.updateablesToAdd.Add(bearTrapItem);
			Scene.drawablesToAdd.Add(bearTrapItem);
		}
	}
}
