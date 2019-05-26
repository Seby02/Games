using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Game2.Core
{
    public class World : GameObject
    {
        public Color[] colorTab;

        private Color _collisionColor;
        public Color collisionColor
        {
            get { return _collisionColor; }
        }

        public World(Color collisionColor)
        {
            _collisionColor = collisionColor;
        }
    }
}
