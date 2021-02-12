using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nokia3310Jam.Sprites
{
    public abstract class Sprite
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public bool Grounded = false;
        public bool IsRemoved = false;
        protected Texture2D _texture;
        protected int _framesOnSheet;
        protected float _Depth = 0.7f;

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (_texture.Width / _framesOnSheet) + 1, _texture.Height + 1);
            }
        }
        public Sprite(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            Position = position;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (Grounded)
            {
                Velocity.Y = 0;
                Velocity.X /= 2;
            }
            else
            {
                Velocity.X /= 1.1f;
            }
            Velocity.Y += 5;
        }

        public virtual void Draw( GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, _Depth);
        }

    }
}
