using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace HKFramework
{
    public static class Config
    {
        private static int _width, _height;
        private static bool _fullScreen = false;
        public static float AspectRatio { get { return GameUtils.GetUtil<GraphicsDevice>().Viewport.AspectRatio; } }

        public static void SetResolution(int width, int height)
        {
            _width = width;
            _height = height;
            UpdateWindow();
        }

        public static void ToggleFullScreen()
        {
            _fullScreen = !_fullScreen;
            UpdateWindow();
        }

        private static void UpdateWindow()
        {
            GameUtils.GetUtil<GraphicsDeviceManager>().PreferredBackBufferWidth = _width;
            GameUtils.GetUtil<GraphicsDeviceManager>().PreferredBackBufferHeight = _height;
            GameUtils.GetUtil<GraphicsDeviceManager>().IsFullScreen = _fullScreen;
            GameUtils.GetUtil<GraphicsDeviceManager>().ApplyChanges();
        }

        public static int Width { get { return _width; } }
        public static int Height { get { return _height; } }
        public static bool Fullscreen { get { return _fullScreen; } }
        public static Rectangle WindowBounds { get { return new Rectangle(0, 0, _width, _height); } }
    }
}
