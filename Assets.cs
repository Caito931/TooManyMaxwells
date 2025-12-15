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
    public static Texture2D bullet;
    public static Texture2D casing;
    public static Texture2D ball;
    public static Texture2D shell;
    public static Texture2D muzzleFlash;
    public static Texture2D doge;

    // List SoundEffect
    public static List<SoundEffect> sounds = new List<SoundEffect>();
    public static SoundEffect shoot;
    public static SoundEffect shoot2;
    public static SoundEffect shoot3;
    public static SoundEffect shoot4;
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
        bullet = Content.Load<Texture2D>("bullet");
        casing = Content.Load<Texture2D>("casing");
        ball = Content.Load<Texture2D>("ball");
        shell = Content.Load<Texture2D>("shell");
        muzzleFlash = Content.Load<Texture2D>("muzzleFlash");
        doge = Content.Load<Texture2D>("doge");

        textures.Add(background);
        textures.Add(background2);
        textures.Add(walter);
        textures.Add(maxwellAtlas);
        textures.Add(ak47);
        textures.Add(m4a1);
        textures.Add(hkg36);
        textures.Add(aa12);
        textures.Add(bullet);
        textures.Add(casing);
        textures.Add(ball);
        textures.Add(shell);
        textures.Add(muzzleFlash);
        textures.Add(doge);

        shoot = Content.Load<SoundEffect>("shoot");
        shoot2 = Content.Load<SoundEffect>("shoot2");
        shoot3 = Content.Load<SoundEffect>("shoot3");
        shoot4 = Content.Load<SoundEffect>("shoot4");
        reload = Content.Load<SoundEffect>("reload");

        sounds.Add(shoot);
        sounds.Add(shoot2);
        sounds.Add(shoot3);
        sounds.Add(shoot4);
        sounds.Add(reload);

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
