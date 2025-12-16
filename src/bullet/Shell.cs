using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TooManyMaxwells;

namespace TooManyMaxwells;

class Shell
{
    public static List<Shell> shells = new List<Shell>();
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

    public Shell(Gun gun)
    {
        this.rotationSpeed = new Random().Next(360, 360);
        this.alphaSpeed = 255 / destroyTime;
        this.rotationDir = Shell.possibleRDir[new Random().Next(0, 2)];
        this.pos = new Vector2(gun.pos.X, gun.pos.Y);
        this.endPos = new Vector2(gun.pos.X, gun.pos.Y + gun.height);
        this.width = Assets.shell.Width/2;
        this.height = Assets.shell.Height/2;
        this.size = new Vector2(width, height);
        
        if (gun.facing == "right") { this.dir = -1; }
        else { this.dir = 1; }
    }

    // Update
    public static void Update(float dt)
    {
        for (int i = shells.Count - 1; i >= 0; i --)
        {
            var casing = shells[i];

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
                    shells.RemoveAt(i); 
                }
            }

            // GameOver
            if (GameRoot.GameOver)
            {
                shells.Clear();
            }
        }

    }

    // Draw
    public static void Draw(SpriteBatch spriteBatch)
    {
        foreach (Shell s in shells)
        {
            spriteBatch.Draw(
                Assets.shell, 
                s.pos, 
                null,
                s.color, 
                s.angle,
                new Vector2(s.width/2f, s.height/2f),
                0.2f, 
                SpriteEffects.None, 
                0f
            );

        }
    }

}