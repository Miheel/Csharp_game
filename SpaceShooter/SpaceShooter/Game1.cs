using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SpaceShooter
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont myFont;
        HighScore highScore;
        int points = 0;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IsMouseVisible = true;
            highScore = new HighScore(10);
            GameElements.currentState = GameElements.state.Menu;
            GameElements.Initialize();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            highScore.LoadFromFile("highscore.txt");
            myFont = Content.Load<SpriteFont>("myFont");
            GameElements.LoadContent(Content, Window);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            highScore.SaveToFile("highscore.txt");
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
             if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            switch (GameElements.currentState)
            {
                case GameElements.state.Run:
                    GameElements.currentState = GameElements.RunUpdate(Content, Window, gameTime);
                    break;
                case GameElements.state.EnterHighScore:
                    GameElements.currentState = GameElements.EnterUpdate(gameTime, points);
                    break;
                case GameElements.state.PrintHighScore:
                    GameElements.currentState = GameElements.printUpdate();
                    KeyboardState keyboardState = Keyboard.GetState();
                    if (keyboardState.IsKeyDown(Keys.E))
                        GameElements.currentState = GameElements.state.EnterHighScore;
                    break;
                case GameElements.state.Quit:
                    this.Exit();
                    break;
                default:
                    GameElements.currentState = GameElements.MenuUpdate(gameTime);
                    break;
            }
            base.Update(gameTime);
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here
            spriteBatch.Begin();
            switch (GameElements.currentState)
            {
                case GameElements.state.Run:
                    GameElements.RunDraw(spriteBatch);
                    break;
                case GameElements.state.EnterHighScore:
                    GameElements.EnterDraw(spriteBatch, myFont);
                    break;
                case GameElements.state.PrintHighScore:
                    GameElements.PrintDraw(spriteBatch, myFont);
                    break;
                case GameElements.state.Quit:
                    this.Exit();
                    break;
                default:
                    GameElements.MenuDraw(spriteBatch);
                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
