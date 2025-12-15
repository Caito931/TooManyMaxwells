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
    public string facing;
    public int damage;
    public static GunType[] gunTypes = [GunType.Hkg36, GunType.M4a1, GunType.Ak47];
    public int gunTypeIndex = 0;
    public GunType type;

    public Gun()
    {
        type = GunType.Hkg36;
        if (type == GunType.M4a1)
        {
            width = Assets.m4a1.Width/2;
            height = Assets.m4a1.Height/2;
        }
        if (type == GunType.Ak47)
        {
            width = Assets.ak47.Width/2;
            height = Assets.ak47.Height/2;
        }
        if (type == GunType.Hkg36)
        {
            width = Assets.hkg36.Width/2;
            height = Assets.hkg36.Height/2;
        }

        barrelLength = width * 0.55f;
        pos = new Vector2(GameRoot.winWidth/2 - width/2, GameRoot.winHeight/2 - height/2);
        hitBox = new Rectangle((int)pos.X, (int)pos.Y, (int)width, (int)height);
        damage = 20;
    }

    public void Update(float dt, Player player, MouseState mouseState, Vector2 mousePosition)
    {
        // Switch Type
        if (GameRoot.ks.IsKeyDown(Keys.F1) && !GameRoot.previousKs.IsKeyDown(Keys.F1))
        {
            gunTypeIndex = (gunTypeIndex + 1) % gunTypes.Length;
            type = gunTypes[gunTypeIndex];
        }
        
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
            muzzleFlashAngle = new Random().Next(-1, 3);
            ammoCount -= 1;
            Casing.casings.Add(new Casing(this));

            if (type == GunType.M4a1)
            {
                Assets.shoot2.Play();
            }
            else if (type == GunType.Ak47)
            {
                Assets.shoot.Play();
            }
            else if (type == GunType.Hkg36)
            {
                Assets.shoot3.Play();
            }
        }

        // Reload
        if (GameRoot.ks.IsKeyDown(Keys.R) && !GameRoot.previousKs.IsKeyDown(Keys.R))
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

        // Clamp Recoil
        recoilAmount = MathHelper.Clamp(recoilAmount, 0f, RecoilKick);

        // Update Gun Position
        pos = new Vector2(
            player.pos.X + player.width / 1.2f,
            player.pos.Y + player.height / 1.5f
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
        // Facing left?
        bool facingLeft = MathF.Abs(angle) > MathF.PI / 2f;
        fx = facingLeft
            ? SpriteEffects.FlipVertically
            : SpriteEffects.None;
        
        if (facingLeft)
        {
            facing = "left";
            pos.X -= width / 4f;
        }
        else
        {
            facing = "right";
        }
        
        // Decide Angle, Texture and Muzzle Pos
        float drawAngle = angle;
        Texture2D drawTexture = Assets.whitePixel;
        Vector2 muzzlePos =  new Vector2(0, 0);
        if (type == GunType.M4a1)
        {
            drawTexture = Assets.m4a1;
            barrelLength = width/2f * 1.1f;
                muzzlePos =  new Vector2(
                pos.X + direction.X * barrelLength * 1.5f + 8,
                pos.Y + direction.Y * barrelLength * 1.5f - 12
            );
        }
        if (type == GunType.Ak47)
        {
            drawTexture = Assets.ak47;
            barrelLength = width/2f;
            muzzlePos =  new Vector2(
                pos.X + direction.X * barrelLength * 1.5f,
                pos.Y + direction.Y * barrelLength * 1.5f - 20
            );
        }
        if (type == GunType.Hkg36)
        {
            drawTexture = Assets.hkg36;
            barrelLength = width/2f * 0.55f;
            muzzlePos =  new Vector2(
                pos.X + direction.X * barrelLength * 1.5f,
                pos.Y + direction.Y * barrelLength * 1.5f - 10
            );
        }

        // Gun
        spriteBatch.Draw(
            drawTexture, 
            pos, 
            null,
            Color.White, 
            drawAngle,
            new Vector2(width, height),
            0.5f, 
            fx, 
            0f
        );

        // Muzzle Flash
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

    public enum GunType
    {
        Ak47,
        M4a1,
        Hkg36
    }
}