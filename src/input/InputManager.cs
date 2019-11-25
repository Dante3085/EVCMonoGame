
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EVCMonoGame.src.input
{
    // Todo: Mouse DoubleClick
    // TODO: OnKeyCombinationReleased(keys)
    // TODO: InputContexts
    // TODO: Actions and States

    public static class InputManager
    {
        #region StaticFields

        private static KeyboardState currentKeyboardState;
        private static KeyboardState previousKeyboardState;
        private static List<Keys> buffer = new List<Keys>();

        private static GamePadState currentGamePadStatePlayerOne;
        private static GamePadState previousGamePadStatePlayerOne;

        private static GamePadState currentGamePadStatePlayerTwo;
        private static GamePadState previousGamePadStatePlayerTwo;

        private static MouseState currentMouseState;
        private static MouseState previousMouseState;

        private static Buttons[] buttonsEnum = (Buttons[])Enum.GetValues(typeof(Buttons));

        private static bool inputByKeyboard = true;

        private const int originalWindowWidth = 1920;
        private const int originalWindowHeight = 1080;
        private static Vector2 mouseResolutionScale = Vector2.Zero;

        #endregion
        #region StaticProperties
        /// <summary>
        /// Returns true if the most recent input was given by the Keyboard(Any Key has been pressed).
        /// Returns false if the most recent input was given by the GamePad.
        /// </summary>
        public static bool InputByKeyboard
        {
            get { return inputByKeyboard; }
        }

        public static bool HasLeftGamePadStickMoved
        {
            get { return currentGamePadStatePlayerOne.ThumbSticks.Left.LengthSquared() > 0; }
        }

        public static bool HasRightGamePadStickMoved
        {
            get { return currentGamePadStatePlayerOne.ThumbSticks.Right.LengthSquared() > 0; }
        }

        //public static bool OnAnyGamePadFaceButtonPressed
        //{
        //    get
        //    {
        //        return OnAnyButtonPressed(Buttons.X, Buttons.Y, Buttons.B, Buttons.A);
        //    }
        //}

        public static bool HasMouseMoved
        {
            get
            {
                return currentMouseState.Position != previousMouseState.Position;
            }
        }

        #endregion
        #region StaticMethods
        /// <summary>
        /// Always call before all you'r input operations(First instruction in Update()).
        /// </summary>
        public static void Update(GameTime gameTime, Viewport viewport)
        {
            mouseResolutionScale = new Vector2(originalWindowWidth / viewport.Width, originalWindowHeight / viewport.Height);

            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            previousGamePadStatePlayerOne = currentGamePadStatePlayerOne;
            currentGamePadStatePlayerOne = GamePad.GetState(PlayerIndex.One, GamePadDeadZone.Circular);

            previousGamePadStatePlayerTwo = currentGamePadStatePlayerTwo;
            currentGamePadStatePlayerTwo = GamePad.GetState(PlayerIndex.Two, GamePadDeadZone.Circular);

            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            // Check if last input was given by keyboard or gamepad.
            if (inputByKeyboard)
            {
                if (OnAnyButtonPressed(PlayerIndex.One, buttonsEnum))
                {
                    inputByKeyboard = false;
                }
            }
            else
            {
                if (currentKeyboardState.GetPressedKeys().Length > 0)
                {
                    inputByKeyboard = true;
                }
            }

            // Update inputBuffers.

            // Add new keys to keyCombinationBuffer

        }

        #region Keyboard
        #region Keys
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
        /// Returns true if isKeyPressed() is true for all of the given keys 
        /// and OnKeyPressed is true for one of the key
        /// and no other keys are being pressed, otherwise false.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool OnKeyCombinationPressed(params Keys[] keys)
        {
            bool oneKeyPressedFirstTime = false;

            foreach (Keys k in keys)
            {
                if(!IsKeyPressed(k)) return false;
                if (!oneKeyPressedFirstTime && OnKeyPressed(k)) oneKeyPressedFirstTime = true;
            }
            return oneKeyPressedFirstTime && (keys.Length==currentKeyboardState.GetPressedKeys().Length);
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

        public static bool AreAllKeysPressed(params Keys[] keys)
        {
            foreach (Keys k in keys)
            {
                if (!IsKeyPressed(k))
                {
                    return false;
                }
            }
            return true;
        }

        //private static bool Contains(List<Keys> buffer, Keys[] keys)
        //{
        //    foreach (Keys key in keys)
        //    {
        //        if (!buffer.Contains(key))
        //            return false;
        //    }
        //    return true;
        //}

        //public static bool OnAllKeysPressed(params Keys[] keys)
        //{
        //    // TODO: Das funktioniert so wie ich es will. Der Code ist aber
        //    //       extrem hässlich.
            
        //    // Prinzip: Überprüfe mit AreAllKeysPressed(), ob alle nötigen
        //    // Keys gedrückt sind. Falls alle Keys gedrückt sind, merke dir
        //    // das dies geschehen ist und verhindere das true zurückgegeben 
        //    // wird bis nicht mehr alle Keys gedrückt sind.

        //    if (!Contains(buffer, keys) && 
        //        AreAllKeysPressed(keys))
        //    {
        //        buffer.AddRange(keys);
        //        return true;
        //    }
        //    else if (!AreAllKeysPressed(keys))
        //    {
        //        buffer.RemoveAll((key) =>
        //        {
        //            return keys.Contains(key);
        //        });
        //        return false;
        //    }
        //    return false;
        //}

        #endregion
        #endregion
        #region GamePad
        #region Buttons
        public static bool OnButtonPressed(Buttons button, PlayerIndex playerIndex)
        {
            if (playerIndex == PlayerIndex.One)
            {
                return !previousGamePadStatePlayerOne.IsButtonDown(button) &&
                        currentGamePadStatePlayerOne.IsButtonDown(button);
            }
            else
            {
                return !previousGamePadStatePlayerTwo.IsButtonDown(button) &&
                        currentGamePadStatePlayerTwo.IsButtonDown(button);
            }
        }

        public static bool OnButtonReleased(Buttons button, PlayerIndex playerIndex)
        {
            if (playerIndex == PlayerIndex.One)
            {
                return previousGamePadStatePlayerOne.IsButtonDown(button) &&
                      !currentGamePadStatePlayerOne.IsButtonDown(button);
            }
            else
            {
                return previousGamePadStatePlayerTwo.IsButtonDown(button) &&
                      !currentGamePadStatePlayerTwo.IsButtonDown(button);
            }
        }

        public static bool IsButtonPressed(Buttons button, PlayerIndex playerIndex)
        {
            if (playerIndex == PlayerIndex.One)
            {
                return currentGamePadStatePlayerOne.IsButtonDown(button);
            }
            else
            {
                return currentGamePadStatePlayerTwo.IsButtonDown(button);
            }
        }

        public static bool WasButtonPressed(Buttons button, PlayerIndex playerIndex)
        {
            if (playerIndex == PlayerIndex.One)
            {
                return previousGamePadStatePlayerOne.IsButtonDown(button);
            }
            else
            {
                return previousGamePadStatePlayerTwo.IsButtonDown(button);
            }
        }

        public static bool OnAnyButtonPressed(PlayerIndex playerIndex, params Buttons[] buttons)
        {
            foreach (Buttons b in buttons)
            {
                if (OnButtonPressed(b, playerIndex))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsAnyButtonPressed(PlayerIndex playerIndex, params Buttons[] buttons)
        {
            foreach (Buttons b in buttons)
            {
                if (IsButtonPressed(b, playerIndex))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool AreAllButtonsPressed(PlayerIndex playerIndex, params Buttons[] buttons)
        {
            foreach (Buttons b in buttons)
            {
                if (!IsButtonPressed(b, playerIndex))
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        public static GamePadThumbSticks CurrentThumbSticks(PlayerIndex playerIndex)
        {
            return playerIndex == PlayerIndex.One ? currentGamePadStatePlayerOne.ThumbSticks :
                                                    currentGamePadStatePlayerTwo.ThumbSticks;
        }

        public static GamePadThumbSticks PreviousThumbSticks(PlayerIndex playerIndex)
        {
            return playerIndex == PlayerIndex.One ? previousGamePadStatePlayerOne.ThumbSticks :
                                                    previousGamePadStatePlayerTwo.ThumbSticks;
        }

        public static GamePadTriggers CurrentTriggers(PlayerIndex playerIndex)
        {
            return playerIndex == PlayerIndex.One ? currentGamePadStatePlayerOne.Triggers :
                                                    currentGamePadStatePlayerTwo.Triggers;
        }

        public static GamePadTriggers PreviousTriggers(PlayerIndex playerIndex)
        {
            return playerIndex == PlayerIndex.One ? previousGamePadStatePlayerOne.Triggers :
                                                    previousGamePadStatePlayerTwo.Triggers;
        }
        #endregion
        #region Mouse
        public static Vector2 CurrentMousePosition()
        {
            return currentMouseState.Position.ToVector2() /** mouseResolutionScale*/;
        }

        public static Vector2 PreviousMousePosition()
        {
            return previousMouseState.Position.ToVector2();
        }

        public static bool OnLeftMouseButtonClicked()
        {
            return previousMouseState.LeftButton == ButtonState.Released &&
                   currentMouseState.LeftButton == ButtonState.Pressed;
        }

        public static bool OnLeftMouseButtonReleased()
        {
            return previousMouseState.LeftButton == ButtonState.Pressed &&
                   currentMouseState.LeftButton == ButtonState.Released;
        }

        public static bool OnRightMouseButtonClicked()
        {
            return previousMouseState.RightButton == ButtonState.Released &&
                   currentMouseState.RightButton == ButtonState.Pressed;
        }

        public static bool OnRightMouseButtonReleased()
        {
            return previousMouseState.RightButton == ButtonState.Pressed &&
                   currentMouseState.RightButton == ButtonState.Released;
        }

        public static bool OnMiddleMouseButtonClicked()
        {
            return previousMouseState.MiddleButton == ButtonState.Released &&
                   currentMouseState.MiddleButton == ButtonState.Pressed;
        }

        public static bool OnMiddleMouseButtonReleased()
        {
            return previousMouseState.MiddleButton == ButtonState.Pressed &&
                   currentMouseState.MiddleButton == ButtonState.Released;
        }

        public static bool IsLeftMouseButtonDown()
        {
            return currentMouseState.LeftButton == ButtonState.Pressed;
        }

        public static bool IsRightMouseButtonDown()
        {
            return currentMouseState.RightButton == ButtonState.Pressed;
        }

        public static bool IsMiddleMouseButtonDown()
        {
            return currentMouseState.MiddleButton == ButtonState.Pressed;
        }

        public static float CurrentScrollWheel()
        {
            return currentMouseState.ScrollWheelValue;
        }

        public static float PreviousScrollWheel()
        {
            return previousMouseState.ScrollWheelValue;
        }

        #endregion
        #endregion
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
