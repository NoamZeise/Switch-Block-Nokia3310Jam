using Control.InputClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nokia3310Jam.Sprites
{
    class Player : Sprite
    {
        InputState previousState;
        SoundEffect _jump;
        Rectangle sprite;
        bool muted = false;
        public Player(Texture2D texture, Vector2 position, SoundEffect jump) : base(texture, position)
        {
            sprite = new Rectangle(0, 0, texture.Width / 3, texture.Height);
            _framesOnSheet = 3;
            _jump = jump;
    
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            float addxVel = 0;
            if (Input.Left) //left
            {
                addxVel += -50;
            }
            if(Input.Right) //right
            {
                addxVel += 50;
            }
            Velocity.X = addxVel;
            if(Input.Up && Grounded) //up
            {
                Velocity.Y = -90;
                if (!muted)
                {
                    _jump.Play(0.3f, 0f, 0f);
                }
            }
            if (Input.Mute && !previousState.Mute)
                muted = !muted;

            else if (!Input.Up && !Grounded && Velocity.Y < 0) //if not holding up fall down
                Velocity.Y += 10;


            previousState = Input.GetState();
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Grounded) //jump animation
            {
                if (Velocity.Y < 0)
                {
                    sprite.X = sprite.Width;
                }
                if (Velocity.Y > 0)
                {
                    sprite.X = sprite.Width * 2;
                }
            }
            else if (sprite.X != 0)
                sprite.X = 0;


            spriteBatch.Draw(_texture, Position, sprite, Color.White, 0f, Vector2.Zero, 1f,SpriteEffects.None, _Depth );
        }
    }
}
