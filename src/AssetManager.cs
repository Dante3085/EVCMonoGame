using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

public enum ETexture
{
    SORA,
    RIKU,

    SORA_RED,
    SORA_GREEN,
    SORA_BLUE,
    SORA_YELLOW,
    SORA_WHITE,
    SORA_GOD_MODE,
}

public enum EFont
{
    DEFAULT,
}

public enum ESong
{
    CASTLEVANIA,
    WAY_TO_FALL,

    BEGINNING,
    DEFENDER_BROTHERS,
    REST_ROOM,
    TRAIN_STATION,
    SYNTH_TRAIN,
    ARABIAN_DESERT,
    CREEPY_CASTLE,
    BOSS,
}

public enum ESoundEffect
{
    GARGOYLE,
}

public static class AssetManager
{
    private static bool loaded = false;

    private static Dictionary<ETexture, Texture2D> textures = new Dictionary<ETexture, Texture2D>();
    private static Dictionary<EFont, SpriteFont> fonts = new Dictionary<EFont, SpriteFont>();
    private static Dictionary<ESong, Song> songs = new Dictionary<ESong, Song>();
    private static Dictionary<ESoundEffect, SoundEffect> sfx = new Dictionary<ESoundEffect, SoundEffect>();

    private static Dictionary<String, ETexture> textureNames = new Dictionary<string, ETexture>()
    {
        { "rsrc/spritesheets/khcom_riku_transparent", ETexture.RIKU },

        { "rsrc/spritesheets/khcom_sora_transparent", ETexture.SORA },
        { "rsrc/spritesheets/khcom_sora_transparent_red", ETexture.SORA_RED },
        { "rsrc/spritesheets/khcom_sora_transparent_green", ETexture.SORA_GREEN },
        { "rsrc/spritesheets/khcom_sora_transparent_blue", ETexture.SORA_BLUE },
        { "rsrc/spritesheets/khcom_sora_transparent_yellow", ETexture.SORA_YELLOW },
        { "rsrc/spritesheets/khcom_sora_transparent_white", ETexture.SORA_WHITE },
        { "rsrc/spritesheets/khcom_sora_transparent_god_mode", ETexture.SORA_GOD_MODE },
    };

    private static Dictionary<String, EFont> fontNames = new Dictionary<string, EFont>()
    {
        { "rsrc/fonts/DefaultFont", EFont.DEFAULT },
    };

    private static Dictionary<String, ESong> songNames = new Dictionary<string, ESong>()
    {
        { "rsrc/audio/music/Beginning", ESong.BEGINNING },
        { "rsrc/audio/music/DefenderBrothers", ESong.DEFENDER_BROTHERS },
        { "rsrc/audio/music/RestRoom", ESong.REST_ROOM },
        { "rsrc/audio/music/TrainStation", ESong.TRAIN_STATION },
        { "rsrc/audio/music/SynthTrain", ESong.SYNTH_TRAIN },
        { "rsrc/audio/music/ArabianDesert", ESong.ARABIAN_DESERT },
        { "rsrc/audio/music/CreepyCastle", ESong.CREEPY_CASTLE },
        { "rsrc/audio/music/Boss", ESong.BOSS },
    };

    private static Dictionary<String, ESoundEffect> sfxNames = new Dictionary<string, ESoundEffect>()
    {
        { "rsrc/audio/sfx/Gargoyle", ESoundEffect.GARGOYLE },
    };

    public static void LoadAssets(ContentManager contentManager)
    {
        if (loaded)
        {
            throw new InvalidOperationException("All assets have already been loaded. It does not make " +
                                                "senese to call this Method more than once.");
        }

        // Load Texture2Ds
        foreach (String textureName in textureNames.Keys)
        {
            textures.Add(textureNames[textureName], contentManager.Load<Texture2D>(textureName));
        }

        // Load SpriteFonts
        foreach (String fontName in fontNames.Keys)
        {
            fonts.Add(fontNames[fontName], contentManager.Load<SpriteFont>(fontName));
        }

        // Load Songs
        foreach (String songName in songNames.Keys)
        {
            songs.Add(songNames[songName], contentManager.Load<Song>(songName));
        }

        // Load SFX
        foreach (String sfxName in sfxNames.Keys)
        {
            sfx.Add(sfxNames[sfxName], contentManager.Load<SoundEffect>(sfxName));
        }

        loaded = true;
    }

    public static Texture2D GetTexture(ETexture texture)
    {
        return textures[texture];
    }

    public static SpriteFont GetFont(EFont font)
    {
        return fonts[font];
    }

    public static Song GetSong(ESong song)
    {
        return songs[song];
    }

    public static SoundEffect GetSoundEffect(ESoundEffect soundEffect)
    {
        return sfx[soundEffect];
    }
}

