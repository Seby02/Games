using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Game2.Core.Collision;

namespace Game2.Core
{
    public class Player : GameObject
    {
        // Cette propriété servira à pouvoir changer de direction tout en étant en collision dans une autre
        private Collision.Direction _collidedDirection;
        public Collision.Direction collidedDirection
        {
            get { return _collidedDirection; }
            set { _collidedDirection = value; }
        }

        // appel du constructeur parent GameObject
        public Player(int totalAnimationFrames, int frameWidth, int frameHeight, World world)
            : base(totalAnimationFrames, frameWidth, frameHeight, world)
        {
            direction = Collision.Direction.RIGHT;
            frameIndex = framesIndex.RIGHT_1;
            _collidedDirection = Collision.Direction.NONE;
        }

        public Player()
        {
            // image de départ 
            direction = Direction.RIGHT;
            framesIndex frameIndex = framesIndex.RIGHT_1;

            Source = new Rectangle((int)frameIndex * frameWidth,0,frameWidth,frameHeight);

            // déterminer à quel indice d'image correspond le frameIndex et nous ajustons la valeur en fonction du résultat
            switch (direction)
            {
                case Direction.TOP:
                    if (frameIndex == framesIndex.TOP_1)
                        frameIndex = framesIndex.TOP_2;
                    else
                        frameIndex = framesIndex.TOP_1;
                    break;
                case Direction.LEFT:
                    if (frameIndex == framesIndex.LEFT_1)
                        frameIndex = framesIndex.LEFT_2;
                    else
                        frameIndex = framesIndex.LEFT_1;
                    break;
                case Direction.BOTTOM:
                    if (frameIndex == framesIndex.BOTTOM_1)
                        frameIndex = framesIndex.BOTTOM_2;
                    else
                        frameIndex = framesIndex.BOTTOM_1;
                    break;
                case Direction.RIGHT:
                    if (frameIndex == framesIndex.RIGHT_1)
                        frameIndex = framesIndex.RIGHT_2;
                    else
                        frameIndex = framesIndex.RIGHT_1;
                    break;
            }
        }

        // Paramétrage des touches du clavier
        public void Move(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.Z))
            {
                direction = Collision.Direction.TOP;

                if (!Collision.Collided(this, world))
                {
                    if (collidedDirection != Collision.Direction.TOP)
                    {
                        collidedDirection = Collision.Direction.NONE;
                        Position.Y -= 1;
                    }
                }
            }
            if (state.IsKeyDown(Keys.Q))
            {
                direction = Collision.Direction.LEFT;

                if (!Collision.Collided(this, world))
                {
                    if (collidedDirection != Collision.Direction.LEFT)
                    {
                        collidedDirection = Collision.Direction.NONE;
                        Position.X -= 1;
                    }
                }
            }
            if (state.IsKeyDown(Keys.S))
            {
                direction = Collision.Direction.BOTTOM;

                if (!Collision.Collided(this, world))
                {
                    if (collidedDirection != Collision.Direction.BOTTOM)
                    {
                        collidedDirection = Collision.Direction.NONE;
                        Position.Y += 1;
                    }
                }
            }
            if (state.IsKeyDown(Keys.D))
            {
                // on assigne la direction droite à la touche D
                direction = Collision.Direction.RIGHT;

                // appel méthode statique Collided. Si elle renvoie false 0 en collision
                if (!Collision.Collided(this, world))
                {

                    //Si par exemple, on se dirige vers la droite, tout droit vers le mur, d'un coup on s'arrête.
                    //Pour pouvoir changer de direction, il faut détecter si la touche qu'on enfonce est différente 
                    //par rapport à la direction où on a réalisé notre récente collision. 
                    //!Avec un simple test, si on prend le test de la touche Q pour la gauche, 
                    //on teste si la touche est différente, on change notre variable de collision à sa valeur par défaut NONE 
                    //puis on décrémente notre position X vu que nous nous dirigeons vers la gauche 
                    if (collidedDirection != Collision.Direction.RIGHT)
                    {
                        collidedDirection = Collision.Direction.NONE;
                        Position.X += 1;
                    }
                }
            }
        }
    }
}
