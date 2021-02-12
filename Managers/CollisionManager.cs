using Microsoft.Xna.Framework;
using Nokia3310Jam.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiled;

namespace Nokia3310Jam.Managers
{
    class CollisionManager
    {
        public static void Update(GameTime gameTime, List<Sprite> sprites, Map map)
        {
            foreach(var sprite in sprites)
            {
                CheckCollisionWithMap(gameTime, map, sprite);
                WithinMapBounds(map, sprite);
            }
        }

        private static void WithinMapBounds(Map map, Sprite sprite)
        {
            if (sprite.Position.Y > (map.Height * map.TileHeight) + sprite.Rectangle.Height)
                sprite.IsRemoved = true;

            if (sprite.Position.X < 0)
            {
                sprite.Position.X = 0;
                sprite.Velocity.X = 0;
            }
            if (sprite.Position.X > (map.Width * map.TileWidth - sprite.Rectangle.Width) + 1)
            {
                sprite.Position.X = (map.Width * map.TileWidth - sprite.Rectangle.Width) + 1;
                map.nextMap = true;
                sprite.Velocity.X = 0;
            }
            if (sprite.Position.Y < 0)
            {
                sprite.Position.Y = 0;
                sprite.Velocity.Y = 0;
            }
        }

        private static void CheckCollisionWithMap(GameTime gameTime, Map map, Sprite sprite)
        {
            sprite.Grounded = false;
            var previousPosition = sprite.Position;

            sprite.Position.Y += sprite.Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (sprite is Player)
            {
                if (sprite.Velocity.Y >= 0)
                {
                    foreach (var plat in map.fallingPlatforms)
                    {
                        bool touched = false;
                        foreach (var tile in plat)
                        {
                            if (sprite.Rectangle.Intersects(tile.Rectangle))
                                touched = true;
                        }
                        if (touched)
                        {
                            foreach (var tile in plat)
                                tile.activated = true;
                        }
                    }
                }
            }

            if (Colliding(sprite, map.SolidColliders))
            {
                Ycolliding(gameTime, map.SolidColliders, previousPosition, sprite);
            }
            sprite.Position.X += sprite.Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Colliding(sprite, map.SolidColliders))
            {
                Xcolliding(gameTime, map.SolidColliders, previousPosition, sprite);
            }

            //stuck in switch block
            if (Colliding(sprite, map.SolidColliders))
            {
                if (!Colliding(sprite, map.FallingRects))
                {
                    sprite.Position.Y += 1;
                }
                else
                {
                    sprite.Position.Y -= 1;
                    if (sprite.Velocity.X > 0)
                        sprite.Position.X++;
                    else if (sprite.Velocity.X < 0)
                        sprite.Position.X--;
                }
            }
            //if (Colliding(sprite, map.SolidColliders))
            //{
            //    sprite.IsRemoved = true;
           // }
        }

        static void Ycolliding(GameTime gameTime, List<Rectangle> colliders, Vector2 previousPosition, Sprite sprite)
        {
            if (sprite.Velocity.Y > 0) //going down
            {
                sprite.Grounded = true;
                while (sprite.Velocity.Y > 0 && Colliding(sprite, colliders))
                {
                    sprite.Position.Y = previousPosition.Y;
                    sprite.Velocity.Y--;
                    sprite.Position.Y += sprite.Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
            if (sprite.Velocity.Y < 0)
            {
                while (sprite.Velocity.Y < 0 && Colliding(sprite, colliders))
                {
                    sprite.Position.Y = previousPosition.Y;
                    sprite.Velocity.Y++;
                    sprite.Position.Y += sprite.Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
        }
        static void Xcolliding(GameTime gameTime, List<Rectangle> colliders, Vector2 previousPosition, Sprite sprite)
        {
            if (sprite.Velocity.X > 0) //going right
            {
                while (sprite.Velocity.X > 0 && Colliding(sprite, colliders))
                {
                    sprite.Position.X = previousPosition.X;
                    sprite.Velocity.X--;
                    sprite.Position.X += sprite.Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
            if (sprite.Velocity.X < 0)
            {
                while (sprite.Velocity.X < 0 && Colliding(sprite, colliders))
                {
                    sprite.Position.X = previousPosition.X;
                    sprite.Velocity.X++;
                    sprite.Position.X += sprite.Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
        }
        static bool Colliding(Sprite sprite, List<Rectangle> colliders)
        {
            foreach (var collider in colliders)
            {
                if (sprite.Rectangle.Intersects(collider))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
