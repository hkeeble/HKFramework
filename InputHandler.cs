using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace HKFramework
{
    public class InputHandler : Microsoft.Xna.Framework.GameComponent
    {
        private static GamePadState currentGamePadState;
        private static GamePadState prevGamePadState;

        public InputHandler(Game game)
            : base(game)
        {
            
        }

        #region GamePad Accessors
        /// <summary>
        /// Checks if button has just been pressed.
        /// </summary>
        /// <param name="button">The button to check.</param>
        /// <returns></returns>
        public static bool IsButtonPressed(Buttons button)
        {
            if (prevGamePadState.IsButtonDown(button) == false && currentGamePadState.IsButtonDown(button) == true)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks if button is held down.
        /// </summary>
        /// <param name="button">The button to check.</param>
        /// <returns></returns>
        public static bool IsButtonDown(Buttons button)
        {
            if (prevGamePadState.IsButtonDown(button) == true && currentGamePadState.IsButtonDown(button) == true)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks if button has been released.
        /// </summary>
        /// <param name="button">The button to check.</param>
        /// <returns></returns>
        public static bool IsButtonReleased(Buttons button)
        {
            if (prevGamePadState.IsButtonDown(button) == true && currentGamePadState.IsButtonDown(button) == false)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks if button is up.
        /// </summary>
        /// <param name="button">The button to check.</param>
        /// <returns></returns>
        public static bool IsButtonUp(Buttons button)
        {
            if (prevGamePadState.IsButtonDown(button) == false && currentGamePadState.IsButtonDown(button) == false)
                return true;
            else
                return false;
        }

        public static Vector2 LeftStick
        {
            get { return currentGamePadState.ThumbSticks.Left; }
        }

        public static Vector2 RightStick
        {
            get { return currentGamePadState.ThumbSticks.Right; }
        }

        #endregion

        public static void ClearBuffers()
        {
            prevGamePadState = new GamePadState();
            currentGamePadState = new GamePadState();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            prevGamePadState = currentGamePadState;
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            base.Update(gameTime);
        }
    }
}
