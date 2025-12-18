using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.PortableExecutable;
using System.Runtime.ConstrainedExecution;
using System.Security.Principal;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TooManyMaxwells;

namespace TooManyMaxwells;

public class Gun
{
    public bool selected;
    public Rectangle hitBox;
    public Texture2D drawTexture;
    public Vector2 pos;
    public Vector2 drawPos;
    public Vector2 muzzlePos;
    public Vector2 origin;
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
    public int maxAmmoCount;
    public int ammoCount;
    public string facing;
    public int damage;
    public static List<Gun> guns;
    public static int gunIndex = 0;
    public static GunType[] gunTypes = [GunType.Hkg36, GunType.M4a1, GunType.Ak47, GunType.Aa12, GunType.Barrett, GunType.Scar];
    public int gunTypeIndex = 0;
    public GunType type;
    public GunMode gunMode;
    int defaultAmmoBarWidth = 500;
    int defaultAmmoBarHeight = 100;
    
    public static void LoadGuns()
    {
        guns = [GameRoot.hkg36, GameRoot.m4a1, GameRoot.ak47, GameRoot.aa12, GameRoot.barrett, GameRoot.scar];
    }

    public virtual void Update(float dt, Player player, MouseState mouseState, MouseState previousMouseState, Vector2 mousePosition)
    {
        // Update Gun Position
        pos = new Vector2(
            player.pos.X + player.width / 1.2f,
            player.pos.Y + player.height / 1.5f
        );

        if (selected)
        {
            // // Switch Gun
            // if (GameRoot.ks.IsKeyDown(Keys.F1) && !GameRoot.previousKs.IsKeyDown(Keys.F1))
            // {
            //     gunTypeIndex = (gunTypeIndex + 1) % gunTypes.Length;
            //     type = gunTypes[gunTypeIndex];
            //     UpdateGunStats(this);
            // }
            
            // Direction
            direction = mousePosition - pos;
            direction.Normalize();
            angle = (float)Math.Atan2(direction.Y, direction.X); // Radians

            // Apply recoil
            Vector2 recoilOffset = -direction * recoilAmount;
            pos += recoilOffset;

            // Time between each Shot
            if (shotTimer != 0)
            {
                muzzleFlashVisible = true;
                shotTimer -= dt;

                if (shotTimer <= 0) { shotTimer = 0; muzzleFlashVisible = false; }
            }

            // Shoot
            Gun.Shoot(this, player, mouseState, previousMouseState);

            // Reload
            if (GameRoot.keyboardState.IsKeyDown(Keys.R) && !GameRoot.previousKeyboardState.IsKeyDown(Keys.R) && ammoCount <= 0)
            {
                ammoCount = maxAmmoCount;
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

        }
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        if (selected)
        {
            // Facing left?
            bool facingLeft = MathF.Cos(angle) < 0;

            fx = facingLeft
                ? SpriteEffects.FlipVertically
                : SpriteEffects.None;
            
            if (facingLeft)
            {
                facing = "left";
                if (!GameRoot.GameOver) { pos.X -= width / 4f; }
            }
            else
            {
                facing = "right";
                if (!GameRoot.GameOver) { pos.X -= width / 16f; }
            }
            
            // Decide Angle, Texture and Muzzle Pos
            float drawAngle = angle;
            Gun.UpdateGunDrawStats(this);

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
        }
    }

    public virtual void DrawUi(SpriteBatch spriteBatch)
    {
        if (selected)
        {
            // Back
            spriteBatch.Draw(Assets.whitePixel, new Rectangle(
                GameRoot.winWidth/2-defaultAmmoBarWidth/2, GameRoot.winHeight - defaultAmmoBarHeight+10, defaultAmmoBarWidth, 50),
                Color.Black
            );


            // Front
            spriteBatch.Draw(Assets.whitePixel, new Rectangle(
                GameRoot.winWidth/2-defaultAmmoBarWidth/2, GameRoot.winHeight - defaultAmmoBarHeight+10, ammoCount*defaultAmmoBarWidth/maxAmmoCount, 50),
                Color.Yellow
            );
            
            // Icon
            spriteBatch.Draw(
                Assets.ammoIcon, 
                new Vector2(GameRoot.winWidth/2-defaultAmmoBarWidth/2-Assets.ammoIcon.Width, 
                GameRoot.winHeight - defaultAmmoBarHeight+10 - Assets.ammoIcon.Height/2),
                Color.White
            );
        }
    }

    public static void Shoot(Gun gun, Player player, MouseState mouseState, MouseState previousMouseState)
    {
        if (mouseState.LeftButton == ButtonState.Pressed && gun.shotTimer <= 0 && gun.ammoCount > 0 && gun.gunMode == GunMode.Auto)
        {
            gun.recoilVelocity += RecoilKick;
            gun.shotTimer = gun.shotTime;
            gun.muzzleFlashAngle = new Random().Next(-1, 3);
            gun.ammoCount -= 1;

            if (gun.type == GunType.M4a1)
            {
                Assets.shoot2.Play();
                Casing.casings.Add(new Casing(gun));
                Bullet.bullets.Add(new Bullet(gun, player, Assets.bullet));
            }
            else if (gun.type == GunType.Ak47)
            {
                Assets.shoot.Play();
                Casing.casings.Add(new Casing(gun));
                Bullet.bullets.Add(new Bullet(gun, player, Assets.bullet));
            }
            else if (gun.type == GunType.Hkg36)
            {
                Assets.shoot3.Play();
                Casing.casings.Add(new Casing(gun));
                Bullet.bullets.Add(new Bullet(gun, player, Assets.bullet));
            }
            else if (gun.type == GunType.Aa12)
            {
                Assets.shoot4.Play();
                Shell.shells.Add(new Shell(gun));
                Bullet.bullets.Add(new Bullet(gun, player, Assets.ball));
            }
            else if (gun.type == GunType.Scar)
            {
                Assets.shoot6.Play();
                Casing.casings.Add(new Casing(gun));
                Bullet.bullets.Add(new Bullet(gun, player, Assets.bullet));
            }
        }
        if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton != ButtonState.Pressed && gun.ammoCount > 0 && gun.gunMode == GunMode.Semi)
        {
            gun.recoilVelocity += RecoilKick;
            gun.shotTimer = gun.shotTime;
            gun.muzzleFlashAngle = new Random().Next(-1, 3);
            gun.ammoCount -= 1;

            if (gun.type == GunType.Barrett)
            {
                Assets.shoot5.Play();
                Casing.casings.Add(new Casing(gun));
                Bullet.bullets.Add(new Bullet(gun, player, Assets.bullet));
            }

        }
    }

    public static void SetGunStats(Gun gun)
    {
        float BarrelLength = 0;
        int Damage = 0;

        switch (gun.type)
        {
            case GunType.Hkg36:
                BarrelLength = 0.55f;
                Damage = 15;
                gun.gunMode = GunMode.Auto;
            break;
            case GunType.M4a1:
                BarrelLength = 0.7f;
                Damage = 12;
                gun.gunMode = GunMode.Auto;
            break;
            case GunType.Ak47:
                BarrelLength = 0.7f;
                Damage = 20;
                gun.gunMode = GunMode.Auto;
            break;
            case GunType.Aa12:
                BarrelLength = 0.55f;
                Damage = 40;
                gun.gunMode = GunMode.Auto;
            break;
            case GunType.Barrett:
                BarrelLength = 0.55f;
                Damage = 50;
                gun.gunMode = GunMode.Semi;
            break;
            case GunType.Scar:
                BarrelLength = 0.55f;
                Damage = 30;
                gun.gunMode = GunMode.Auto;
            break;
        }

        gun.barrelLength = gun.width * BarrelLength;
        gun.pos = new Vector2(GameRoot.winWidth/2 - gun.width/2, GameRoot.winHeight/2 - gun.height/2);
        gun.hitBox = new Rectangle((int)gun.pos.X, (int)gun.pos.Y, (int)gun.width, (int)gun.height); // Useless for now
        gun.damage = Damage;
    }

    public static void UpdateGunStats(Gun gun)
    {
        if (gun.type == GunType.Hkg36)
        {
            gun.width = Assets.hkg36.Width/2;
            gun.height = Assets.hkg36.Height/2;
            gun.shotTime = 0.08f;
            gun.damage = 15;
            gun.maxAmmoCount = 30;
            gun.ammoCount = 30;
            gun.selected = true;
        }
        if (gun.type == GunType.M4a1)
        {
            gun.width = Assets.m4a1.Width/2;
            gun.height = Assets.m4a1.Height/2;
            gun.shotTime = 0.06f;
            gun.damage = 12;
            gun.maxAmmoCount = 30;
            gun.ammoCount = 30;
            gun.selected = true;
        }
        if (gun.type == GunType.Ak47)
        {
            gun.width = Assets.ak47.Width/2;
            gun.height = Assets.ak47.Height/2;
            gun.shotTime = 0.1f;
            gun.damage = 20;
            gun.maxAmmoCount = 30;
            gun.ammoCount = 30;
            gun.selected = true;
        }
        if (gun.type == GunType.Aa12)
        {
            gun.width = Assets.aa12.Width/2;
            gun.height = Assets.aa12.Height/2;
            gun.shotTime = 0.2f;
            gun.damage = 40;
            gun.maxAmmoCount = 20;
            gun.ammoCount = 20;
            gun.selected = true;
        }
        if (gun.type == GunType.Barrett)
        {
            gun.width = Assets.barrett.Width/2;
            gun.height = Assets.barrett.Height/2;
            gun.damage = 50;
            gun.shotTime = 0.2f;
            gun.maxAmmoCount = 5;
            gun.ammoCount = 5;
            gun.selected = true;
        }
        if (gun.type == GunType.Scar)
        {
            gun.width = Assets.scar.Width/2;
            gun.height = Assets.scar.Height/2;
            gun.damage = 30;
            gun.shotTime = 0.09f;
            gun.maxAmmoCount = 20;
            gun.ammoCount = 20;
            gun.selected = true;
        }


    }

    public static void UpdateGunDrawStats(Gun gun)
    {
        if (gun.type == GunType.Hkg36)
        {
            gun.drawTexture = Assets.hkg36;
            gun.barrelLength = gun.width/2f * 0.55f;
            gun.muzzlePos = new Vector2(
            gun.pos.X + gun.direction.X * gun.barrelLength * 1.5f,
            gun.pos.Y + gun.direction.Y * gun.barrelLength * 1.5f - 10);
        }
        if (gun.type == GunType.M4a1)
        {
            gun.drawTexture = Assets.m4a1;
            gun.barrelLength = gun.width/2f * 0.7f;
            gun.muzzlePos =  new Vector2(
                gun.pos.X + gun.direction.X * gun.barrelLength * 1.5f + 8,
                gun.pos.Y + gun.direction.Y * gun.barrelLength * 1.5f - 12
            );
        }
        if (gun.type == GunType.Ak47)
        {
            gun.drawTexture = Assets.ak47;
            gun.barrelLength = gun.width/2f * 0.7f;
            gun.muzzlePos =  new Vector2(
                gun.pos.X + gun.direction.X * gun.barrelLength * 1.5f,
                gun.pos.Y + gun.direction.Y * gun.barrelLength * 1.5f - 20
            );
        }
        if (gun.type == GunType.Aa12)
        {
            gun.drawTexture = Assets.aa12;
            gun.barrelLength = gun.width/2f * 0.55f;
            gun.muzzlePos =  new Vector2(
                gun.pos.X + gun.direction.X * gun.barrelLength * 1.5f,
                gun.pos.Y + gun.direction.Y * gun.barrelLength * 1.5f - 10
            );
        }
        if (gun.type == GunType.Barrett)
        {
            gun.drawTexture = Assets.barrett;
            gun.barrelLength = gun.width/2f * 0.6f;
            gun.muzzlePos =  new Vector2(
                gun.pos.X + gun.direction.X * gun.barrelLength * 1.5f,
                gun.pos.Y + gun.direction.Y * gun.barrelLength * 1.5f 
            );
        }
        if (gun.type == GunType.Scar)
        {
            gun.drawTexture = Assets.scar;
            gun.barrelLength = gun.width/2f * 0.7f;
            gun.muzzlePos =  new Vector2(
                gun.pos.X + gun.direction.X * gun.barrelLength * 1.5f,
                gun.pos.Y + gun.direction.Y * gun.barrelLength * 1.5f - 10
            );
        }
    }
    
    public enum GunType
    {
        Ak47,
        M4a1,
        Hkg36,
        Aa12,
        Barrett,
        Scar
    }

    public enum GunMode
    {
        Auto,
        Semi
    }
}