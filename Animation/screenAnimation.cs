using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nokia3310Jam.Animation
{
    class screenAnimation : paralax
    {
        double _changeTimer = 0;
        double _changeDelay;
        List<Texture2D> _textureList;
        int currentTex = 0;
        public screenAnimation(Vector2 pos, List<Texture2D> texList, double changeDelay, float depth, bool absolute) : base(pos, texList[0], 0, depth, absolute)
        {
            _changeDelay = changeDelay;
            _textureList = texList;
        }

        public void Reset()
        {
            currentTex = 0;
            _texture = _textureList[currentTex];
            _changeTimer = 0;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, bool paused)
        {
            if (!paused)
            {
                _changeTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (_changeTimer > _changeDelay)
                {
                    _changeTimer = 0;
                    currentTex++;
                    if (currentTex >= _textureList.Count)
                        currentTex = 0;
                    _texture = _textureList[currentTex];
                }
            }


            base.Draw(gameTime, spriteBatch, paused);
        }
    }
}
