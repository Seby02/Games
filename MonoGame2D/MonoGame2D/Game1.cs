using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;

namespace MonoGame2D
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        // La variable SKYRATIO nous indique quelle part de la scène nous voulons accorder au ciel par rapport à l’herbe : ici, les deux tiers.
        const float SKYRATIO = 2f / 3f;
        float screenWidth;
        float screenHeight;
        Texture2D grass;
        SpriteClass dino;
        SpriteClass broccoli;

        //position de la barre d’espace pour déterminer si elle maintenue enfoncée ou enfoncée, puis relâchée.
        bool spaceDown;

        Texture2D gameOverTexture;
        bool gameOver;

        //prévient si c'est la 1ere fois que l'user a démarré le jeu
        bool gameStarted;

        //détermine la vitesse de déplacement de l’obstacle brocoli sur l’écran.
        float broccoliSpeedMultiplier;

        //détermine la vitesse à laquelle l’avatar du joueur accélère vers le bas après un saut.
        float gravitySpeed;

        //déterminent la vitesse avec laquelle l’avatar du joueur se déplace et saute
        float dinoSpeedX;
        float dinoJumpY;

        float score;
        Random random;

        Texture2D startGameSplash;
        SpriteFont scoreFont;
        SpriteFont stateFont;

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
            // mode de fenêtre de l'application en fullscreen
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.FullScreen;

            screenHeight = ScaleToHighDPI((float)ApplicationView.GetForCurrentView().VisibleBounds.Height);
            screenWidth = ScaleToHighDPI((float)ApplicationView.GetForCurrentView().VisibleBounds.Width);

            this.IsMouseVisible = false;

            broccoliSpeedMultiplier = 0.5f;
            spaceDown = false;
            gameStarted = false;
            score = 0;
            random = new Random();
            dinoSpeedX = ScaleToHighDPI(1000f);
            dinoJumpY = ScaleToHighDPI(-1200f);
            gravitySpeed = ScaleToHighDPI(30f);
            gameOver = false;

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

            // TODO: use this.Content to load your game content here
            grass = Content.Load<Texture2D>("grass");
            dino = new SpriteClass(GraphicsDevice, "Content/ninja-cat-dino.png", ScaleToHighDPI(1f));
            // réduction de l'image du brocoli à 0,2 fois sa taille d'origine
            broccoli = new SpriteClass(GraphicsDevice, "Content/broccoli.png", ScaleToHighDPI(0.2f));

            startGameSplash = Content.Load<Texture2D>("start-splash");
            scoreFont = Content.Load<SpriteFont>("Score");
            stateFont = Content.Load<SpriteFont>("GameState");
            gameOverTexture = Content.Load<Texture2D>("game-over");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here

            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardHandler(); // Handle keyboard input

            // // Stop all movement when the game ends
            if (gameOver)
            {
                dino.dX = 0;
                dino.dY = 0;
                broccoli.dX = 0;
                broccoli.dY = 0;
                broccoli.dA = 0;
            }
            // Update animated SpriteClass objects based on their current rates of change
            dino.Update(elapsedTime);
            broccoli.Update(elapsedTime);

            // Accelerate the dino downward each frame to simulate gravity.
            dino.dY += gravitySpeed;

            // Set game floor so the player does not fall through it
            if (dino.y > screenHeight * SKYRATIO)
            {
                dino.dY = 0;
                dino.y = screenHeight * SKYRATIO;
            }

            // Set game edges to prevent the player from moving offscreen
            if (dino.x > screenWidth - dino.texture.Width / 2)
            {
                dino.x = screenWidth - dino.texture.Width / 2;
                dino.dX = 0;
            }
            if (dino.x < 0 + dino.texture.Width / 2)
            {
                dino.x = 0 + dino.texture.Width / 2;
                dino.dX = 0;
            }

            // If the broccoli goes offscreen, spawn a new one and iterate the score
            if (broccoli.y > screenHeight + 100 || broccoli.y < -100 || broccoli.x > screenWidth + 100 || broccoli.x < -100)
            {
                SpawnBroccoli();
                score++;
            }
            // Cela appelle la méthode RectangleCollision que nous avons créée dans SpriteClasset marque le jeu comme étant terminé si elle retourne true.
            if (dino.RectangleCollision(broccoli)) gameOver = true;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            /* Ici, nous utilisons la méthode spriteBatch.Draw pour placer la texture donnée dans les limites d’un objet Rectangle. 
 * Un Rectangle est défini par les coordonnées x et y de ses angles supérieur gauche et inférieur droit. 
 * À l’aide des variables screenWidth, screenHeight et SKYRATIO; nous dessinons la texture du rectangle sur le tiers inférieur de l’écran.
*/
            spriteBatch.Draw(grass, new Rectangle(0, (int)(screenHeight * SKYRATIO),(int)screenWidth, (int)screenHeight), Color.White);

            if (gameOver)
            {
                // Draw game over texture
                spriteBatch.Draw(gameOverTexture, new Vector2(screenWidth / 2 - gameOverTexture.Width / 2, screenHeight / 4 - gameOverTexture.Width / 2), Color.White);

                String pressEnter = "Appuyez sur Entree pour recommencer !";

                // Measure the size of text in the given font
                Vector2 pressEnterSize = stateFont.MeasureString(pressEnter);

                // Draw the text horizontally centered
                spriteBatch.DrawString(stateFont, pressEnter, new Vector2(screenWidth / 2 - pressEnterSize.X / 2, screenHeight - 200), Color.White);
            }

            broccoli.Draw(spriteBatch);
            dino.Draw(spriteBatch);

            // utilise la description de sprite que nous avons créée (Arial taille 36) pour tirer le score actuel du joueur vers le coin supérieur droit de l’écran.
            spriteBatch.DrawString(scoreFont, score.ToString(),
            new Vector2(screenWidth - 100, 50), Color.Black);

            if (!gameStarted)
            {
                // Fill the screen with black before the game starts
                spriteBatch.Draw(startGameSplash, new Rectangle(0, 0,
                (int)screenWidth, (int)screenHeight), Color.White);

                String title = "DinoChat deteste les brocolis ";
                String pressSpace = "Appuyez sur Espace pour commencer ";

                // Mesure la largeur et la hauteur de chaque ligne une fois imprimée, à l’aide de la méthode SpriteFont.MeasureString(String). 
                // Cela nous donne la taille en tant qu'objet Vector2, avec la propriété X contenant sa largeur, et Y sa hauteur.
                Vector2 titleSize = stateFont.MeasureString(title);
                Vector2 pressSpaceSize = stateFont.MeasureString(pressSpace);

                // Draw the text horizontally centered
                spriteBatch.DrawString(stateFont, title,
                new Vector2(screenWidth / 2 - titleSize.X / 2, screenHeight / 3),
                Color.ForestGreen);
                spriteBatch.DrawString(stateFont, pressSpace,
                new Vector2(screenWidth / 2 - pressSpaceSize.X / 2,
                screenHeight / 2), Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        // Scale a number of pixels so that it displays properly on a High-DPI screen, such as a Surface Pro or Studio
        public float ScaleToHighDPI(float f)
        {
            DisplayInformation d = DisplayInformation.GetForCurrentView();
            f *= (float)d.RawPixelsPerViewPixel;
            return f;
        }

        public void SpawnBroccoli()
        {
            // La première partie de la méthode détermine le point de l'écran où sera généré l’objet brocoli, en utilisant pour cela deux nombres aléatoires.
            int direction = random.Next(1, 5);
            switch (direction)
            {
                case 1:
                    broccoli.x = -100;
                    broccoli.y = random.Next(0, (int)screenHeight);
                    break;
                case 2:
                    broccoli.y = -100;
                    broccoli.x = random.Next(0, (int)screenWidth);
                    break;
                case 3:
                    broccoli.x = screenWidth + 100;
                    broccoli.y = random.Next(0, (int)screenHeight);
                    break;
                case 4:
                    broccoli.y = screenHeight + 100;
                    broccoli.x = random.Next(0, (int)screenWidth);
                    break;
            }
            // La deuxième partie détermine la vitesse à laquelle le brocoli se déplace, ce qui est déterminé par le score du moment. 
            // Il accélérera chaque fois que le joueur aura réussi à en éviter cinq.

            if (score % 5 == 0) broccoliSpeedMultiplier += 0.2f;

            // La troisième partie définit la direction du mouvement du sprite brocoli. 
            //Il pointe dans la direction de l’avatar du joueur (dino) lorsque le brocoli est généré. 
            // Nous lui attribuons également une valeur dA de 7f qui fait tournoyer le brocoli dans les airs en poursuivant le joueur.
            broccoli.dX = (dino.x - broccoli.x) * broccoliSpeedMultiplier;
            broccoli.dY = (dino.y - broccoli.y) * broccoliSpeedMultiplier;
            broccoli.dA = 7f;
        }

        // Start a new game, either when the app starts up or after game over
        public void StartGame()
        {
            // Reset dino position
            dino.x = screenWidth / 2;
            dino.y = screenHeight * SKYRATIO;

            // Reset broccoli speed and respawn it
            broccoliSpeedMultiplier = 0.5f;
            SpawnBroccoli();

            score = 0; // Reset score
        }

        void KeyboardHandler()
        {
            KeyboardState state = Keyboard.GetState();

            // Quit the game if Escape is pressed.
            if (state.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // Start the game if Space is pressed.
            if (!gameStarted)
            {
                if (state.IsKeyDown(Keys.Space))
                {
                    StartGame();
                    gameStarted = true;
                    spaceDown = true;
                    gameOver = false;
                }
                return;
            }
            // autorise l’utilisateur de réinitialiser le jeu si elle appuye sur ENTRÉE :
            if (gameOver && state.IsKeyDown(Keys.Enter))
            {
                StartGame();
                gameOver = false;
            }

            // Jump if Space is pressed
            if (state.IsKeyDown(Keys.Space) || state.IsKeyDown(Keys.Up))
            {
                // Jump if the Space is pressed but not held and the dino is on the floor
                if (!spaceDown && dino.y >= screenHeight * SKYRATIO - 1) dino.dY = dinoJumpY;

                spaceDown = true;
            }
            else spaceDown = false;

            // Handle left and right
            if (state.IsKeyDown(Keys.Left)) dino.dX = dinoSpeedX * -1;

            else if (state.IsKeyDown(Keys.Right)) dino.dX = dinoSpeedX;
            else dino.dX = 0;
        }
    }
}
