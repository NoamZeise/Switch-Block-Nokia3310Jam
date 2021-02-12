using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collision_and_Camera_Testing.Tiled.Tile_Classes
{
    public class BounceTile
    {
        public float Bounce;
        public Rectangle Rectangle;

        public BounceTile(float bounce, Rectangle rectangle)
        {
            Bounce = bounce;
            Rectangle = rectangle;
        }
    }
}
