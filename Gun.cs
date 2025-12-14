using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TooManyMaxwells;

namespace TooManyMaxwells;

public class Gun
{
    public Rectangle hitBox;
    public Vector2 pos;
    public Vector2 drawPos;
    public float width;
    public float height;
    public Vector2 direction;
    public float angle;
    public float shotTime = 0.1f;
    public float shotTimer = 0f;
    public float recoilAmount = 0f;
    public float recoilVelocity = 0f;
    public const float RecoilKick = 300f;
    public const float RecoilReturn = 150f;
    public bool muzzleFlashVisible = false;
    public float muzzleFlashAngle;
    public float barrelLength;
    public SpriteEffects fx;
    public int ammoCount = 30;

    public Gun()
    {
        width = Assets.ak47.Width/2;
        height = Assets.ak47.Height/2;
        barrelLength = width * 0.55f;
        pos = new Vector2(GameRoot.winWidth/2 - width/2, GameRoot.winHeight/2 - height/2);
        hitBox = new Rectangle((int)pos.X, (int)pos.Y, (int)width, (int)height);
    }

    public void Update(float dt, Player player, MouseState mouseState, Vector2 mousePosition)
    {
        // Time between each Shot
        if (shotTimer != 0)
        {
            muzzleFlashVisible = true;
            shotTimer -= dt;

            if (shotTimer <= 0) { shotTimer = 0; muzzleFlashVisible = false; }
        }

        // Shoot
        if (mouseState.LeftButton == ButtonState.Pressed && shotTimer <= 0 && ammoCount > 0)
        {
            Bullet.bullets.Add(new Bullet(this, player));
            recoilVelocity += RecoilKick;
            shotTimer = shotTime;
            Assets.shoot.Play();
            muzzleFlashAngle = new Random().Next(-1, 3);
            ammoCount -= 1;
        }

        // Reload
        if (GameRoot.ks.IsKeyDown(Keys.R) && !GameRoot.previousKs.IsKeyDown(Keys.R) && ammoCount <= 0)
        {
            ammoCount = 30;
            Assets.reload.Play();
        }

        // Recoil
        float recoilForce = -recoilAmount * RecoilReturn;
        recoilVelocity += recoilForce * dt;

        // Damping
        recoilVelocity *= 0.85f;

        // Integrate
        recoilAmount += recoilVelocity * dt;

        // Snap small values to zero
        if (MathF.Abs(recoilAmount) < 0.01f)
        {
            recoilAmount = 0f;
            recoilVelocity = 0f;
        }

        recoilAmount = MathHelper.Clamp(recoilAmount, 0f, RecoilKick);

        pos = new Vector2(
            player.pos.X + player.width / 4f,
            player.pos.Y + player.height / 2f
        );

        // Direction
        direction = mousePosition - pos;
        direction.Normalize();
        angle = (float)Math.Atan2(direction.Y, direction.X); // Radians

        // Apply recoil
        Vector2 recoilOffset = -direction * recoilAmount;
        pos += recoilOffset;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        fx = SpriteEffects.None;
        float drawAngle = angle;

        spriteBatch.Draw(
            Assets.ak47, 
            pos, 
            null,
            Color.White, 
            drawAngle,
            new Vector2(width/2f, height/2f),
            0.5f, 
            fx, 
            0f
        );

        Vector2 muzzlePos = pos + direction * barrelLength * 1.5f;
        if (muzzleFlashVisible) 
        {
            spriteBatch.Draw(
                Assets.muzzleFlash,
                muzzlePos,
                null,
                Color.White,
                muzzleFlashAngle,
                new Vector2(
                    Assets.muzzleFlash.Width / 2f,
                    Assets.muzzleFlash.Height / 2f
                ),
                1f,
                SpriteEffects.None,
                0f
            );
        }

        // Ammo
        spriteBatch.DrawString(Assets.font, $"Ammo: {ammoCount}", new Vector2(15, GameRoot.winHeight - 50), Color.White);
    }
}