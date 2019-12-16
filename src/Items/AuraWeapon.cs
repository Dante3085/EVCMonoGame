using System;
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
    public class AuraWeapon : Weapon
    {
        public int   strength;
        public int   defense;
        public int   speed;
        public float knockbackMultiplier;

        private EAura aura;

        public AuraWeapon
            (
                Vector2 position, 
                String inventoryIconPath, 
                String anmConfigFile, 
                String idleAnim, 
                EAura aura,
                int strength = 0,
                int defense = 0,
                int speed = 0,
                float knockbackMultiplier = 1
            )
            : base
            (
                position,
                inventoryIconPath,
                anmConfigFile,
                idleAnim,
                GameplayState.Lane.LaneOne
            )
        {
            this.aura = aura;

            this.strength = strength;
            this.defense = defense;
            this.speed = speed;

            if (knockbackMultiplier <= 0)
            {
                this.knockbackMultiplier = 1;
            }
            else
            {
                this.knockbackMultiplier = Math.Abs(knockbackMultiplier);
            }
        }

        public override Item Copy()
        {
            AuraWeapon auroWeapon = new AuraWeapon(WorldPosition, inventoryIconPath, anmConfigFile, idleAnim, aura);
            auroWeapon.strength = strength;
            auroWeapon.defense = defense;
            auroWeapon.speed = speed;
            auroWeapon.knockbackMultiplier = knockbackMultiplier;

            return auroWeapon;
        }

        public override void ActivateSpecial(Player player, GameTime gameTime)
        {
            base.ActivateSpecial(player, gameTime);

            ((PlayerOne)player).weapon = this;
            ((PlayerOne)player).SetGlow(aura);
        }
    }
}
