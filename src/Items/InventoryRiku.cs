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
            GodImperator,
            SplitMissle,
            PenetrateMissle,
            BounceMissle
		}

		public InventoryRiku(Player riku) : base(riku)
		{
		}

		public override void StarterItems()
		{
			base.StarterItems();

			WeaponRiku weapon_2 = new GodImperatorMissle(Vector2.Zero);
			WeaponRiku weapon_3 = new SplitMissle(Vector2.Zero);
            WeaponRiku weapon = new PenetrateMissle(Vector2.Zero);
            WeaponRiku weapon_4 = new BounceMissle(Vector2.Zero);

            weapon.stack = 0;
            weapon_2.stack = 0;
            weapon_3.stack = 0;
            weapon_4.stack = 0;

            CollisionManager.RemoveCollidable(weapon, CollisionManager.itemCollisionChannel);
			CollisionManager.RemoveCollidable(weapon_2, CollisionManager.itemCollisionChannel);
			CollisionManager.RemoveCollidable(weapon_3, CollisionManager.itemCollisionChannel);
            CollisionManager.RemoveCollidable(weapon_4, CollisionManager.itemCollisionChannel);

            AddWeapon(weapon);
			AddWeapon(weapon_2);
			AddWeapon(weapon_3);
            AddWeapon(weapon_4);
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
                    
                    //Draw Stack Ammount
                    spriteBatch.DrawString(font, weapon.stack.ToString(), screenPosition - itemPosition.ToVector2() + itemSize.ToVector2() + usableItemAmmountDrawOffset, Color.White * 1.0f);

                }

                itemPositionCounter++;
			}
		}

		public void ActivateSpecialAttack(GameTime gameTime, Ability ability, double cooldown = 0)
		{
			activeWeapon = weapons.ElementAt<Weapon>((int)ability);
			if (((WeaponRiku)activeWeapon).Unlocked)
			{
				base.ActivateSpecialAttack(gameTime);
			}
		}

        public WeaponRiku GetWeapon(Ability ability)
        {
            return (WeaponRiku)weapons.ElementAt<Weapon>((int)ability);
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

        public bool IsAbilityOnStock(Ability ability)
        {
            return GetWeapon(ability).stack > 0 && GetWeapon(ability).Unlocked ? true : false;
        }
	}
}
