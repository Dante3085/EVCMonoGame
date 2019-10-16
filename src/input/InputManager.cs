
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

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

        private static GamePadState currentGamePadState;
        private static GamePadState previousGamePadState;

        private static Buttons[] buttonsEnum = (Buttons[])Enum.GetValues(typeof(Buttons));

        private static bool inputByKeyboard = true;

        /// <summary>
        /// Returns true if the most recent input was given by the Keyboard(Any Key has been pressed).
        /// Returns false if the most recent input was given by the GamePad.
        /// </summary>
        public static bool InputByKeyboard
        {
            get { return inputByKeyboard; }
        }

        // private static Dictionary<int, bool> keyCombinations;

        /// <summary>
        /// Always call before all you'r input operations(First instruction in Update()).
        /// </summary>
        public static void UpdateInputStates()
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            previousGamePadState = currentGamePadState;
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            if (inputByKeyboard)
            {
                if (OnAnyButtonPressed(buttonsEnum))
                {
                    Console.WriteLine("InputByKeyboard false");
                    inputByKeyboard = false;
                }
            }
            else
            {
                if (currentKeyboardState.GetPressedKeys().Length > 0)
                {
                    Console.WriteLine("InputByKeyboard true");
                    inputByKeyboard = true;
                }
            }
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
            return !previousKeyboardState.IsKeyDown(key) && 
                    currentKeyboardState.IsKeyDown(key);
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
            return previousKeyboardState.IsKeyDown(key) && 
                  !currentKeyboardState.IsKeyDown(key);
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

        public static bool OnButtonPressed(Buttons button)
        {
            return !previousGamePadState.IsButtonDown(button) && 
                    currentGamePadState.IsButtonDown(button);
        }

        public static bool OnButtonReleased(Buttons button)
        {
            return previousGamePadState.IsButtonDown(button) &&
                  !currentGamePadState.IsButtonDown(button);
        }

        public static bool IsButtonPressed(Buttons button)
        {
            return currentGamePadState.IsButtonDown(button);
        }

        public static bool WasButtonPressed(Buttons button)
        {
            return previousGamePadState.IsButtonDown(button);
        }

        public static bool OnAnyButtonPressed(params Buttons[] buttons)
        {
            foreach (Buttons b in buttons)
            {
                if (OnButtonPressed(b))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsAnyButtonPressed(params Buttons[] buttons)
        {
            foreach (Buttons b in buttons)
            {
                if (IsButtonPressed(b))
                {
                    return true;
                }
            }
            return false;
        }

        public static GamePadThumbSticks CurrentThumbSticks()
        {
            return currentGamePadState.ThumbSticks;
        }

        public static GamePadThumbSticks PreviousThumbSticks()
        {
            return previousGamePadState.ThumbSticks;
        }

        public static GamePadTriggers CurrentTriggers()
        {
            return currentGamePadState.Triggers;
        }

        public static GamePadTriggers PreviousTriggers()
        {
            return previousGamePadState.Triggers;
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
