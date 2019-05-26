using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2.Core
{
    public static class Collision
    {
        public enum Direction
        {
            NONE = -1,
            LEFT = 0,
            RIGHT = 1,
            TOP = 2,
            BOTTOM = 3
        }

        // méthode sert à récupérer la couleur d'un pixel à une position donnée
        private static Color GetColorAt(GameObject gameObject, World world)
        {
            // Nous commençons par ajouter une variable de type Color que nous initialisons avec la couleur de collision 
            // qui est stockée dans notre objet world. Ensuite premier test qui va permettre d'éviter des erreurs
            // de dépassement d'indice. On détermine donc si on se trouve bien à l'intérieur de la fenêtre avant de faire quoi que
            // ce soit. Ensuite, prenons l'exemple du premier case dans le switch, nous recherchons le pixel qui correspond 
            // au point de collision par rapport à la direction en cours. Ici c'est la direction droite.
            Color color = world.collisionColor;

            if ((int)gameObject.Position.X >= 0 && (int)gameObject.Position.X < world.Texture.Width
                && (int)gameObject.Position.Y >= 0 && (int)gameObject.Position.Y < world.Texture.Height)
            {
                switch (gameObject.direction)
                {
                    case Direction.RIGHT:
                        {
                            color = world.colorTab[((int)gameObject.Position.X + gameObject.frameWidth) + ((int)gameObject.Position.Y + (gameObject.frameHeight / 2)) * world.Texture.Width];
                        }
                        break;
                    case Direction.LEFT:
                        {
                            color = world.colorTab[(int)gameObject.Position.X + ((int)gameObject.Position.Y + (gameObject.frameHeight / 2)) * world.Texture.Width];
                        }
                        break;
                    case Direction.BOTTOM:
                        {
                            color = world.colorTab[((int)gameObject.Position.X + (gameObject.frameWidth / 2)) + ((int)gameObject.Position.Y + gameObject.frameHeight) * world.Texture.Width];
                        }
                        break;
                    case Direction.TOP:
                        {
                            color = world.colorTab[((int)gameObject.Position.X + (gameObject.frameWidth / 2)) + (int)gameObject.Position.Y * world.Texture.Width];
                        }
                        break;
                }
            }

            return color;
        }
        // méthode qui permettra de déterminer si notre personnage est en collision 
        //et en conséquence il sera nécessaire de changer d'orientation

        public static bool Collided(GameObject gameObject, World world)
        {
            bool b = false;
            Color color = GetColorAt(gameObject, world);

            if (color != world.collisionColor)
                b = false;
            else
                b = true;

            return b;
        }
    }
}
