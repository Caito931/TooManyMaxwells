using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TooManyMaxwells;

namespace TooManyMaxwells;

public class Enemy
{
    public static List<Enemy> enemies = new List<Enemy>();
    public Animation animation;
    public Rectangle hitBox;
    public Vector2 pos;
    public Color color;
    public float speed;
    public float width;
    public float height;
    public int health;
    public float flashTime = 0.05f;
    public float flashTimer = 0f;
    public Vector2 Target;
    public bool scaleAnimating = false;
    public int scaleDir = 1;

    public Enemy()
    {
        Vector2 possiblePos1 = new Vector2(-200, new Random().Next(0, GameRoot.winHeight)); // Left
        Vector2 possiblePos2 = new Vector2(GameRoot.winWidth+200, new Random().Next(0, GameRoot.winHeight)); // Right
        Vector2 possiblePos3 = new Vector2(new Random().Next(0, GameRoot.winWidth), -200); // Up
        Vector2 possiblePos4 = new Vector2(new Random().Next(0, GameRoot.winWidth), GameRoot.winHeight+200); // Down

        Vector2[] possiblePositions = [possiblePos1, possiblePos2, possiblePos3, possiblePos4];

        pos = possiblePositions[new Random().Next(0, possiblePositions.Length)];
        float scale = 0.25f;
        width = 400;
        height = 400;

        animation = new Animation(
            Assets.maxwellAtlas,
            scale,
            pos,
            4,
            0,
            0,
            14,
            width,
            height
        );
        color = Color.White;
        animation.color = color;
        speed = 100f;
        hitBox = new Rectangle((int)pos.X, (int)pos.Y, (int)width, (int)height);
        health = 100;

        Target = new Vector2(GameRoot.winWidth/2f, GameRoot.winHeight/2f);
    }

    public static void Update(float dt, Player player)
    {
        // Movment
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            var enemy = enemies[i];

            if (enemy.flashTimer != 0)
            {
                enemy.flashTimer -= dt;
                enemy.color = Color.White; // Red

                if (enemy.flashTimer <= 0)
                {
                    enemy.flashTimer = 0;
                    enemy.color = Color.White;
                    enemy.animation.scale = 0.25f;
                }
            }

            if (enemy.scaleAnimating)
            {
                enemy.popUp(1f, 0.2f, dt);
            }

            if (enemy.health <= 0)
            {
                Enemy.enemies.RemoveAt(i);
            }

            if (enemy.pos.X < enemy.Target.X)
            {
                enemy.pos.X += enemy.speed * dt;
            } 
            else if (enemy.pos.X > enemy.Target.X)
            {
                enemy.pos.X -= enemy.speed * dt;
            }

            // if (enemy.pos.Y - player.pos.Y <= 0.01)
            // {
            //     enemy.pos.Y = player.pos.Y;
            // }
            if (enemy.pos.Y < enemy.Target.Y)
            {
                enemy.pos.Y += enemy.speed * dt;
            } 
            else if (enemy.pos.Y > enemy.Target.Y)
            {
                enemy.pos.Y -= enemy.speed * dt;
            }

            enemy.animation.pos = enemy.pos;

            enemy.hitBox = new Rectangle(
                (int)enemy.animation.pos.X, (int)enemy.animation.pos.Y, 
                (int)(enemy.animation.w*enemy.animation.scale), (int)(enemy.animation.h*enemy.animation.scale)
            );

            enemy.animation.Update(dt);
            enemy.animation.color = enemy.color;

        }

    }

    public void Flash()
    {
        this.flashTimer = this.flashTime;
        animation.scale = 0.2f;
        scaleAnimating = true;
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
        foreach (Enemy e in Enemy.enemies)
        {
            e.animation.Draw(spriteBatch);

            spriteBatch.Draw(
                Assets.whitePixel, 
                new Rectangle((int)e.animation.pos.X, (int)e.animation.pos.Y, e.health, 10),
                Color.Green
            );
        }
       
    }

    public void popUp(float SYspeed, float minSY, float dt)
    {
        if (scaleDir == 1)
        {
            animation.scale -= SYspeed * dt;
            animation.pos.Y -= SYspeed*100 * dt;
            animation.pos.X -= SYspeed*100 * dt;
            if (animation.scale <= minSY) 
            {
                scaleDir = 0;
            }
        }
        if (scaleDir == 0)
        {
            animation.scale += SYspeed * dt;
            animation.pos.Y += SYspeed*100 * dt;
            animation.pos.X += SYspeed*100 * dt;

            if (animation.scale >= 0.25f) 
            {
                scaleDir = 1;
                scaleAnimating = false;
                animation.scale = 0.25f;
            }
        }
    }
}