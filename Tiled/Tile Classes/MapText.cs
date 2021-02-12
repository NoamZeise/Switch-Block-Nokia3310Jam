using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiled.Tile_Classes
{
    public class MapText
    {
        private Vector2 _position;
        private string _text;

        public MapText(Vector2 position, string text)
        {
            _position = position;
            _text = text;
        }
        public void Draw(SpriteBatch spriteBatch, SpriteFont Font)
        {
            spriteBatch.DrawString(Font, _text, _position, Color.White);


        }
    }
}
