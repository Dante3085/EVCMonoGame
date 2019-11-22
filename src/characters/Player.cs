using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

using EVCMonoGame.src.scenes;

namespace EVCMonoGame.src.characters
{
    public abstract class Player : Character, scenes.IDrawable
    {

        public Player
            (
                String name, 
                int maxHp, 
                int currentHp,
                int maxMp,
                int currentMp,
                int strength,
                int defense,
                int intelligence,
                int agility,
                int movementSpeed,
                Vector2 position
            )
            : base
            (
                  name: name,
                  maxHp: maxHp,
                  currentHp: currentHp,
                  maxMp: maxMp,
                  currentMp: currentMp,
                  strength: strength,
                  defense: defense,
                  intelligence: intelligence,
                  agility: agility,
                  movementSpeed: movementSpeed,
                  position: position
            )
        {

        }


    }
}
