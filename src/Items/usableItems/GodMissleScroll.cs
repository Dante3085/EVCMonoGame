using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C3.MonoGame;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.collision;
using EVCMonoGame.src.projectiles;
using EVCMonoGame.src.scenes;
using EVCMonoGame.src.states;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EVCMonoGame.src.Items
{
	public class GodMissleScroll : UsableItem
	{

		public GodMissleScroll(Vector2 position)
			: base
			(
				  position,
				  "rsrc/spr" +
				  "" +
				  "itesheets/singleImages/arrow",
				  "Content/rsrc/spritesheets/configFiles/coin.anm.txt",
				  "COIN",
				  GameplayState.Lane.LaneTwo,
				  "GodMissleScroll"
			)
		{
			//stackable = false;
			shopPrice = 50;
		}

		public override Item Copy()
		{
			return new GodMissleScroll(WorldPosition);
		}

		public override void PickUp(Player player)
		{
			base.PickUp(player);
		}

		public override void Use(Player player)
		{
			base.Use(player);

			Player riku = player;
			
			CombatArgs combatArgs = riku.CombatArgs;
			combatArgs.NewId();
			MagicMissileGodImperator missile = new MagicMissileGodImperator(Vector2.Zero, Orientation.DOWN);
			switch (riku.playerOrientation)
			{
				case Orientation.LEFT:
					combatArgs.knockBack = new Vector2(-10, 0);
					missile = new MagicMissileGodImperator(riku.CollisionBox.Location.ToVector2() +
						new Vector2(-(missile.CollisionBox.Width + 1), riku.CollisionBox.Height / 2 - missile.CollisionBox.Height / 2), Orientation.LEFT);
					break;

				case Orientation.UP_LEFT:
					combatArgs.knockBack = new Vector2(-10, -10);
					missile = new MagicMissileGodImperator(riku.WorldPosition +
						new Vector2(-(missile.CollisionBox.Width + 1), -(missile.CollisionBox.Height + 1)), Orientation.UP_LEFT, 10);
					break;

				case Orientation.UP:
					combatArgs.knockBack = new Vector2(0, -200);
					missile = new MagicMissileGodImperator(riku.WorldPosition +
						new Vector2(riku.CollisionBox.Width / 2 - missile.CollisionBox.Width / 2, -(missile.CollisionBox.Height + 1)), Orientation.UP);
					break;

				case Orientation.UP_RIGHT:
					combatArgs.knockBack = new Vector2(10, -10);
					missile = new MagicMissileGodImperator(riku.WorldPosition +
						new Vector2(riku.CollisionBox.Width + 1, -(missile.CollisionBox.Height + 1)), Orientation.UP_RIGHT, 10);
					break;

				case Orientation.RIGHT:
					combatArgs.knockBack = new Vector2(10, 0);
					missile = new MagicMissileGodImperator(riku.WorldPosition +
						new Vector2(riku.CollisionBox.Width + 1, riku.CollisionBox.Height / 2 - missile.CollisionBox.Height / 2), Orientation.RIGHT);
					break;

				case Orientation.DOWN_RIGHT:
					combatArgs.knockBack = new Vector2(10, 10);
					missile = new MagicMissileGodImperator(riku.WorldPosition +
						new Vector2(riku.CollisionBox.Width + 1, riku.CollisionBox.Height + 1), Orientation.DOWN_RIGHT, 10);
					break;

				case Orientation.DOWN:
					combatArgs.knockBack = new Vector2(0, 10);
					missile = new MagicMissileGodImperator(riku.WorldPosition +
						new Vector2(riku.CollisionBox.Width / 2 - missile.CollisionBox.Width / 2, riku.CollisionBox.Height + 1), Orientation.DOWN);
					break;

				case Orientation.DOWN_LEFT:
					combatArgs.knockBack = new Vector2(-10, 10);
					missile = new MagicMissileGodImperator(riku.WorldPosition +
						new Vector2(-(missile.CollisionBox.Width + 1), riku.CollisionBox.Height + 1), Orientation.DOWN_LEFT, 10);
					break;
			}

			missile.LoadContent(MagicMissile.content);
			GameplayState.PlayerTwo.missiles.Add(missile);
		}
	}
}
