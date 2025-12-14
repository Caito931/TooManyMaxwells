using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TooManyMaxwells;

public class Assets
{
    // Font
    public static SpriteFont font;

    // List Texture2d
    public static List<Texture2D> textures = new List<Texture2D>();
    public static Texture2D background;
    public static Texture2D walter;
    public static Texture2D maxwellAtlas;
    public static Texture2D ak47;
    public static Texture2D bullet;
    public static Texture2D muzzleFlash;
    
    // List SoundEffect
    public static List<SoundEffect> sounds = new List<SoundEffect>();
    public static SoundEffect shoot;
    public static SoundEffect reload;

    public static void Load(ContentManager Content, GraphicsDevice graphicsDevice)
    {
        font = Content.Load<SpriteFont>("Roboto");

        background = Content.Load<Texture2D>("background");
        walter = Content.Load<Texture2D>("walter");
        maxwellAtlas = Content.Load<Texture2D>("maxwellAtlas");
        ak47 = Content.Load<Texture2D>("ak47");
        bullet = Content.Load<Texture2D>("bullet");
        muzzleFlash = Content.Load<Texture2D>("muzzleFlash");

        textures.Add(background);
        textures.Add(walter);
        textures.Add(maxwellAtlas);
        textures.Add(ak47);
        textures.Add(bullet);
        textures.Add(muzzleFlash);

        shoot = Content.Load<SoundEffect>("shoot");
        reload = Content.Load<SoundEffect>("reload");

        sounds.Add(shoot);
        sounds.Add(reload);
    }

    public static void Dispose()
    {
        foreach (Texture2D t in textures)
        {
            t.Dispose();
        }
        foreach (SoundEffect se in sounds)
        {
            se.Dispose();
        }
    }
}
