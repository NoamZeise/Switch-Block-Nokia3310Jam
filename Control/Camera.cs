using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nokia3310Jam;
using Nokia3310Jam.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiled;

namespace Control.Camera2D
{
    public class Camera
    {
        GraphicsDevice _graphicsDevice;
        GraphicsDeviceManager _graphics;


        public Rectangle ScreenRectangle;
        public RenderTarget2D RenderTarget;
        public Matrix Translation;
        public Matrix Transform { get; private set; }

        private float xMin = Game1.SCREEN_WIDTH / 2;
        private float yMin = Game1.SCREEN_HEIGHT / 2;
        private float xMax;
        private float yMax;
        private float targetX;
        private float targetY;

        private float transformX;
        private float transformY;

        float scale = 1f;
        int _targetWidth
        {
            get
            {
                return RenderTarget.Width;
            }
        }

        int _targetHeight
        {
            get
            {
                return RenderTarget.Height;
            }
        }
        Vector2 camera = new Vector2(0, 0);
        public Camera(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics, RenderTarget2D renderTarget)
        {
            _graphics = graphics;
            _graphicsDevice = graphicsDevice;
            RenderTarget = renderTarget;

            Translation = Matrix.CreateTranslation(0, 0, 0);
            scale = 1f;
            SetScale(scale);
        }
        public void UpdateTarget()
        {
            SetScale(scale);
        }

        private Rectangle setScreenRectangle(int screenWidth, int screenHeight, int targetWidth, int targetHeight)
        {
            /*
            if (screenWidth > screenHeight || targetHeight > targetWidth)
                return new Rectangle((screenWidth - (int)((float)screenHeight * ((float)targetWidth / (float)targetHeight))) / 2, 0, (int)((float)screenHeight * ((float)targetWidth / (float)targetHeight)), screenHeight);
            if (screenHeight > screenWidth || targetWidth > targetHeight)
                return new Rectangle(0, (screenHeight - (int)((float)screenWidth * ((float)targetHeight / (float)targetWidth))) / 2, screenWidth, (int)((float)screenWidth * ((float)targetHeight / (float)targetWidth)));
            return new Rectangle(0, 0, screenWidth, screenHeight);
            */
            double widthRatio, heightRatio;
            widthRatio = (double)screenWidth / (double)targetWidth;
            heightRatio = (double)screenHeight / (double)targetHeight;
            double ratio = widthRatio;
            if (widthRatio > heightRatio)
                ratio = heightRatio;
            double topOffset, sideOffset;
            topOffset = ((double)screenHeight - ((double)targetHeight * ratio)) / 2;
            sideOffset = ((double)screenWidth - ((double)targetWidth * ratio)) / 2;
            return new Rectangle((int)sideOffset, (int)topOffset, (int)((double)targetWidth * ratio), (int)((double)targetHeight * ratio));
        }


        public void SetScale(float scale)
        {
            //Translation = Matrix.CreateTranslation(((float)(_targetWidth * scale) - _targetWidth) / 2f, ((float)(_targetHeight * scale) - _targetWidth) / 2f, 0) *
            //   Matrix.CreateTranslation(-camera.X, -camera.Y, 0);
            RenderTarget = new RenderTarget2D(_graphicsDevice, (int)(_targetWidth * scale), (int)(_targetHeight * scale));
            ScreenRectangle = setScreenRectangle(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight, RenderTarget.Width, RenderTarget.Height);

        }
        public void SetOffset(Vector2 offset)
        {
            camera = offset;
        }
        public void Follow(Sprite target, Map map)
        {
            xMax = map.Width * map.TileWidth - Game1.SCREEN_WIDTH / 2;
            yMax = map.Height * map.TileHeight - Game1.SCREEN_HEIGHT / 2;
            targetX = (int)target.Position.X + target.Rectangle.Width / 2;
            targetY = (int)target.Position.Y + target.Rectangle.Height / 2;


            Matrix position = new Matrix();
            setX(target, map);
            setY(target, map);

            if (Game1.SCREEN_HEIGHT > map.Height * map.TileHeight)
            {

                transformY = -Game1.SCREEN_HEIGHT / 2 + (Game1.SCREEN_HEIGHT - map.Height * map.TileHeight) / 2;
            }
            if (Game1.SCREEN_WIDTH > map.Width * map.TileWidth)
            {
                transformX = -Game1.SCREEN_WIDTH / 2 + (Game1.SCREEN_WIDTH - map.Width * map.TileWidth) / 2;
            }

            position = Matrix.CreateTranslation(
                    transformX,
                    transformY, 0);

            Matrix offset = Matrix.CreateTranslation( //translation matrix, half screen height and width, otherwise camera centred on to left corner
                    Game1.SCREEN_WIDTH / 2,
                    Game1.SCREEN_HEIGHT / 2,
                    0);
            Transform = position * offset;

        }

        private void setX(Sprite target, Map map)
        {
            if (targetX > xMin &&
                targetX < xMax)
            {
                transformX = -target.Position.X - (target.Rectangle.Width / 2);
            }
            else if (targetX < xMax)
            {
                transformX = -xMin;
            }
            else if (targetX > xMin)
            {
                transformX = -(map.Width * map.TileWidth) + Game1.SCREEN_WIDTH / 2;
            }
        }
        private void setY(Sprite target, Map map)
        {
           // Console.WriteLine(target.Y);
            if (targetY > yMin &&
                targetY < yMax)
            {
                transformY = -target.Position.Y - (target.Rectangle.Height / 2);
            }
            else if (targetY < yMax)
            {
                
                transformY = -yMin;
            }
            else if (targetY > yMin)
            {
                transformY = -(map.Height * map.TileHeight) + Game1.SCREEN_HEIGHT / 2;
            }
            else
            {
                transformY = -yMin;
            }
        }
    }
}

/*
 * EXAMPLE UPDATE FUNCTION
 * 
        protected override void Update(GameTime gameTime)
        {
           
            _camera.Follow(location);
            _camera.SetScale(scale);
        }


//////EXAMPLE DRAW FUNCTION

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_camera.RenderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _camera.Translation);

            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(_camera.RenderTarget, _camera.ScreenRectangle, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
        */
