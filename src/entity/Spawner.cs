using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TooManyMaxwells;

namespace TooManyMaxwells;

public class Spawner
{
    public static List<Spawner> spawners = new List<Spawner>();
    public const float DEFAULT_SPAWN_TIME = 2;
    public bool active;
    public float spawnTime;
    public float spawnTimer;
    public Vector2 pos;
    public int waveCount;
    public int Id;

    public Spawner(int PosIndex, float SpawnTime, int WaveCount)
    {
        Id = PosIndex;
        active = true;
        pos = SetEnemyPos(PosIndex);; // possiblePositions[new Random().Next(0, possiblePositions.Length)];
        spawnTime = SpawnTime; // DEFAULT_SPAWN_TIME;
        spawnTimer = spawnTime;
        waveCount = WaveCount;
    }

    public void Update(float dt)
    {
        if (active)
        {
            if (spawnTimer > 0)
            {
                spawnTimer -= 1 * dt;

                if (spawnTimer <= 0)
                {
                    pos = SetEnemyPos(Id);
                    Enemy.enemies.Add(new Enemy(pos));
                    spawnTime -= 0.1f;
                    spawnTimer = spawnTime;
                    waveCount--;
                }
            }

            if (waveCount <= 0)
            {
                active = false;
            }
        }
    }

    public static void UpdateSpawners(float dt)
    {
        for (int i = spawners.Count - 1; i >= 0; i--)
        {
            var spawner = spawners[i];

            spawner.Update(dt);
        }
        foreach (Spawner s in spawners)
        {
            if (s.active)
            {
                
            }
            else if (!s.active && Enemy.enemies.Count <= 0)
            {
                GameRoot.GameWon = true;
                GameRoot.endMessage = "You won!!";
            }
        }
    }

    public Vector2 SetEnemyPos(int PosIndex)
    {
        Vector2 Pos = new Vector2();

        if (PosIndex == 1) { Pos = new Vector2(-(400*0.25f), new Random().Next(0, GameRoot.winHeight)); } // Left
        if (PosIndex == 2) { Pos = new Vector2(GameRoot.winWidth+(400*0.25f), new Random().Next(0, GameRoot.winHeight)); } // Right
        if (PosIndex == 3) { Pos = new Vector2(new Random().Next(0, GameRoot.winWidth), -(400*0.25f)); } // Up
        if (PosIndex == 4) { Pos = new Vector2(new Random().Next(0, GameRoot.winWidth), GameRoot.winHeight+(400*0.25f)); } // Down

        return Pos;
    }


}