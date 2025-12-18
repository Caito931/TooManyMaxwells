using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TooManyMaxwells;

public class GameRoot : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    public static int winWidth = 1920;
    public static int winHeight = 1080;
    public static bool isFullScreen = false;

    public static KeyboardState keyboardState;
    public static KeyboardState previousKeyboardState;

    public static MouseState mouseState;
    public static MouseState previousMouseState;

    public static Player player;

    public static Gun hkg36;
    public static Gun scar;
    public static Gun mp5;
    public static Gun vector;
    public static Gun aa12;
    public static Gun shotgun;
    public static Gun awp;
    public static Gun barrett;
    
    public static Spawner spawner1;
    public static Spawner spawner2;
    public static Spawner spawner3;
    public static Spawner spawner4;

    public static bool GameOver = false;
    public static bool GameWon = false;    
    public static Rectangle dogeHitbox;
    public static string endMessage;

    public GameRoot()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // Window
        Window.Title = "Too Many Maxwells";
        _graphics.PreferredBackBufferWidth = winWidth;
        _graphics.PreferredBackBufferHeight = winHeight;
        _graphics.IsFullScreen = isFullScreen;
        _graphics.ApplyChanges();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Load Game Assets
        Assets.Load(Content, GraphicsDevice);

        // Doge
        dogeHitbox = new Rectangle(winWidth/2 - Assets.doge.Width/2, winHeight/2 - Assets.doge.Height/2, Assets.doge.Width, Assets.doge.Height);

        // Player
        player = new Player();

        // Hkg36
        hkg36 = new Hkg36();

        // Scar
        scar = new Scar();

        // Mp5
        mp5 = new Mp5();

        // Vector
        vector = new Vector();

        // Aa12
        aa12 = new Aa12();

        // Shotgun
        shotgun = new Shotgun();

        // Awp
        awp = new Awp();

        // Barrett
        barrett = new Barrett();

        // Guns
        Gun.LoadGuns();

        // Spawners
        Spawner.SetDifficulty(Spawner.spawners, Spawner.Sets.Easy);
    }

    protected override void Dispose(bool disposing)
    {
        // Dispose Game Assets
        Assets.Dispose();

        base.Dispose(disposing);
    }

    protected override void Update(GameTime gameTime)
    {
        // DeltaTime
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Exit
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        // Keyboard State
        keyboardState = Keyboard.GetState();

        // Mouse State and Position
        mouseState = Mouse.GetState();
        Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);

        // Spawn Enemy
        // if (keyboardState.IsKeyDown(Keys.E) && !previousKeyboardState.IsKeyDown(Keys.E))
        // {
        //     Enemy.enemies.Add(new Enemy());
        // }

        if (!GameOver)
        {
            // Player
            player.Update(dt);

            // Guns
            for (int i = 0; i < Gun.guns.Count; i++)
            {
                Gun.guns[i].Update(dt, player, mouseState, previousMouseState, mousePosition);
                if (i != Gun.gunIndex)
                {
                    Gun.guns[i].selected = false;
                }
                else { Gun.guns[i].selected = true; }
            }
            
            // Change Gun
            if (keyboardState.IsKeyDown(Keys.F1) && !previousKeyboardState.IsKeyDown(Keys.F1)) 
            { 
                Gun.gunIndex = (Gun.gunIndex + 1) % Gun.guns.Count; 
            }

            // Bullet
            Bullet.Update(dt, mousePosition);

            // Casing
            Casing.Update(dt);

            // Shell
            Shell.Update(dt);

            // Enemy
            Enemy.Update(dt, player);

            // Bullet and Enemy Collision
            for (int i = Enemy.enemies.Count - 1; i >= 0; i--)
            { 
                for (int j = Bullet.bullets.Count - 1; j >= 0; j--)
                {
                    var enemy = Enemy.enemies[i];
                    var bullet = Bullet.bullets[j];

                    if (bullet.hitBox.Intersects(enemy.hitBox))
                    {
                        enemy.health -= bullet.damage;
                        enemy.Flash();
                        Bullet.bullets.RemoveAt(j);
                    }
                }
            }

            // Spawner
            Spawner.UpdateSpawners(dt);
        }
        if (GameOver || GameWon)
        {
            if (keyboardState.IsKeyDown(Keys.E) && !previousKeyboardState.IsKeyDown(Keys.E))
            {
                Restart();
            }
        }

        // Change fullscreen
        if (keyboardState.IsKeyDown(Keys.F11) && !previousKeyboardState.IsKeyDown(Keys.F11))
        {
            isFullScreen = !isFullScreen;
            _graphics.IsFullScreen = isFullScreen;
            _graphics.ApplyChanges();
        }

        previousKeyboardState = keyboardState;
        previousMouseState = mouseState;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        // Draw
        _spriteBatch.Begin();

        // Background
        _spriteBatch.Draw(Assets.background, new Rectangle(0,0, winWidth, winHeight), Color.White);

        // Casing
        Casing.Draw(_spriteBatch);

        // Shell
        Shell.Draw(_spriteBatch);

        // Player
        player.Draw(_spriteBatch);

        // Guns
        foreach (Gun gun in Gun.guns) { gun.Draw(_spriteBatch); }

        // Bullet
        Bullet.Draw(_spriteBatch);

        // Enemy
        Enemy.Draw(_spriteBatch);

        // Doge
        _spriteBatch.Draw(Assets.doge, new Vector2(winWidth/2 - Assets.doge.Width/2, winHeight/2 - Assets.doge.Height/2), Color.White);

        //-------------------------

        // UI
        
        // Gun UI
        foreach (Gun gun in Gun.guns) { gun.DrawUi(_spriteBatch); }

        // Press F1
        _spriteBatch.DrawString(Assets.font, "Press F1 to Switch gun", new Vector2(15, GameRoot.winHeight - 100), Color.White);

        // Press R
        _spriteBatch.DrawString(Assets.font, "Press R to reload", new Vector2(15, GameRoot.winHeight - 50), Color.White);

        // You Lost!
        if (GameOver || GameWon)
        {
            _spriteBatch.DrawString(Assets.font, endMessage, new Vector2(winWidth/2 - 9*16/2, 0 + 9*16/1.2f), Color.Red);
            _spriteBatch.DrawString(Assets.font, "Press E to Restart!", new Vector2(winWidth/2 - 19*16/2, 0 + 19*16), Color.Red);
        }

        _spriteBatch.End();
        

        base.Draw(gameTime);
    }

    static void Restart()
    {
        Spawner.SetDifficulty(Spawner.spawners, Spawner.Sets.Easy);

        GameOver = false;
        GameWon = false;

        Casing.casings.Clear();
        Shell.shells.Clear();
        Bullet.bullets.Clear();
        Enemy.enemies.Clear();

        player.pos = new Vector2(winWidth/2 - player.width/2, winHeight/2 - player.height/2);

        foreach (Gun gun in Gun.guns)
        {
            gun.ammoCount = gun.maxAmmoCount;
        }
    }
}

// dotnet publish -c Release -r linux-x64 --self-contained true 
// dotnet publish -c Release -r win-x64