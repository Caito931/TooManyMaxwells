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
    public static Texture2D background2;
    public static Texture2D walter;
    public static Texture2D maxwellAtlas;
    public static Texture2D ak47;
    public static Texture2D m4a1;
    public static Texture2D hkg36;
    public static Texture2D aa12;
    public static Texture2D barrett;
    public static Texture2D scar;
    public static Texture2D bullet;
    public static Texture2D casing;
    public static Texture2D ball;
    public static Texture2D shell;
    public static Texture2D muzzleFlash;
    public static Texture2D doge;
    public static Texture2D ammoIcon;
    public static Texture2D shellIcon;

    // List SoundEffect
    public static List<SoundEffect> sounds = new List<SoundEffect>();
    public static SoundEffect shoot;
    public static SoundEffect shoot2;
    public static SoundEffect shoot3;
    public static SoundEffect shoot4;
    public static SoundEffect shoot5;
    public static SoundEffect shoot6;
    public static SoundEffect reload;

    // White Pixel
    public static Texture2D whitePixel;

    public static void Load(ContentManager Content, GraphicsDevice graphicsDevice)
    {
        font = Content.Load<SpriteFont>("Roboto");

        background = Content.Load<Texture2D>("background");
        background2 = Content.Load<Texture2D>("background(2)");
        walter = Content.Load<Texture2D>("walter(2)");
        maxwellAtlas = Content.Load<Texture2D>("maxwellAtlas");
        ak47 = Content.Load<Texture2D>("ak47(2)");
        m4a1 = Content.Load<Texture2D>("m4a1");
        hkg36 = Content.Load<Texture2D>("hkg36");
        aa12 = Content.Load<Texture2D>("aa12");
        barrett = Content.Load<Texture2D>("barrett");
        scar = Content.Load<Texture2D>("scar");
        bullet = Content.Load<Texture2D>("bullet");
        casing = Content.Load<Texture2D>("casing");
        ball = Content.Load<Texture2D>("ball");
        shell = Content.Load<Texture2D>("shell");
        muzzleFlash = Content.Load<Texture2D>("muzzleFlash");
        doge = Content.Load<Texture2D>("doge");
        ammoIcon = Content.Load<Texture2D>("ammoIcon");
        shellIcon = Content.Load<Texture2D>("shellIcon");

        textures.AddRange(
            new List<Texture2D>(){
                background, background2, walter, maxwellAtlas, 
                ak47, m4a1, hkg36, aa12, barrett, scar, 
                bullet, casing, 
                ball, shell, 
                muzzleFlash, 
                doge,
                ammoIcon, shellIcon
            }
        );
        
        shoot = Content.Load<SoundEffect>("shoot");
        shoot2 = Content.Load<SoundEffect>("shoot2");
        shoot3 = Content.Load<SoundEffect>("shoot3");
        shoot4 = Content.Load<SoundEffect>("shoot4");
        shoot5 = Content.Load<SoundEffect>("shoot5");
        shoot6 = Content.Load<SoundEffect>("shoot6");
        reload = Content.Load<SoundEffect>("reload");

        sounds.AddRange(
            new List<SoundEffect>()
            {
                shoot, shoot2, shoot3, shoot4, shoot5, shoot6,
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
