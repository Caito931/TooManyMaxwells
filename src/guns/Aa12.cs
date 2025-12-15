using System;
using System.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TooManyMaxwells;

namespace TooManyMaxwells;

public class Aa12 : Gun
{
    public Aa12()
    {
        type = GunType.Aa12;
        Gun.UpdateGunStats(this);
        Gun.SetGunStats(this);
    }

    public override void Update(float dt, Player player, MouseState mouseState, Vector2 mousePosition)
    {
        base.Update(dt, player, mouseState, mousePosition);

        // Do here another stuff
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        // Do here another stuff
    }
}