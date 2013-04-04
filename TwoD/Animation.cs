using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HKFramework;

namespace HKFramework.TwoD
{
    public enum AnimationType
    {
        Loop,
        Reverse,
        PlayOnce
    }

    /// <summary>
    /// Class defines a simple animation, that has no collision rect, does not move and is purely for aesthetics.
    /// </summary>
    public class Animation
    {
        private Texture2D _texture;
        public Vector2 Position;

        private TimeSpan _timeToNextFrame;
        private int _millisecondsBetweenFrame;
        private Point _currentFrame;

        private Rectangle _frameRect;
        private int _frameWidth, _frameHeight;
        private int _sheetFrameWidth, _sheetFrameHeight;

        AnimationType _animType;
        private int _animDir = 1;
        public bool Playing = true;

        public Animation(Texture2D texture, Vector2 position, int millisecondsBetweenFrame, int frameWidth, int frameHeight, AnimationType animationType)
        {
            _texture = texture;
            Position = position;
            _millisecondsBetweenFrame = millisecondsBetweenFrame;
            _frameWidth = frameWidth;
            _frameHeight = frameHeight;
            _sheetFrameWidth = _texture.Width / frameWidth;
            _sheetFrameHeight = _texture.Height / frameHeight;

            _animType = animationType;
        }

        public void Draw()
        {
            GameUtils.GetUtil<SpriteBatch>().Draw(_texture, Position, _frameRect, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            if(Playing)
            {
                _timeToNextFrame += gameTime.ElapsedGameTime;

                if (_timeToNextFrame >= TimeSpan.FromMilliseconds(_millisecondsBetweenFrame))
                {
                    _timeToNextFrame = TimeSpan.Zero;
                    _currentFrame.X += _animDir;
                    if (_currentFrame.X > _sheetFrameWidth - 1 || _currentFrame.X < 0)
                    {
                        if (_animType == AnimationType.Loop)
                            _currentFrame.X = 0;
                        if (_animType == AnimationType.Reverse)
                        {
                            if (_animDir == -1)
                                _currentFrame.X = 0;
                            else
                                _currentFrame.X = _sheetFrameWidth - 1;
                            _animDir = -_animDir;
                        }
                        if (_animType == AnimationType.PlayOnce)
                        {
                            _currentFrame.X = _sheetFrameWidth - 1;
                            Playing = false;
                        }
                    }
                    _frameRect = new Rectangle(_currentFrame.X * _frameWidth, _currentFrame.Y * _frameHeight, _frameWidth, _frameHeight);
                }
            }
        }

        public Rectangle FrameSize { get { return new Rectangle(0, 0, _frameWidth, _frameHeight); } }
    }
}
