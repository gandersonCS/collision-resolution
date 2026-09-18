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
                    balls[i].Center -= balls[i].Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    balls[i].Center -= balls[j].Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

                    Vector2 collisionAxis = balls[i].Center - balls[j].Center;
                    collisionAxis.Normalize();
                    float angle = (float)System.Math.Acos(Vector2.Dot(collisionAxis, Vector2. UnitX));

                    float m0 = balls[i].Mass;
                    float m1 = balls[j].Mass;

                    Vector2 u0 = Vector2.Transform(balls[i].Velocity, Matrix.CreateRotationZ(angle));
                    Vector2 u1 = Vector2.Transform(balls[j].Velocity, Matrix.CreateRotationZ(angle));

                    Vector2 v0;
                    Vector2 v1;
                    v0.X = ((m0 - m1) / (m0 + m1)) * u0.X + ((2 * m1) / (m0 + m1)) * u1.X;
                    v1.X = ((2 * m0) / (m0 + m1)) * u0.X + ((m1 - m0) / (m0 + 1)) * u1.X;
                    v0.Y = u0.Y;
                    v1.Y = u1.Y;

                    balls[i].Velocity = Vector2.Transform(v0, Matrix.CreateRotationZ(-angle));
                    balls[j].Velocity = Vector2.Transform(v1, Matrix.CreateRotationZ(-angle));

                    balls[i].Center -= balls[i].Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    balls[i].Center -= balls[j].Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;





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