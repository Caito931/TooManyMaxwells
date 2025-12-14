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
    public float flashTime = 0.07f;
    public float flashTimer = 0f;

    public Enemy()
    {
        int[] possibleX = [-200, GameRoot.winWidth+200];
        int[] possibleY = [0, GameRoot.winHeight];

        pos = new Vector2(possibleX[new Random().Next(0, 2)], possibleY[new Random().Next(0, 2)]);
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
                enemy.color = Color.Red;

                if (enemy.flashTimer <= 0)
                {
                    enemy.flashTimer = 0;
                    enemy.color = Color.White;
                }
            }

            if (enemy.health <= 0)
            {
                Enemy.enemies.RemoveAt(i);
            }

            if (enemy.pos.X < player.pos.X)
            {
                enemy.pos.X += enemy.speed * dt;
            } 
            else if (enemy.pos.X > player.pos.X)
            {
                enemy.pos.X -= enemy.speed * dt;
            }

            // if (enemy.pos.Y - player.pos.Y <= 0.01)
            // {
            //     enemy.pos.Y = player.pos.Y;
            // }
            if (enemy.pos.Y < player.pos.Y)
            {
                enemy.pos.Y += enemy.speed * dt;
            } 
            else if (enemy.pos.Y > player.pos.Y)
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
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
        foreach (Enemy e in Enemy.enemies)
        {
            e.animation.Draw(spriteBatch);
        }
       
    }

}