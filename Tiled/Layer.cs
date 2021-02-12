using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tiled
{
    public class Layer
    {
        public int id;
        public string name;
        public int width;
        public int height;

        
        public int[] tiles;
        public char[] data;

        #region Tiled Layer Properties
        //add any properties used in Tiled Layers, ensure default values for layers which do not include these preoperties

        public bool collidable = false;
        public bool switchBlock = false;
        public bool spawn = false;
        public double switchDelay = 0f;
        public float Falling = 0;

        #endregion
        public void dataToTiles()
        {
            tiles = new int[height * width];
            int position = 0;

            for (int i = 0; i < height * width; i++)
            {

                    string element = "";
                    
                    while (true)
                    {
                        if (position >= data.Length)
                            break;
                        if(char.IsDigit(data[position]))
                        {
                            element += data[position];
                        }

                        if (data[position++] == ',')
                            break;
                    }

                    if (element == "")
                        throw (new System.Exception("no tile"));
                    else
                        tiles[i] = Convert.ToInt32(element);

            }
        }


    }
}
