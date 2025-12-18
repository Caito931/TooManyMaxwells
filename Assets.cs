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

    public static Texture2D hkg36;
    public static Texture2D scar;
    public static Texture2D mp5;
    public static Texture2D vector;
    public static Texture2D aa12;
    public static Texture2D shotgun;
    public static Texture2D awp;
    public static Texture2D barrett;
   
    public static Texture2D bullet;

    public static Texture2D casing;
    public static Texture2D ball;
    public static Texture2D shell;

    public static Texture2D muzzleFlash;
    public static Texture2D doge;

    public static Texture2D rifleAmmoIcon;
    public static Texture2D smgAmmoIcon;
    public static Texture2D shotgunShellIcon;

    // List SoundEffect
    public static List<SoundEffect> sounds = new List<SoundEffect>();
    public static SoundEffect Hkg36;
    public static SoundEffect Scar;
    public static SoundEffect Mp5;
    public static SoundEffect Vector;
    public static SoundEffect Aa12;
    public static SoundEffect Shotgun;
    public static SoundEffect Awp;
    public static SoundEffect Barrett;
    public static SoundEffect reload;

    // White Pixel
    public static Texture2D whitePixel;

    public static void Load(ContentManager Content, GraphicsDevice graphicsDevice)
    {
        font = Content.Load<SpriteFont>("Roboto");

        background = Content.Load<Texture2D>("background");

        walter = Content.Load<Texture2D>("walter");
        maxwellAtlas = Content.Load<Texture2D>("maxwellAtlas");

        hkg36 = Content.Load<Texture2D>("hkg36");
        scar = Content.Load<Texture2D>("scar");
        mp5 = Content.Load<Texture2D>("mp5");
        vector = Content.Load<Texture2D>("vector");
        aa12 = Content.Load<Texture2D>("aa12");
        shotgun = Content.Load<Texture2D>("shotgun");
        awp = Content.Load<Texture2D>("awp");
        barrett = Content.Load<Texture2D>("barrett");

        bullet = Content.Load<Texture2D>("bullet");
        casing = Content.Load<Texture2D>("casing");

        ball = Content.Load<Texture2D>("ball");
        shell = Content.Load<Texture2D>("shell");

        muzzleFlash = Content.Load<Texture2D>("muzzleFlash");
        doge = Content.Load<Texture2D>("doge");

        rifleAmmoIcon = Content.Load<Texture2D>("rifleAmmoIcon");
        smgAmmoIcon = Content.Load<Texture2D>("smgAmmoIcon");
        shotgunShellIcon = Content.Load<Texture2D>("shotgunAmmoIcon");

        textures.AddRange(
            new List<Texture2D>(){
                background, walter, maxwellAtlas, 
                hkg36, scar, mp5, vector, aa12, shotgun, awp, barrett,
                bullet, casing, 
                ball, shell, 
                muzzleFlash, 
                doge,
                rifleAmmoIcon, smgAmmoIcon, shotgunShellIcon
            }
        );
        
        Hkg36 = Content.Load<SoundEffect>("Hkg36_S");
        Scar = Content.Load<SoundEffect>("Scar_S");
        Mp5 = Content.Load<SoundEffect>("Mp5_S");
        Vector = Content.Load<SoundEffect>("Vector_S");
        Aa12 = Content.Load<SoundEffect>("Aa12_S");
        Shotgun = Content.Load<SoundEffect>("Shotgun_S");
        Awp = Content.Load<SoundEffect>("Awp_S");
        Barrett = Content.Load<SoundEffect>("Barrett_S");
        reload = Content.Load<SoundEffect>("reload");

        sounds.AddRange(
            new List<SoundEffect>()
            {
                Hkg36, Scar, Mp5, Vector, Aa12, Shotgun, Awp, Barrett,
                reload
            }
        );

        whitePixel = new Texture2D(graphicsDevice, 1, 1);
        whitePixel.SetData([Color.White]);
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
