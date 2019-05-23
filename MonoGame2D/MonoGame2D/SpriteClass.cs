using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame2D
{
    class SpriteClass
    {
        private Stream stream;
        const float HITBOXSCALE = .5f; // hitbox réduit de moitié

        public Texture2D texture
        {
            get;
        }
        // position actuelle du sprite
        public float x
        {
            get; set;
        }
        // position actuelle du sprite
        public float y
        {
            get; set;
        }
        // angle actuel du sprite en degrés (0 étant vertical et 90 représentant une inclinaison de 90 degrés vers la droite).
        public float angle
        {
            get; set;
        }
        // taux des changements par seconde des variables (x)
        public float dX
        {
            get; set;
        }
        // taux des changements par seconde des variables (y)
        public float dY
        {
            get; set;
        }
        // taux des changements par seconde des variables (angle)
        public float dA
        {
            get; set;
        }

        public float scale
        {
            get; set;
        }

        public SpriteClass(GraphicsDevice graphicsDevice, string textureName, float scale)
        {
            this.scale = scale;

            // Load the specified texture
            var stream = TitleContainer.OpenStream(textureName);
            texture = Texture2D.FromStream(graphicsDevice, stream);
        }

        // méthode qui sert à mettre à jour les valeurs des sprites en fonction de leurs modifications.
        public void Update(float elapsedTime)
        {
            this.x += this.dX * elapsedTime;
            this.y += this.dY * elapsedTime;
            this.angle += this.dA * elapsedTime;
        }
        // méthode qui sert à dessiner le sprite dans la fenêtre de jeu
        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 spritePosition = new Vector2(this.x, this.y);
            spriteBatch.Draw(texture, spritePosition, null, Color.White, this.angle, new Vector2(texture.Width / 2, texture.Height / 2), new Vector2(scale, scale), SpriteEffects.None, 0f);
        }

        // méthode qui détecte sur les objets se sont heurtés.
        public bool RectangleCollision(SpriteClass otherSprite)
        {
            if (this.x + this.texture.Width * this.scale * HITBOXSCALE / 2 < otherSprite.x - otherSprite.texture.Width * otherSprite.scale / 2)
                return false;
            if (this.y + this.texture.Height * this.scale * HITBOXSCALE / 2 < otherSprite.y - otherSprite.texture.Height * otherSprite.scale / 2)
                return false;
            if (this.x - this.texture.Width * this.scale * HITBOXSCALE / 2 > otherSprite.x + otherSprite.texture.Width * otherSprite.scale / 2)
                return false;
            if (this.y - this.texture.Height * this.scale * HITBOXSCALE / 2 > otherSprite.y + otherSprite.texture.Height * otherSprite.scale / 2)
                return false;
            return true;
        }
    }
}
