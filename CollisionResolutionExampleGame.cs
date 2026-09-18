using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace CollisionResolutionExercise;

/// <summary>
/// An example game demonstrating elastic collisions
/// </summary>
public class CollisionResolutionExampleGame : Game
{
    // private variables
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    private List<BallSprite> balls;

    /// <summary>
    /// Constructs a new game
    /// </summary>
    public CollisionResolutionExampleGame()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        graphics.PreferredBackBufferWidth = Constants.GAME_WIDTH;
        graphics.PreferredBackBufferHeight = Constants.GAME_HEIGHT;

    }

    /// <summary>
    /// Initializes the game
    /// </summary>
    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        System.Random random = new System.Random();
        balls = new List<BallSprite>();
        for(int i = 0; i < 5; i++)
        {
            balls.Add(new BallSprite()
            {
                Center = new Vector2(random.Next(50, 680), random.Next(50, 310)),
                Velocity = new Vector2(50 - (float)random.NextDouble() * 100, 50 - (float)random.NextDouble() * 100),
                Mass = random.Next(5, 50)
            });
        }
        base.Initialize();
    }

    /// <summary>
    /// Loads game content
    /// </summary>
    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        foreach (var ball in balls) ball.LoadContent(Content);
    }

    /// <summary>
    /// Updates the game
    /// </summary>
    /// <param name="gameTime">An object representing time in the game</param>
    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        // Move each ball
        foreach (var ball in balls) ball.Update(gameTime);

        // Detect any collisions
        for (int i = 0; i < balls.Count; i++) {
            for(int j = i + 1; j < balls.Count; j++)
            {
                if (balls[i].CollidesWith(balls[j]))
                {
                    balls[i].Colliding = true;
                    balls[j].Colliding = true;

                    // TODO: Handle collisions

                }
            }
        } 

        base.Update(gameTime);
    }

    /// <summary>
    /// Draws the game
    /// </summary>
    /// <param name="gameTime">An object representing time in the game</param>
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        spriteBatch.Begin();
        foreach (var ball in balls) ball.Draw(gameTime, spriteBatch);
        spriteBatch.End();

        base.Draw(gameTime);
    }
}