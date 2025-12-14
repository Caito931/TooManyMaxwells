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
    public float FPS;
    public int counter;
    public int activeFrame;
    public int numFrames;
    public float w; // width
    public float h; // height

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
            Color.White,
            0f,
            Vector2.Zero,
            1f,
            0f,
            0f
        );
    }
    
}