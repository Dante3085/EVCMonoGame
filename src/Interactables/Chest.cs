using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using C3.MonoGame;

using EVCMonoGame.src.scenes;
using EVCMonoGame.src.states;
using EVCMonoGame.src.input;
using EVCMonoGame.src.collision;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.Traps;
using EVCMonoGame.src.Items;

namespace EVCMonoGame.src
{
    public class Chest : Lever, scenes.IUpdateable
	{

		private Vector2 worldPosition;
		private List<Item> items;

		private GameplayState.Lane lane;

		private double pickUpCooldown = 1500;

		private bool interactOnced = false;


		public Chest(Vector2 worldPosition, List<Item> items, GameplayState.Lane lane) : base(worldPosition)
		{
			this.worldPosition = worldPosition;
			this.items = items;
			this.lane = lane;

			deactivatedTextureRec = new Rectangle(0, 196, 48, 47);
			activatedTextureRec = new Rectangle(0, 337, 48, 47);

			foreach (Item item in items)
				item.isInShop = true;
		}
		
		public override void Interact(Player player)
		{
			if (lane == player.lane && !Activated)
			{
				base.Interact(player);

				Random random = new Random();

				foreach (Item item in items)
				{
					Vector2 rndPos = worldPosition + new Vector2(random.Next(250), random.Next(250));
					item.WorldPosition = rndPos;
					Scene.drawablesToAdd.Add(item);
					Scene.updateablesToAdd.Add(item);
				}
			}
				
		}

		public override void LoadContent(ContentManager content)
		{
			texture = content.Load<Texture2D>("rsrc/spritesheets/chests");
		}

		public void Update(GameTime gameTime)
		{
			if (Activated && !interactOnced)
			{
				if (pickUpCooldown > 0)
					pickUpCooldown -= gameTime.ElapsedGameTime.TotalMilliseconds;
				else
					foreach(Item item in  items)
						item.isInShop = false;
			}
		}
	}
}
