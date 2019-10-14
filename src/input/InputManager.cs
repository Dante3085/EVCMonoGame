
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;
using System;

namespace EVCMonoGame.src.input
{
    // TODO: OnKeyCombinationPressed(keys)
    // TODO: OnKeyCombinationReleased(keys)
    // TODO: InputContexts
    // TODO: Actions and States

    public static class InputManager
    {
        private static KeyboardState currentKeyboardState;
        private static KeyboardState previousKeyboardState;

        // private static Dictionary<int, bool> keyCombinations;

        /// <summary>
        /// Always call before all you'r input operations(First instruction in Update()).
        /// </summary>
        public static void UpdateInputStates()
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
        }

        /// <summary>
        /// Returns true on the initial press of the given key.
        /// <para>Returns true if the given key was up in the previous Update() call,
        /// but is now down in the current Update() call, otherwise false. </para>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool OnKeyPressed(Keys key)
        {
            return !previousKeyboardState.IsKeyDown(key) && currentKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Returns true on the initial release of the given key.
        /// <para>Returns true if the given key was down in the previous Update() call,
        /// but is now up in the current Update() call, otherwise false.</para>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool OnKeyReleased(Keys key)
        {
            return previousKeyboardState.IsKeyDown(key) && !currentKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Returns true if the given key is down in the current Update() call, otherwise false.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsKeyPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Returns true if the given key was down in the previous Update() call, otherwise false.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool WasKeyPressed(Keys key)
        {
            return previousKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Returns true if OnKeyPressed() is true for any of the given keys, otherwise false.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool OnAnyKeyPressed(params Keys[] keys)
        {
            foreach (Keys k in keys)
            {
                if (OnKeyPressed(k))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns true if OnKeyReleased() is true for any of the given keys, otherwise false.
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static bool OnAnyKeyReleased(params Keys[] keys)
        {
            foreach (Keys k in keys)
            {
                if (OnKeyReleased(k))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns true if IsKeyPressed() is true for any of the given keys, otherwise false.
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static bool IsAnyKeyPressed(params Keys[] keys)
        {
            foreach (Keys k in keys)
            {
                if (IsKeyPressed(k))
                {
                    return true;
                }
            }
            return false;
        }

        //public static bool OnKeyCombinationPressed(params Keys[] keys)
        //{
        //    String keyCombination = "";

        //    if (!keyCombinations.ContainsKey(keys.GetHashCode()))
        //    {
        //        keyCombinations.Add(keys.GetHashCode(), )
        //    }

        //    //foreach (Keys k in keys)
        //    //{
        //    //    if (previousKeyboardState.IsKeyDown(k) ||
        //    //        !currentKeyboardState.IsKeyDown(k))
        //    //    {
        //    //        return false;
        //    //    }
        //    //}
        //    //return true;

        //    foreach (Keys k in keys)
        //    {
        //        if (!currentKeyboardState.IsKeyDown(k))
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}
    }

    //public class KeyCombination
    //{
    //    private List<Keys> keys;

    //    public KeyCombination(params Keys[] keys)
    //    {
    //        this.keys.AddRange(keys);
    //    }
    //}
}
