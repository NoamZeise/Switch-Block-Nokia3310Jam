using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Tiled.Tile_Classes;
using Nokia3310Jam.Tiled.Tile_Classes;

namespace Tiled
{
    public class Map
    {
        public List<Layer> Layers;
        public List<Texture2D> TileMap;

        public int Width;
        public int Height;
        public int TileWidth;
        public int TileHeight;


        public List<Rectangle> SolidColliders;
        public List<Rectangle> FallingRects;
        public List<SwitchBlock> switchBlocks;
        public List<List<FallingTile>> fallingPlatforms;
        public List<spawn> spawnPoints;
        public Rectangle playerSpawn;
        int switchTileStart = 1000;
        int spawnTileStart = 1000;

        public List<MapText> MapText;
        public List<TiledObject> ObjectLayer;

        public bool nextMap = false;
        public bool lastMap = false;
        float _depth = 0.4f;

        public Map(string fileName, ContentManager content, GraphicsDevice graphicsDevice)
        {
            Layers = new List<Layer>();
            TileMap = new List<Texture2D>();

            StreamReader sr = new StreamReader(fileName);
            XmlReader xmlR = XmlReader.Create(sr);

            SolidColliders = new List<Rectangle>();
            switchBlocks = new List<SwitchBlock>();
            fallingPlatforms = new List<List<FallingTile>>();
            FallingRects = new List<Rectangle>();
            spawnPoints = new List<spawn>();
            MapText = new List<MapText>();
            ObjectLayer = new List<TiledObject>();


            bool inMap = false;
            while (xmlR.Read())
            {
                if (xmlR.Name == "map" && !inMap)
                {
                    //set map properties
                    Width = Convert.ToInt32(xmlR.GetAttribute("width"));
                    TileWidth = Convert.ToInt32(xmlR.GetAttribute("tilewidth"));
                    Height = Convert.ToInt32(xmlR.GetAttribute("height"));
                    TileHeight = Convert.ToInt32(xmlR.GetAttribute("tileheight"));
                    inMap = true;
                }

                if (xmlR.Name == "tileset")
                    makeTileSet(xmlR.GetAttribute("source"), content, graphicsDevice);
                if (xmlR.Name == "layer")
                    createLayer(xmlR);
                if (xmlR.Name == "objectgroup")
                    createObjects(xmlR);

            }
            //set collidables
            for (int layer = 0; layer < Layers.Count; layer++)
            {
                if (Layers[layer].collidable)
                {
                    int currentTile = 0;
                    for (int i = 0; i < Layers[layer].height; i++)
                    {
                        int y = i * TileHeight;
                        for (int j = 0; j < Layers[layer].width; j++)
                        {
                            int x = j * TileWidth;

                            if (Layers[layer].collidable && Layers[layer].tiles[currentTile] != 0)
                            {
                                SolidColliders.Add(new Rectangle(x, y, TileWidth, TileHeight));
                            }
                            if (currentTile++ >= Layers[layer].tiles.Length)
                                return;
                        }
                    }
                }
                if (Layers[layer].switchBlock)
                {
                    int currentTile = 0;
                    for (int i = 0; i < Layers[layer].height; i++)
                    {
                        int y = i * TileHeight;
                        for (int j = 0; j < Layers[layer].width; j++)
                        {
                            int x = j * TileWidth;

                            if (Layers[layer].switchBlock && Layers[layer].tiles[currentTile] != 0)
                            {
                                bool filled = true;
                                if (Layers[layer].tiles[currentTile] <= switchTileStart + 35)
                                    filled = false;

                                switchBlocks.Add(new SwitchBlock(filled, new Rectangle(x, y, TileWidth, TileHeight), Layers[layer].tiles[currentTile], Layers[layer].switchDelay));
                            }
                            if (currentTile++ >= Layers[layer].tiles.Length)
                                return;
                        }
                    }
                }
                if(Layers[layer].spawn)
                {
                    int currentTile = 0;
                    for (int i = 0; i < Layers[layer].height; i++)
                    {
                        int y = i * TileHeight;
                        for (int j = 0; j < Layers[layer].width; j++)
                        {
                            int x = j * TileWidth;

                            if (Layers[layer].spawn && Layers[layer].tiles[currentTile] != 0)
                            {

                                spawnPoints.Add(new spawn(new Rectangle(x, y, 4, 4), Layers[layer].tiles[currentTile] - spawnTileStart));
                                if (Layers[layer].tiles[currentTile] - spawnTileStart == 0)
                                    playerSpawn = new Rectangle(x, y, 4, 4);
                            }
                            if (currentTile++ >= Layers[layer].tiles.Length)
                                return;
                        }
                    }
                   

                }
                if (Layers[layer].Falling != 0)
                {
                    int currentTile = 0;
                    for (int i = 0; i < Layers[layer].height; i++)
                    {
                        int y = i * TileHeight;

                        var SinglePlatform = new List<FallingTile>();
                        for (int j = 0; j < Layers[layer].width; j++)
                        {
                            int x = j * TileWidth;

                            if (Layers[layer].Falling != 0 && Layers[layer].tiles[currentTile] != 0)
                            {
                                SinglePlatform.Add(new FallingTile(TileMap[Layers[layer].tiles[currentTile]],
                                    new Vector2(x, y), Layers[layer].Falling));

                            }
                            if (currentTile++ >= Layers[layer].tiles.Length - 1)
                                break;

                            if ((Layers[layer].tiles[currentTile] == 0) && SinglePlatform.Count > 0)
                            {
                                fallingPlatforms.Add(SinglePlatform);
                                SinglePlatform = new List<FallingTile>();
                            }
                        }
                        if (SinglePlatform.Count > 0)
                            fallingPlatforms.Add(SinglePlatform);
                    }
                }
            }
        }

        private void makeTileSet(string source, ContentManager content, GraphicsDevice graphicsDevice)
        {
            if (TileMap.Count == 0)
            {
                var blank = new Texture2D(graphicsDevice, TileWidth, TileHeight);
                TileMap.Add(blank);
            }
            if (source.Contains(".png"))
            {
                source = source.Replace(".png", "");
            }
            StreamReader sr = new StreamReader("maps/" + source);
            var xmlR = XmlReader.Create(sr);

            int spacing = 0, margin = 0, tilecount = 0, columns = 0;

            int imgWidth = 0, imgHeight = 0;

            bool endOfTileSet = false;
            while (xmlR.Read())
            {
                if (xmlR.Name == "tileset" && !endOfTileSet)
                {
                    spacing = Convert.ToInt32(xmlR.GetAttribute("spacing"));
                    margin = Convert.ToInt32(xmlR.GetAttribute("margin"));
                    tilecount = Convert.ToInt32(xmlR.GetAttribute("tilecount"));
                    columns = Convert.ToInt32(xmlR.GetAttribute("columns"));
                }
                if (xmlR.Name == "tileset")
                    endOfTileSet = !endOfTileSet;
                if (xmlR.Name == "image")
                {
                    source = xmlR.GetAttribute("source");
                    imgWidth = Convert.ToInt32(xmlR.GetAttribute("width"));
                    imgHeight = Convert.ToInt32(xmlR.GetAttribute("height"));
                }
            }
            if (source.Contains(".png"))
            {
                source = source.Replace(".png", "");
            }
            var fullMap = content.Load<Texture2D>("Tilesets/" + source);

            if (source == "switchBlocks")
                switchTileStart = TileMap.Count;
            if (source == "spawnSprites")
                spawnTileStart = TileMap.Count;
            int currentTile = TileMap.Count;
            for (int i = 0; i < tilecount / columns; i++)
            {
                int y = margin + i * TileHeight + i * spacing;
                for (int j = 0; j < columns; j++)
                {
                    int x = margin + j * TileWidth + j * spacing;
                    var tileRect = new Rectangle(x, y, TileWidth, TileHeight);

                    Texture2D tile = new Texture2D(graphicsDevice, tileRect.Width, tileRect.Height);
                    int count = tileRect.Width * tileRect.Height;
                    Color[] data = new Color[count];

                    fullMap.GetData(0, tileRect, data, 0, count);
                    tile.SetData(data);

                    TileMap.Add(tile);
                }
            }
        }

        private void createLayer(XmlReader xmlR)
        {
            var layer = new Layer
            {
                id = Convert.ToInt32(xmlR.GetAttribute("id")),
                name = xmlR.GetAttribute("name"),
                width = Convert.ToInt32(xmlR.GetAttribute("width")),
                height = Convert.ToInt32(xmlR.GetAttribute("height"))
            };

            xmlR.Read();

            bool inProperties = false;
            bool inData = false;
            while (xmlR.Name != "layer")
            {
                if (xmlR.Name == "properties")
                    inProperties = !inProperties;
                if (xmlR.Name == "data")
                    inData = !inData;
                if (inProperties)
                {
                    //add any properites used in tiled, and ensure they are also properties of the layer class

                    if (xmlR.GetAttribute("name") == "Collidable")
                        layer.collidable = Convert.ToBoolean(xmlR.GetAttribute("value"));
                    if (xmlR.GetAttribute("name") == "switchBlock")
                        layer.switchBlock = Convert.ToBoolean(xmlR.GetAttribute("value"));
                    if (xmlR.GetAttribute("name") == "switchDelay")
                        layer.switchDelay = Convert.ToDouble(xmlR.GetAttribute("value"));
                    if (xmlR.GetAttribute("name") == "spawn")
                        layer.spawn = Convert.ToBoolean(xmlR.GetAttribute("value"));
                    if (xmlR.GetAttribute("name") == "Falling")
                    {
                        layer.Falling = (float)Convert.ToDouble(xmlR.GetAttribute("value"));
                    }
                }
                if (inData)
                    layer.data = xmlR.Value.ToCharArray();
                xmlR.Read();
            }
            layer.dataToTiles();
            Layers.Add(layer);
        }

        private void createObjects(XmlReader xmlR)
        {
            xmlR.Read();
            while (xmlR.Name != "objectgroup")
            {
                if (xmlR.Name == "text")
                {
                    MapText.Add(new MapText(new Vector2(ObjectLayer[ObjectLayer.Count - 1].Rectangle.X,
                        ObjectLayer[ObjectLayer.Count - 1].Rectangle.Y),//position of text
                        xmlR.ReadElementContentAsString())); //text from file
                    ObjectLayer.RemoveAt(ObjectLayer.Count - 1);
                }
                if (xmlR.Name == "object")
                {
                    if (xmlR.GetAttribute("name") != "" && xmlR.GetAttribute("name") != null)
                    {
                        string name = xmlR.GetAttribute("name").ToLower();
                        int x = (int)Convert.ToDouble(xmlR.GetAttribute("x"));
                        int y = (int)Convert.ToDouble(xmlR.GetAttribute("y"));
                        if (xmlR.GetAttribute("width") == "")
                            ObjectLayer.Add(new TiledObject(name, x, y));
                        else
                        {
                            int width = (int)Convert.ToDouble(xmlR.GetAttribute("width"));
                            int height = (int)Convert.ToDouble(xmlR.GetAttribute("height"));
                            ObjectLayer.Add(new TiledObject(name, x, y, width, height));
                        }
                    }
                    else if (xmlR.GetAttribute("template") != "" && xmlR.GetAttribute("template") != null)
                    {
                        string name = "";
                        StreamReader sr = new StreamReader("maps/" + xmlR.GetAttribute("template"));
                        int x = (int)Convert.ToDouble(xmlR.GetAttribute("x"));
                        int y = (int)Convert.ToDouble(xmlR.GetAttribute("y"));
                        int width = -10;
                        int height = -10;
                        XmlReader xmlTemplateR = XmlReader.Create(sr);
                        while (xmlTemplateR.Read())
                        {

                            if (xmlTemplateR.Name == "object")
                                if (xmlTemplateR.GetAttribute("name") != ""
                                    && xmlTemplateR.GetAttribute("name") != null)
                                {
                                    name = xmlTemplateR.GetAttribute("name").ToLower();
                                    if (xmlTemplateR.GetAttribute("width") != ""
                                        && xmlTemplateR.GetAttribute("width") != null)
                                    {
                                        width = (int)Convert.ToDouble(xmlTemplateR.GetAttribute("width"));
                                        height = (int)Convert.ToDouble(xmlTemplateR.GetAttribute("height"));
                                        ObjectLayer.Add(new TiledObject(name, x, y, width, height));
                                    }
                                }
                        }
                        if (xmlR.GetAttribute("width") == "" && width == -10 && height == -10)
                            ObjectLayer.Add(new TiledObject(name, x, y));
                        else
                        {
                            if (width == -10 && height == -10)
                            {
                                width = (int)Convert.ToDouble(xmlR.GetAttribute("width"));
                                height = (int)Convert.ToDouble(xmlR.GetAttribute("height"));
                            }
                            ObjectLayer.Add(new TiledObject(name, x, y, width, height));
                        }


                    }
                }
                xmlR.Read();
            }
        }

        public void Reset()
        {
            foreach (var plat in fallingPlatforms)
                foreach (var tile in plat)
                    tile.Reset();
            foreach (var switchB in switchBlocks)
                switchB.Reset();
            foreach (var fallRect in FallingRects)
            {
                SolidColliders.Remove(fallRect);
            }
            FallingRects.Clear();
        }

        public void Update(GameTime gameTime)
        {
            foreach(var fallRect in FallingRects)
            {
                 SolidColliders.Remove(fallRect);
            }
            foreach (var switchB in switchBlocks)
            {
                switchB.Update(gameTime);
                if(switchB.FilledIn)
                {
                    if (!SolidColliders.Contains(switchB.Rectangle))
                    {
                        SolidColliders.Add(switchB.Rectangle);
                    }
                }
                else
                {
                    if(SolidColliders.Contains(switchB.Rectangle))
                    {
                        SolidColliders.Remove(switchB.Rectangle);
                    }
                }
            }
            foreach (var platform in fallingPlatforms)
                foreach (var tile in platform)
                {
                    FallingRects.Remove(tile.Rectangle);
                    tile.Update(gameTime);
                    FallingRects.Add(tile.Rectangle);
                }


            foreach (var fallRect in FallingRects)
            {
               SolidColliders.Add(fallRect);
            }
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int layer = 0; layer < Layers.Count; layer++)
            {
                if (Layers[layer].spawn)
                    continue;
                if (Layers[layer].switchBlock)
                {
                    foreach (var switchB in switchBlocks)
                    {
                        var offset = 0;
                        if(switchB.FilledIn)
                        {
                            offset = 36;
                        }
                        spriteBatch.Draw(TileMap[switchB.Index + offset], switchB.Rectangle, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, _depth + ((float)layer / 100));
                    }
                }
                else if (Layers[layer].Falling != 0)
                {
                    foreach (var platform in fallingPlatforms)
                        foreach (var tile in platform)
                            tile.Draw(gameTime, spriteBatch, _depth + ((float)layer / 100));
                    continue;
                }
                else
                {
                    int currentTile = 0;
                    for (int i = 0; i < Layers[layer].height; i++)
                    {
                        int y = i * TileHeight;
                        for (int j = 0; j < Layers[layer].width; j++)
                        {
                            int x = j * TileWidth;
                            spriteBatch.Draw(TileMap[Layers[layer].tiles[currentTile++]], new Vector2(x, y), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, _depth + ((float)layer / 100));
                        }
                    }
                }
            }

        }
    }
}
