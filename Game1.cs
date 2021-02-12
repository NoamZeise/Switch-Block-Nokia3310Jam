using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Control.Camera2D;
using Control.InputClasses;
using Tiled;
using Nokia3310Jam.Sprites;
using System.Collections.Generic;
using Nokia3310Jam.Managers;
using System.IO;
using Nokia3310Jam.Animation;
using Microsoft.Xna.Framework.Audio;
using System;

namespace Nokia3310Jam
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static int SCREEN_WIDTH = 84;
        public static int SCREEN_HEIGHT = 48;
        Camera _camera;
        KeyboardState previousState;

        Color[] Palette = { new Color(199, 240, 216, 255), new Color(0x43523d) };


        List<Map> maps;
        int currentMap = 0;
        Player player;
        List<Sprite> sprites;

        List<paralax> paralaxScreens;
        screenAnimation controlsAnimation;
        screenAnimation pauseAnimation;
        paralax thanksScreen;

        double gameTimer = 0;
        bool isPaused = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 840;
            graphics.PreferredBackBufferHeight = 480;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            _camera = new Camera(GraphicsDevice, graphics, new RenderTarget2D(GraphicsDevice, SCREEN_WIDTH, SCREEN_HEIGHT));
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            sprites = new List<Sprite>();
            paralaxScreens = new List<paralax>();
            maps = new List<Map>();

            if(File.Exists("save"))
            {
                StreamReader sr = new StreamReader("save");
                currentMap = Convert.ToInt32(sr.ReadLine());
                gameTimer = Convert.ToDouble(sr.ReadLine());
                sr.Close();
            }
            else
            {
                currentMap = 0;
                gameTimer = 0;
            }
            int i = 0;
            while (File.Exists("Maps/" + i.ToString() + ".tmx"))
            {
                maps.Add(new Map("Maps/" + i++.ToString() + ".tmx", Content, GraphicsDevice));
            }
            if(File.Exists("Maps/last.tmx"))
                maps.Add(new Map("Maps/last.tmx", Content, GraphicsDevice));
            maps[maps.Count - 1].lastMap = true;


            player = new Player(Content.Load<Texture2D>("Sprites/Player/playerJump"), new Vector2(maps[currentMap].playerSpawn.X, maps[currentMap].playerSpawn.Y - 3), Content.Load<SoundEffect>("Sprites/Player/jump"));
            sprites.Add(player);
            _camera.Follow(player, maps[currentMap]);
            //paralaxScreens.Add(new paralax(new Vector2(0, 40), Content.Load<Texture2D>("Paralax/cloudBackground"), 6f, 0.15f, false));
            paralaxScreens.Add(new paralax(Vector2.Zero, Content.Load<Texture2D>("Paralax/cloudBackground2"), 6f, 0.1f, false));
            paralaxScreens.Add(new paralax(Vector2.Zero, Content.Load<Texture2D>("Paralax/smallHill"), -1f, 0.1f, true));

            thanksScreen = new paralax(Vector2.Zero, Content.Load<Texture2D>("Paralax/thanks"), 0f, 0.16f, false);

            List<Texture2D> controlsAnim = new List<Texture2D>();
            controlsAnim.Add(Content.Load<Texture2D>("Paralax/Controls1"));
            controlsAnim.Add(Content.Load<Texture2D>("Paralax/Controls2"));
            controlsAnimation = new screenAnimation(Vector2.Zero, controlsAnim, 1.2, 0.15f, true);


            List<Texture2D> pauseAnim = new List<Texture2D>();
            pauseAnim.Add(Content.Load<Texture2D>("Paralax/pause1"));
            pauseAnim.Add(Content.Load<Texture2D>("Paralax/pause2"));
            pauseAnimation = new screenAnimation(Vector2.Zero, pauseAnim, 1.2, 0.15f, true);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            if (maps[currentMap].lastMap)
            {
                if(File.Exists("save"))
                    File.Delete("save");

                Dictionary<string, double> times = new Dictionary<string, double>();
                times.Add(DateTime.Now.ToString(), gameTimer);
                if (File.Exists("records"))
                {
                    StreamReader sr = new StreamReader("records");

                    while(!sr.EndOfStream)
                    {
                        var time = (Convert.ToDouble(sr.ReadLine()));
                        var date = (sr.ReadLine());
                        try
                        {
                            times.Add(date, time);
                        }
                        catch
                        {

                        }
                        sr.ReadLine();
                    }
                    sr.Close();
                    
                }
                
                StreamWriter sw = new StreamWriter("records");
                foreach(var time in times.Keys)
                {
                    sw.WriteLine(times[time]);
                    sw.WriteLine(time);
                    sw.WriteLine();
                }
                sw.Close();
            }
            else
            {
                StreamWriter sw = new StreamWriter("save");
                sw.WriteLine(currentMap);
                sw.WriteLine(gameTimer);
                sw.Close();
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            gameTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && previousState.IsKeyUp(Keys.Escape))
            {
                isPaused = !isPaused;
                if (isPaused)
                    pauseAnimation.Reset();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.F) && previousState.IsKeyUp(Keys.F))
            {
                graphics.IsFullScreen = !graphics.IsFullScreen;
                if (graphics.IsFullScreen)
                {
                    graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                    graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                }
                else
                {
                    graphics.PreferredBackBufferWidth = 840;
                    graphics.PreferredBackBufferHeight = 480;
                }
                graphics.ApplyChanges();
                _camera.UpdateTarget();
            }

            if (!isPaused)
            {
                foreach (var sprite in sprites)
                    sprite.Update(gameTime);

                maps[currentMap].Update(gameTime);

                _camera.Follow(player, maps[currentMap]);

                CollisionManager.Update(gameTime, sprites, maps[currentMap]);


                if (maps[currentMap].nextMap)
                {
                    if (currentMap + 1 < maps.Count)
                    {
                        currentMap++;
                        Reset();
                    }
                    else
                        Exit();
                }

                foreach (var sprite in sprites)
                {
                    if (sprite.IsRemoved)
                    {
                        sprite.IsRemoved = false;
                        Reset();
                    }
                }
            }
            previousState = Keyboard.GetState();
            base.Update(gameTime);
        }

        private void Reset()
        {
            maps[currentMap].Reset();
            player.Position = new Vector2(maps[currentMap].playerSpawn.X, maps[currentMap].playerSpawn.Y - 3);

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_camera.RenderTarget);
            GraphicsDevice.Clear(Palette[0]);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);
            //ABSOLUTE DRAWING BEHIND
            //spriteBatch.Draw(screenWash, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.2f);
            foreach (var plx in paralaxScreens)
            {
                if (!plx.Absolute)
                    continue;
                plx.Draw(gameTime, spriteBatch, isPaused);
            }

            spriteBatch.End();
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _camera.Transform, sortMode: SpriteSortMode.FrontToBack);

            maps[currentMap].Draw(gameTime, spriteBatch);
            foreach (var sprite in sprites)
                sprite.Draw(gameTime, spriteBatch);

            foreach(var plx in paralaxScreens)
            {
                if (plx.Absolute)
                    continue;
                plx.Draw(gameTime, spriteBatch, isPaused);
            }
            if(maps[currentMap].lastMap)
                thanksScreen.Draw(gameTime, spriteBatch, isPaused);

            if (currentMap == 0)
                controlsAnimation.Draw(gameTime, spriteBatch, isPaused);

            spriteBatch.End();

            if (isPaused)
            {
                spriteBatch.Begin(samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);
                //ABSOLUTE INFRONT

                pauseAnimation.Draw(gameTime, spriteBatch, false);

                spriteBatch.End();
            }

            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(_camera.RenderTarget, _camera.ScreenRectangle, Color.White);

            spriteBatch.End();
        }
    }
}
