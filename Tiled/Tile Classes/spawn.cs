using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nokia3310Jam.Tiled.Tile_Classes
{
    public class spawn
    {
        public Rectangle Rectangle;
        public int spriteIndex;

        public spawn(Rectangle rect, int index)
        {
            Rectangle = rect;
            spriteIndex = index;
        }
    }
}
