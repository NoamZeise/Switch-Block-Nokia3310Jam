using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Control.InputClasses
{
    public class Input
    {
        public static bool One
        {
            get
            {
                if (Keyboard.GetState().IsKeyDown(Keys.NumPad7))
                    return true;
                return false;
            }
        }
        public static bool Up
        {
            get
            {
                if (Keyboard.GetState().IsKeyDown(Keys.NumPad8))
                    return true;
                if (Keyboard.GetState().IsKeyDown(Keys.NumPad2))
                    return true;
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                    return true;
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    return true;
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    return true;
                return false;
            }
        }
        public static bool Three
        {
            get
            {
                if (Keyboard.GetState().IsKeyDown(Keys.NumPad9))
                    return true;
                return false;
            }
        }
        public static bool Left
        {
            get
            {
                if (Keyboard.GetState().IsKeyDown(Keys.NumPad4))
                    return true;
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                    return true;
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    return true;
                return false;
            }
        }
        public static bool Five
        {
            get
            {
                if (Keyboard.GetState().IsKeyDown(Keys.NumPad5))
                    return true;
                return false;
            }
        }
        public static bool Right
        {
            get
            {
                if (Keyboard.GetState().IsKeyDown(Keys.NumPad6))
                    return true;
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                    return true;
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    return true;
                return false;
            }
        }
        public static bool Seven
        {
            get
            {
                if (Keyboard.GetState().IsKeyDown(Keys.NumPad1))
                    return true;
                return false;
            }
        }
        public static bool Eight
        {
            get
            {
                if (Keyboard.GetState().IsKeyDown(Keys.NumPad2))
                    return true;
                return false;
            }
        }

            public static bool Mute
        {
            get
            {
                if (Keyboard.GetState().IsKeyDown(Keys.M))
                    return true;
                return false;
            }
        }


        public static InputState GetState()
        {
            return new InputState(One, Up, Three, Left, Five, Right, Seven, Eight, Mute);
        }
    }
    public class InputState
    {
        public bool One;
        public bool Two;
        public bool Three;
        public bool Four;
        public bool Five;
        public bool Six;
        public bool Seven;
        public bool Eight;
        public bool Mute;

        public InputState(bool one, bool two, bool three, bool four, bool five, bool six, bool seven, bool eight, bool mute)
        {
            One = one;
            Two = two;
            Three = three;
            Four = four;
            Five = five;
            Six = six;
            Seven = seven;
            Eight = eight;
            Mute = mute;
        }
    }
}
