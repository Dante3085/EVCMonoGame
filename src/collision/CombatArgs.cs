using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using EVCMonoGame.src.characters;

namespace EVCMonoGame.src.collision
{
    public class CombatArgs
    {
        private static long uniqueId = 0;
        public long id;
        public CombatCollidable attacker;
        public CombatCollidable victim;
        public CombatantType targetType;

        public int damage        = 0;
        public Vector2 knockBack = Vector2.Zero;
        public bool causesFlinch = false;

        public CombatArgs(CombatCollidable attacker, CombatCollidable victim, CombatantType targetType)
        {
            this.attacker = attacker;
            this.victim = victim;
            this.targetType = targetType;
            this.id = uniqueId++;
        }

        public CombatArgs(CombatArgs combatArgs)
        {
            this.attacker = combatArgs.attacker;
            this.victim = combatArgs.victim;
            this.damage = combatArgs.damage;
            this.knockBack = combatArgs.knockBack;
            this.causesFlinch = combatArgs.causesFlinch;
            this.id = uniqueId++;

            Console.WriteLine("KonstruktorId: " + id);
        }

        public void NewId()
        {
            this.id = uniqueId++;
        }
    }
}
