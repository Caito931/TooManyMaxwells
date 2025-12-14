using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TooManyMaxwells;

public class Animation
{
    public Texture2D atlas;
    public Vector2 pos;
    public Color color;
    public float FPS;
    public int counter;
    public int activeFrame;
    public int numFrames;
    public float scale;
    public float w; // width
    public float h; // height

    public Animation(Texture2D Atlas, float Scale, Vector2 Pos, float Fps, int Counter, int ActiveFrame, int NumFrames, float Width, float Height)
    {
        atlas = Atlas;
        scale = Scale;
        pos = Pos;
        FPS = Fps;
        counter = Counter;
        activeFrame = ActiveFrame;
        numFrames = NumFrames;
        w = Width;
        h = Height; 
    }

    public void Update(float dt)
    {
        counter++;
        if (counter >= FPS)
        {
            counter = 0;
            activeFrame++;

            if (activeFrame == numFrames)
                activeFrame = 0;
        }
    }
    public void Draw(SpriteBatch spriteBatch)
    { 
        spriteBatch.Draw(
            atlas,
            pos,
            new Rectangle(activeFrame * (int)w, 0, (int)w, (int)h),
            color,
            0f,
            Vector2.Zero,
            scale,
            0f,
            0f
        );
    }
    
}