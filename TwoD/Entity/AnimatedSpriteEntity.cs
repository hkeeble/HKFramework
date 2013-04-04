using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HKFramework.TwoD.Entity
{
    public class AnimatedSpriteEntity : SpriteEntity
    {
        private TimeSpan _timeToNextFrame;
        private int _millisecondsBetweenFrame;
        private Point _currentFrame;

        AnimationType _animType;

        private int _animDir = 1;

        protected Rectangle _frameRect;
        private int _frameWidth, _frameHeight;
        private int _sheetFrameWidth, _sheetFrameHeight;

        public AnimatedSpriteEntity(Texture2D texture, Vector2 position, int millisecondsBetweenFrame, int frameWidth, int frameHeight, AnimationType animationType) : base(texture, position)
        {
            Init(millisecondsBetweenFrame, frameWidth, frameHeight, animationType);
        }

        private void Init(int millisecondsBetweenFrame, int frameWidth, int frameHeight, AnimationType animationType)
        {
            _frameWidth = frameWidth;
            _frameHeight = frameHeight;
            _currentFrame = Point.Zero;
            _frameRect = new Rectangle(_currentFrame.X * _frameWidth, _currentFrame.Y * _frameHeight, _frameWidth, _frameHeight);
            _collisionRect = new Rectangle(0, 0, _frameWidth, _frameHeight);
            _millisecondsBetweenFrame = millisecondsBetweenFrame;

            _animType = animationType;

            _sheetFrameWidth = _texture.Width / frameWidth;
            _sheetFrameHeight = _texture.Height / frameHeight;
        }

        public void SetSheet(Texture2D newSheet, int millisecondsBetweenFrame, int frameWidth, int frameHeight, AnimationType animationType)
        {
            Init(millisecondsBetweenFrame, frameWidth, frameHeight, animationType);
            _texture = newSheet;
        }

        public override void Draw()
        {
            GameUtils.GetUtil<SpriteBatch>().Draw(_texture, _position, _frameRect, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        }

        public override void Update(GameTime gameTime)
        {
            _timeToNextFrame += gameTime.ElapsedGameTime;

            if(_timeToNextFrame >= TimeSpan.FromMilliseconds(_millisecondsBetweenFrame))
            {
                _timeToNextFrame = TimeSpan.Zero;
                _currentFrame.X += _animDir;
                if (_currentFrame.X > _sheetFrameWidth - 1 || _currentFrame.X < 0)
                {
                    if(_animType == AnimationType.Loop)
                        _currentFrame.X = 0;
                    if (_animType == AnimationType.Reverse)
                    {
                        if (_animDir == -1)
                            _currentFrame.X = 0;
                        else
                            _currentFrame.X = _sheetFrameWidth - 1;
                        _animDir = -_animDir;
                    }
                }
                _frameRect = new Rectangle(_currentFrame.X * _frameWidth, _currentFrame.Y * _frameHeight, _frameWidth, _frameHeight);
            }

            _collisionRect = new Rectangle((int)_position.X, (int)_position.Y, _frameWidth, _frameHeight);
        }
    }
}
