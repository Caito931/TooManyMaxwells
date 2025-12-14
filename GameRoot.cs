using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TooManyMaxwells;

public class GameRoot : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    public static int winWidth = 1080;
    public static int winHeight = 720;
    public static KeyboardState ks;
    public static KeyboardState previousKs;
    public Player player;
    public Gun gun;

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

        // Gun
        gun = new Gun();

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
        
        // KeyboardState
        ks = Keyboard.GetState();

        // Mouse State and Position
        MouseState mouseState = Mouse.GetState();
        Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);

        // Spawn Enemy
        if (ks.IsKeyDown(Keys.E) && !previousKs.IsKeyDown(Keys.E))
        {
            Enemy.enemies.Add(new Enemy());
        }

        player.Update(dt);

        gun.Update(dt, player, mouseState, mousePosition);

        Bullet.Update(dt, mousePosition);

        Casing.Update(dt);

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

        previousKs = ks;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        // Draw
        _spriteBatch.Begin();

        // Background
        _spriteBatch.Draw(Assets.background, new Vector2(0,0), Color.White);

        // Casing
        Casing.Draw(_spriteBatch);

        // Player
        player.Draw(_spriteBatch);

        // Gun
        gun.Draw(_spriteBatch);

        // Bullet
        Bullet.Draw(_spriteBatch);

        // Enemy
        Enemy.Draw(_spriteBatch);

        _spriteBatch.Draw(Assets.doge, new Vector2(winWidth/2 - Assets.doge.Width/2, winHeight/2 - Assets.doge.Height/2), Color.White);

        _spriteBatch.End();
        

        base.Draw(gameTime);
    }
}
