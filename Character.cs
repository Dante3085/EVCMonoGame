using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVCMonoGame
{
    public class Character
    {
        String name;

        private int strength;
        private int defense;
        private int intelligence;
        private int agility;

        private int maxHp;
        private int currentHp;

        private int maxMp;
        private int currentMp;

        public Character(String name, int strength = 0, int defense = 0, int intelligence = 0, int agility = 0,
                         int maxHp = 0, int currentHp = 0, int maxMp = 0, int currentMp = 0)
        {
            this.name = name;

            this.strength = strength;
            this.defense = defense;
            this.intelligence = intelligence;
            this.agility = agility;

            this.maxHp = maxHp;
            this.currentHp = currentHp > maxHp ? maxHp : currentHp;

            this.maxMp = maxMp;
            this.currentMp = currentMp > maxMp ? maxMp : currentMp;
        }
    }
}
