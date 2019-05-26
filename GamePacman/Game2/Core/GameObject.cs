using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2.Core
{
    public class GameObject
    {
        public World world;
        private int _totalFrames;
        private int _frameHeight;
        private int _frameWidth;
        public Collision.Direction direction;
        


        public GameObject()
        {
        }

        public GameObject(int totalAnimationFrames, int frameWidth, int frameHeight, World world)
        {
            _totalFrames = totalAnimationFrames;
            _frameWidth = frameWidth;
            _frameHeight = frameHeight;
            this.world = world;
        }

        public Vector2 Position;
        public Texture2D Texture;

        // Rectangle permettant de définir la zone de l'image à afficher
        public Rectangle Source;
        // Durée depuis laquelle l'image est à l'écran
        public float time;
        // Durée de visibilité d'une image
        public float frameTime = 0.1f;
        // Indice de l'image en cours

        public framesIndex frameIndex;

        public enum framesIndex
        {
            RIGHT_1 = 0,
            RIGHT_2 = 1,
            BOTTOM_1 = 2,
            BOTTOM_2 = 3,
            LEFT_1 = 4,
            LEFT_2 = 5,
            TOP_1 = 6,
            TOP_2 = 7
        }


        /* Méthode dans laquelle nous appelons la méthode Draw de notre argument. 
         * Le dernier argument qu'on passe est une couleur qui permet de modifier la teinture de l'image. 
         * Nous utilisons ici la couleur blanche pour ne pas modifier notre image. 
         * spriteBatch permet de dessiner à l'écran des textures, 2D dans notre cas.
         * */
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }

        public void DrawAnimation(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Source, Color.White);
        }

        public void UpdateFrame(GameTime gameTime)
        {
            //permet de calculer le temps passé depuis notre dernière mise à jour de l'affichage de notre sprite
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // prochain indice de l'image si nous avons dépassé le temps d'affichage puis, on remet à zéro 
            // la variable time pour la réutiliser correctement pour la prochaine mise à jour.
            while (time > frameTime)
            {
                switch (direction)
                {
                    case Collision.Direction.TOP:
                        if (frameIndex == framesIndex.TOP_1)
                            frameIndex = framesIndex.TOP_2;
                        else
                            frameIndex = framesIndex.TOP_1;
                        break;
                    case Collision.Direction.LEFT:
                        if (frameIndex == framesIndex.LEFT_1)
                            frameIndex = framesIndex.LEFT_2;
                        else
                            frameIndex = framesIndex.LEFT_1;
                        break;
                    case Collision.Direction.BOTTOM:
                        if (frameIndex == framesIndex.BOTTOM_1)
                            frameIndex = framesIndex.BOTTOM_2;
                        else
                            frameIndex = framesIndex.BOTTOM_1;
                        break;
                    case Collision.Direction.RIGHT:
                        if (frameIndex == framesIndex.RIGHT_1)
                            frameIndex = framesIndex.RIGHT_2;
                        else
                            frameIndex = framesIndex.RIGHT_1;
                        break;
                }
                time = 0f;
            }
            // Si l'indice dépasse le nombre de sprites dans notre collection, on repasse au premier. 

            //calcul de la position du nouveau sprite à afficher en déterminant sa position par rapport à l'indice en cours
            Source = new Rectangle((int)frameIndex * frameWidth,0,frameWidth,frameHeight);
        }



        
        public int totalFrames
        {
            get { return _totalFrames; }
        }
        
        public int frameWidth
        {
            get { return _frameWidth; }
        }
        
        public int frameHeight
        {
            get { return _frameHeight; }
        }
    }


}


