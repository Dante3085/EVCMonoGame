using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using EVCMonoGame.src.gui;
using EVCMonoGame.src.input;
using EVCMonoGame.src.scenes;
using EVCMonoGame.src.collision;
using C3.MonoGame;
using EVCMonoGame.src.Items;
using Microsoft.Xna.Framework;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.utility;

namespace EVCMonoGame.src.Items
{
	public class InventoryRiku : Inventory
	{
		public double AbilityCooldown; // in mili

		public enum Ability {
			PenetrateMissle,
			CoinBombMissle
		}

		public InventoryRiku(Player riku) : base(riku)
		{
		}

		public override void StarterItems()
		{
			// base.StarterItems();

			WeaponRiku weapon = new PenetrateMissle(new Vector2(1300, 3800));
			WeaponRiku weapon_2 = new CoinBombMissle(new Vector2(1350, 3820));
			WeaponRiku weapon_3 = new CoinBombMissle(new Vector2(1350, 3820));

			weapon.stack = 0;

			CollisionManager.RemoveCollidable(weapon, CollisionManager.itemCollisionChannel);
			CollisionManager.RemoveCollidable(weapon_2, CollisionManager.itemCollisionChannel);
			CollisionManager.RemoveCollidable(weapon_3, CollisionManager.itemCollisionChannel);

			AddWeapon(weapon);
			AddWeapon(weapon_2);
			AddWeapon(weapon_3);
		}

		public override void DrawWeaponsInventory(GameTime gameTime, SpriteBatch spriteBatch)
		{
			Point itemPosition;
			Texture2D icon;
			Vector2 weaponInventoryOffset = new Vector2(0, 100);

			int itemPositionCounter = 0;

			foreach (WeaponRiku weapon in weapons)
			{
				itemPosition = new Point(itemPositionCounter * itemSize.X + itemPositionCounter * itemSpacing, (int)weaponInventoryOffset.Y);

				// Draw Debug Inventory Grid
				Primitives2D.DrawRectangle(spriteBatch, screenPosition - itemPosition.ToVector2(), itemSize.ToVector2(), Color.White);

				if (weapon.Unlocked)
				{          
					//Draw Icon
					icon = weapons.ElementAt<Weapon>(itemPositionCounter).inventoryIcon;
					spriteBatch.Draw(icon, new Rectangle(new Point((int)screenPosition.X - itemPosition.X, (int)screenPosition.Y - itemPosition.Y), itemSize), Color.White * 1.0f);

				}

				itemPositionCounter++;
			}
		}

		public void ActivateSpecialAttack(GameTime gameTime, Ability ability, double cooldown)
		{
			activeWeapon = weapons.ElementAt<Weapon>((int)ability);
			if (((WeaponRiku)activeWeapon).Unlocked)
			{
				base.ActivateSpecialAttack(gameTime);
			}
		}

		public bool IsAbilityOnCooldown(Ability ability, GameTime gameTime)
		{
			return ((WeaponRiku)weapons.ElementAt<Weapon>((int)ability)).IsOnCooldown(gameTime);
		}

		public override void AddWeapon(Weapon weapon)
		{
			// Stack
			WeaponRiku weaponInStock = (WeaponRiku)weapons.FirstOrDefault(i => ((WeaponRiku)i).weaponName == ((WeaponRiku)weapon).weaponName);

			if (weaponInStock != null)
			{
				weaponInStock.stack += ((WeaponRiku)weapon).stack;
				if (weaponInStock.stack > 0)
					weaponInStock.Unlocked = true;
			}
			else
			{
				weapons.Add(weapon);
			}
		}


	}
}
