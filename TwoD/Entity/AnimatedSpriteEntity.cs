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

        protected Rectangle _frameRect;
        private int _frameWidth, _frameHeight;
        private int _sheetFrameWidth, _sheetFrameHeight;

        public AnimatedSpriteEntity(Texture2D texture, Vector2 position, int millisecondsBetweenFrame, int frameWidth, int frameHeight) : base(texture, position)
        {
            _frameWidth = frameWidth;
            _frameHeight = frameHeight;
            _currentFrame = Point.Zero;
            _frameRect = new Rectangle(_currentFrame.X * _frameWidth, _currentFrame.Y * _frameHeight, _frameWidth, _frameHeight);
            _collisionRect = new Rectangle(0, 0, _frameWidth, _frameHeight);
            _millisecondsBetweenFrame = millisecondsBetweenFrame;

            _sheetFrameWidth = _texture.Width / frameWidth;
            _sheetFrameHeight = _texture.Height / frameHeight;
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
                _currentFrame.X++;
                if (_currentFrame.X > _sheetFrameWidth - 1)
                    _currentFrame.X = 0;
                _frameRect = new Rectangle(_currentFrame.X * _frameWidth, _currentFrame.Y * _frameHeight, _frameWidth, _frameHeight);
            }

            _collisionRect = new Rectangle((int)_position.X, (int)_position.Y, _frameWidth, _frameHeight);
        }
    }
}
