using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nokia3310Jam.Tiled.Tile_Classes
{
    public class SwitchBlock
    {
        public Rectangle Rectangle;
        double changeDelay = 3;
        double changeTimer = 0;
        public int Index;
        int originalIndex;
        bool flashing = false;
        double flashingDelay = 0.1;
        double flashingTimer;
        public bool FilledIn;
        bool originalFilledState;
        public SwitchBlock(bool filledIn, Rectangle rect, int index, double switchDelay)
        {
            changeDelay = switchDelay;
            FilledIn = filledIn;
            originalFilledState = FilledIn;
            Rectangle = rect;
            Index = index;
            if (filledIn)
                Index -= 36;
            originalIndex = Index;
        }

        public void Reset()
        {
            Index = originalIndex;
            changeTimer = 0;
            flashingTimer = 0;
            FilledIn = originalFilledState;
        }
        public void Update(GameTime gameTime)
        {
            changeTimer += gameTime.ElapsedGameTime.TotalSeconds;
            
            if(changeTimer > changeDelay)
            {
                FilledIn = !FilledIn;
                changeTimer = 0;
            }
            if ((changeDelay - changeTimer) < 0.6)
            {
                flashing = true;
            }
            else
                flashing = false;
            if(flashing)
            {
                flashingTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if(flashingTimer > flashingDelay)
                {
                    flashingTimer = 0;
                    if (Index == originalIndex)
                        Index += 3;
                    else
                        Index -= 3;
                }
            }
            else
            {
                flashingTimer = 0;
                Index = originalIndex;
            }
        }



    }
}
