using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Game2.Core;

namespace GamePacman
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MyPacman : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        World world;
        Player player;

        public const int WINDOW_WIDTH = 224;
        public const int WINDOW_HEIGHT = 248;

        public MyPacman()
        {
            
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
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

            
            world = new World(new Color(0, 128, 248));

            // Dans l'ordre des arguments, il possède 8 images et fait 13 pixels de largeur et de hauteur.
            player = new Player(8, 13, 13, world);
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
            // chargement et positionnement 
            world.Texture = Content.Load<Texture2D>("world");
            world.Position = new Vector2(0, 0);
            // Cette méthode permet de créer en réalité un tableau à deux dimensions dans un tableau à une seule dimension
            world.colorTab = new Color[world.Texture.Width * world.Texture.Height];
            player.Texture = Content.Load<Texture2D>("pacman");
            player.Position = new Vector2(0, 109);

            // Création du tableau déclaré dans la classe World. Nous faisons d'un tableau à une dimension un tableau à deux dimensions en multipliant la largeur 
            // et la hauteur de l'image. Ce n'est pas très pratique à utiliser pour les accès aux indices. 
            // La seconde permet quant à elle d'initialiser le tableau grâce à la méthode GetData de la classe Texture.
            // Elle va récupérer les informations de chaque pixel et les stocker à l'endroit adéquat !
            world.colorTab = new Color[world.Texture.Width * world.Texture.Height];
            world.Texture.GetData<Color>(world.colorTab);
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;

            // état du clavier 
            player.Move(Keyboard.GetState());

            player.UpdateFrame(gameTime);
            
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
            spriteBatch.Begin();
            world.Draw(spriteBatch);
            player.DrawAnimation(spriteBatch);
            spriteBatch.End();
            

            base.Draw(gameTime);
        }
    }
}
