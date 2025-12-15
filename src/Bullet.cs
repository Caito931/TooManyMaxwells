using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TooManyMaxwells;

namespace TooManyMaxwells;

public class Bullet
{
    public static List<Bullet> bullets = new List<Bullet>();
    public Texture2D texture;
    public SpriteEffects fx;
    public Rectangle hitBox;
    public Vector2 pos;
    public Vector2 velocity;
    public float speed = 2000f;
    public float width;
    public float height;
    public Vector2 direction;
    public float angle;
    public int dirX;
    public int damage;

    public Bullet(Gun gun, Player player, Texture2D Texture)
    {
        texture = Texture;
        direction = gun.direction;
        angle = gun.angle;
        velocity = direction * speed;

        damage = gun.damage;

        width = texture.Width/2;
        height = texture.Height/2;
        pos = new Vector2(
            gun.pos.X + gun.direction.X * gun.barrelLength, 
            gun.pos.Y + gun.direction.Y * gun.barrelLength - 12
        );
        hitBox = new Rectangle((int)pos.X, (int)pos.Y, (int)width, (int)height);
    }

    public static void Update(float dt, Vector2 mousePosition)
    {
        for (int i = bullets.Count - 1; i >= 0; i--)
        {
            var bullet = bullets[i];

            bullet.hitBox = new Rectangle((int)bullet.pos.X, (int)bullet.pos.Y, (int)bullet.width, (int)bullet.height);

            // Movment
            bullet.pos += bullet.velocity * dt;

            // Remove
            if (bullet.pos.X >= GameRoot.winWidth) { bullets.RemoveAt(i); }
            else if (bullet.pos.X - bullet.width <= 0) { bullets.RemoveAt(i); }
            else if (bullet.pos.Y >= GameRoot.winHeight) { bullets.RemoveAt(i); }
            else if (bullet.pos.Y - bullet.height <= 0) { bullets.RemoveAt(i); }

        }

    }

    public static void Draw(SpriteBatch spriteBatch)
    {
        foreach (Bullet b in bullets)
        {
            b.fx = b.dirX < 0 ? SpriteEffects.FlipVertically : SpriteEffects.None;

            spriteBatch.Draw(
                b.texture, 
                b.pos, 
                null,
                Color.White, 
                b.angle,
                new Vector2(b.width/2f, b.height/2f),
                0.5f, 
                b.fx, 
                0f
            );
        }
    }
}