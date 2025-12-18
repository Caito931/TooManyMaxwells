using System;
using System.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TooManyMaxwells;

namespace TooManyMaxwells;

public class Vector : Gun
{
    public Vector()
    {
        type = GunType.Vector;
        Gun.UpdateGunStats(this);
        Gun.SetGunStats(this);
    }

    public override void Update(float dt, Player player, MouseState mouseState, MouseState previousMouseState, Vector2 mousePosition)
    {
        base.Update(dt, player, mouseState, previousMouseState, mousePosition);

        // Do here another stuff
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        // Do here another stuff
    }

    public override void DrawUi(SpriteBatch spriteBatch)
    {
        base.DrawUi(spriteBatch);

        // Do here another stuff
    }
}