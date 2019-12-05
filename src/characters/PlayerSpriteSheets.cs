using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using EVCMonoGame.src.gui;
using EVCMonoGame.src.scenes;
using EVCMonoGame.src.animation;
using EVCMonoGame.src.collision;

namespace EVCMonoGame.src.characters
{
    public static class PlayerSpriteSheets
    {
        private static bool loaded = false;
        private static List<Texture2D> amulets = new List<Texture2D>();
        public static Texture2D NoGlow
        {
            get
            {
                if (!loaded) return null;
                return amulets[0];
            }
        }
        public static Texture2D RedGlow {
            get {
                if (!loaded) return null;
                return amulets[1];
            }
        }
        public static Texture2D GreenGlow
        {
            get
            {
                if (!loaded) return null;
                return amulets[2];
            }
        }
        public static Texture2D BlueGlow
        {
            get
            {
                if (!loaded) return null;
                return amulets[3];
            }
        }
        public static Texture2D YellowGlow
        {
            get
            {
                if (!loaded) return null;
                return amulets[4];
            }
        }
        public static Texture2D WhiteGlow
        {
            get
            {
                if (!loaded) return null;
                return amulets[5];
            }
        }
        public static Texture2D GodModeGlow
        {
            get
            {
                if (!loaded) return null;
                return amulets[6];
            }
        }
        public static void Load(ContentManager content) {
            loaded = true;
            amulets.Add(content.Load<Texture2D>("rsrc/spritesheets/khcom_sora_transparent"));
            amulets.Add(content.Load<Texture2D>("rsrc/spritesheets/khcom_sora_transparent_red"));
            amulets.Add(content.Load<Texture2D>("rsrc/spritesheets/khcom_sora_transparent_green"));
            amulets.Add(content.Load<Texture2D>("rsrc/spritesheets/khcom_sora_transparent_blue"));
            amulets.Add(content.Load<Texture2D>("rsrc/spritesheets/khcom_sora_transparent_yellow"));
            amulets.Add(content.Load<Texture2D>("rsrc/spritesheets/khcom_sora_transparent_white"));
            amulets.Add(content.Load<Texture2D>("rsrc/spritesheets/khcom_sora_transparent_god_mode"));
        }
    }
}
