using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TooManyMaxwells;

namespace TooManyMaxwells;

class Casing
{
    public static List<Casing> casings = new List<Casing>();
    public Vector2 pos = Vector2.Zero;
    public Vector2 endPos;
    public Vector2 vel;
    public Vector2 size;
    public Color color = Color.White;
    public float width;
    public float height;
    public float destroyTime = 1f;
    public float destroyTimer = 0f;
    public float alphaSpeed;
    public float angle = 0f;
    public float rotationSpeed;
    public float rotationDir;
    private static float[] possibleRDir = [-1, 1];
    public float dir = 0f;

    public Casing(Gun gun)
    {
        this.rotationSpeed = new Random().Next(360, 360);
        this.alphaSpeed = 255 / destroyTime;
        this.rotationDir = Casing.possibleRDir[new Random().Next(0, 2)];
        this.pos = new Vector2(gun.pos.X, gun.pos.Y);
        this.endPos = new Vector2(gun.pos.X, gun.pos.Y + gun.height);
        this.width = Assets.casing.Width/2;
        this.height = Assets.casing.Height/2;
        this.size = new Vector2(width, height);
        
        if (gun.facing == "right") { this.dir = -1; this.pos = new Vector2(gun.pos.X - gun.width/4, gun.pos.Y); }
        else { this.dir = 1; }
    }

    // Update
    public static void Update(float dt)
    {
        for (int i = casings.Count - 1; i >= 0; i --)
        {
            var casing = casings[i];

            // Change Pos & Vel
            if (casing.pos.Y < casing.endPos.Y)
            {
                casing.vel = new Vector2(0, casing.vel.Y + 500 * dt);
                // Update Rotation
                casing.angle += casing.rotationSpeed * dt * casing.rotationDir;
                // Update Pos
                casing.pos = new Vector2(casing.pos.X + 100 * dt * casing.dir, casing.pos.Y + casing.vel.Y * dt);
            }

            // Remove If Touch Ground
            if (casing.pos.Y > casing.endPos.Y)
            {
                casing.angle = 0f;
                casing.color.A -= (byte)(casing.alphaSpeed * dt); // Update Alpha
                casing.destroyTimer += dt;

                if (casing.destroyTimer >= casing.destroyTime) 
                { 
                    casings.RemoveAt(i); 
                }
            }

            // GameOver
            if (GameRoot.GameOver)
            {
                casings.Clear();
            }
        }

    }

    // Draw
    public static void Draw(SpriteBatch spriteBatch)
    {
        foreach (Casing c in casings)
        {
            spriteBatch.Draw(
                Assets.casing, 
                c.pos, 
                null,
                c.color, 
                c.angle,
                new Vector2(c.width/2f, c.height/2f),
                0.2f, 
                SpriteEffects.None, 
                0f
            );

        }
    }

}