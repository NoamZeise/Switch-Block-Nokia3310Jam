using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiled
{
    public class TiledObject
    {
        public string Name;
        public Rectangle Rectangle;
        public bool visible = true;
        public TiledObject(string name, int x, int y)
        {
            Name = name;
            Rectangle = new Rectangle(x, y, 1, 1);
        }
        public TiledObject(string name, int x, int y, int width, int height)
        {
            Name = name;
            Rectangle = new Rectangle(x, y, width, height);
        }
    }
}
