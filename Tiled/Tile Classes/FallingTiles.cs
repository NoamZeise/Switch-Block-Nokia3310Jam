using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nokia3310Jam.Tiled.Tile_Classes
{
    public class FallingTile
    {
        public Texture2D Texture;
        public Vector2 Position;
        public bool activated = false;
        public bool moving = false;

        private float _timer;
        private float _timeKeeper = 0;
        private Vector2 _originalPos;
        private Vector2 _velocity;
        private Vector2 _acceleration = new Vector2(0, 1f);

        public Rectangle oldRect;
        public Rectangle Rectangle;

        public FallingTile(Texture2D texture, Vector2 position, float timer)
        {
            Texture = texture;
            Position = position;
            _originalPos = position;
            _timer = timer;
            _velocity = new Vector2(0, 0);

            oldRect = Rectangle;
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
                
        }
        public void Update(GameTime gameTime)
        {
           if (!activated)
                return;
            _timeKeeper += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(_timeKeeper > _timer)
            {
                moving = true;
                _velocity += _acceleration;
                Position += Vector2.Multiply(_velocity, (float)gameTime.ElapsedGameTime.TotalSeconds);
                oldRect = Rectangle;
                Rectangle.X = (int)Position.X;
                Rectangle.Y = (int)Position.Y;
                
            }
            if(_timeKeeper > _timer + 2)
            {
                Reset();
            }
        }

        public void Reset()
        {
            Position = _originalPos;
            activated = false;
            _timeKeeper = 0;
            _velocity = Vector2.Zero;
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, float depth)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, depth);
        }

    }
}
