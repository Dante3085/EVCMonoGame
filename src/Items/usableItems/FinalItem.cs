using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

using EVCMonoGame.src.states;
using EVCMonoGame.src.characters;

namespace EVCMonoGame.src.Items.usableItems
{
    public class FinalItem : UsableItem
    {

        public FinalItem(Vector2 position, GameplayState.Lane lane)
            : base
            (
                  position,
                  "rsrc/spritesheets/singleImages/ff6FinalItem",
                  "Content/rsrc/spritesheets/configFiles/finalItem.anm.txt",
                  "BLUE_BUBBLES_IDLE",
                  lane,
                  "Final Item"
            )
        {
            sprite.Scale = 6;
        }

        public override Item Copy()
        {
            return new FinalItem(WorldPosition, lane);
        }

        public override void Use(Player player)
        {
            base.Use(player);

            player.CurrentHp = player.MaxHp = 9999;
        }
    }
}
