﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C3.MonoGame;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.collision;
using EVCMonoGame.src.scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EVCMonoGame.src.Items
{
	public class UsableItem : InventoryItem
	{
		public int stack = 1;
		public String itemName = "";
		public bool stackable = true;

		public UsableItem(Vector2 position, String inventoryIconPath, String itemName = "") : base(position, inventoryIconPath)
		{
			this.itemName = itemName;
		}

		public override void PickUp(Player player)
		{
			base.PickUp(player);

			player.PlayerInventory.AddUsableItem(this);
		}

		public virtual void Use(Player player)
		{
			stack--;
		}
	}
}
