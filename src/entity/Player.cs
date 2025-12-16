using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TooManyMaxwells;

namespace TooManyMaxwells;

public class Player
{
    public Rectangle hitBox;
    public Vector2 pos;
    public float speed;
    public float width;
    public float height;

    public Player()
    {
        width = Assets.walter.Width/2;
        height = Assets.walter.Height/2;
        pos = new Vector2(GameRoot.winWidth/2 - width/2, GameRoot.winHeight/2 - height/2);
        speed = 200f;
        hitBox = new Rectangle((int)pos.X, (int)pos.Y, (int)width, (int)height);
    }

    public void Update(float dt)
    {
        // Movment
        if (GameRoot.keyboardState.IsKeyDown(Keys.A) && pos.X > 0)
        {
            pos.X -= speed * dt;
        }
        if (GameRoot.keyboardState.IsKeyDown(Keys.D) && pos.X + width < GameRoot.winWidth)
        {
            pos.X += speed * dt;
        }
        if (GameRoot.keyboardState.IsKeyDown(Keys.W) && pos.Y > 0)
        {
            pos.Y -= speed * dt;
        }
        if (GameRoot.keyboardState.IsKeyDown(Keys.S) && pos.Y + height < GameRoot.winHeight)
        {
            pos.Y += speed * dt;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Assets.walter, new Rectangle((int)pos.X, (int)pos.Y, (int)width, (int)height), Color.White);
    }
}