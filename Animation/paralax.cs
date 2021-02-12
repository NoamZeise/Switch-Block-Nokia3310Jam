using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nokia3310Jam.Animation
{
    class paralax
    {
        Vector2 Position;
        protected Texture2D _texture;
        float _speed;
        float _depth;
        public bool Absolute;
        
        public paralax(Vector2 pos, Texture2D tex, float speed, float depth, bool absolute)
        {
            Position = pos;
            _texture = tex;
            _speed = speed;
            _depth = depth;
            Absolute = absolute;
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, bool paused)
        {
            if (!paused)
            {
                Position.X += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (Math.Abs(Position.X) > _texture.Width)
                    Position.X = 0;
            }
            spriteBatch.Draw(_texture, Position, null, Color.White, 0f, Vector2.Zero,1f, SpriteEffects.None, _depth);
            spriteBatch.Draw(_texture, new Vector2(Position.X +_texture.Width, Position.Y), null, Color.White, 0f, Vector2.Zero, 1f,SpriteEffects.None, _depth);
            spriteBatch.Draw(_texture, new Vector2(Position.X - _texture.Width, Position.Y), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, _depth);
        }

    }
}
