using System;
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
    public Player player;
    public static Gun hkg36;
    public static Gun m4a1;
    public static Gun ak47;
    public static Gun aa12;
    public static Gun barrett;
    public static Gun scar;

    public GameRoot()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        // Window
        Window.Title = "Too Many Maxwells";
        _graphics.PreferredBackBufferWidth = winWidth;
        _graphics.PreferredBackBufferHeight = winHeight;
        // _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        // _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        _graphics.IsFullScreen = isFullScreen;
        _graphics.ApplyChanges();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Load Game Assets
        Assets.Load(Content, GraphicsDevice);

        // Player
        player = new Player();

        // Hkg36
        hkg36 = new Hkg36();

        // M4a1
        m4a1 = new M4a1();

        // Ak47
        ak47 = new Ak47();

        // Aa12
        aa12 = new Aa12();

        // Barrett
        barrett = new Barrett();

        // Scar
        scar = new Scar();

        // Guns
        Gun.LoadGuns();

        // TODO: use this.Content to load your game content here
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
        if (keyboardState.IsKeyDown(Keys.E) && !previousKeyboardState.IsKeyDown(Keys.E))
        {
            Enemy.enemies.Add(new Enemy());
        }

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

        // Change fullscreen
        if (keyboardState.IsKeyDown(Keys.F2) && !previousKeyboardState.IsKeyDown(Keys.F2))
        {
            isFullScreen = !isFullScreen;
            _graphics.IsFullScreen = isFullScreen;
            _graphics.ApplyChanges();

            System.Console.WriteLine("oi");
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
        _spriteBatch.Draw(Assets.background2, new Rectangle(0,0, winWidth, winHeight), Color.White);

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

        // Press F1
        _spriteBatch.DrawString(Assets.font, "Press F1 to Switch gun", new Vector2(255, GameRoot.winHeight - 50), Color.White);

        // Press R
        _spriteBatch.DrawString(Assets.font, "Press R to reload", new Vector2(725, GameRoot.winHeight - 50), Color.White);

        _spriteBatch.End();
        

        base.Draw(gameTime);
    }
}
