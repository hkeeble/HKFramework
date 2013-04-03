using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HKFramework;

namespace HKFramework.TwoD
{
    /// <summary>
    /// Class defines a simple animation, that has no collision rect, does not move and is purely for aesthetics.
    /// </summary>
    public class Animation
    {
        private Texture2D _texture;
        public Vector2 _position;

        private TimeSpan _timeToNextFrame;
        private int _millisecondsBetweenFrame;
        private Point _currentFrame;

        private Rectangle _frameRect;
        private int _frameWidth, _frameHeight;
        private int _sheetFrameWidth, _sheetFrameHeight;

        public Animation(Texture2D texture, Vector2 position, int millisecondsBetweenFrame, int frameWidth, int frameHeight)
        {
            _texture = texture;
            _position = position;
            _millisecondsBetweenFrame = millisecondsBetweenFrame;
            _frameWidth = frameWidth;
            _frameHeight = frameHeight;
            _sheetFrameWidth = _texture.Width / frameWidth;
            _sheetFrameHeight = _texture.Height / frameHeight;
        }

        public void Draw()
        {
            GameUtils.GetUtil<SpriteBatch>().Draw(_texture, _position, _frameRect, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            _timeToNextFrame += gameTime.ElapsedGameTime;

            if (_timeToNextFrame >= TimeSpan.FromMilliseconds(_millisecondsBetweenFrame))
            {
                _timeToNextFrame = TimeSpan.Zero;
                if (_currentFrame.X + 1 >= _sheetFrameWidth)
                    _currentFrame.X = 0;
                else
                    _currentFrame.X++;
                _frameRect = new Rectangle(_currentFrame.X * _frameWidth, _currentFrame.Y * _frameHeight, _frameWidth, _frameHeight);
            }
        }

        public Rectangle FrameSize { get { return new Rectangle(0, 0, _frameWidth, _frameHeight); } }
    }
}
