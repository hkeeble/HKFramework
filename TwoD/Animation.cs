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
    public class Animation : Graphic
    {
        #region Declarations
        private TimeSpan _timeToNextFrame;
        private int _millisecondsBetweenFrame;
        private Point _currentFrame;

        private Rectangle _frameRect;
        private int _frameWidth, _frameHeight;
        private int _sheetFrameWidth, _sheetFrameHeight;

        AnimationType _animType;
        private int _animDir = 1;
        public bool Playing = true;
        #endregion

        /// <summary>
        /// Constructs a new animation.
        /// </summary>
        /// <param name="spriteSheet">The sprite sheet for the animation to use.</param>
        /// <param name="position">The screen position of the animation.</param>
        /// <param name="scale">The scale of the animation.</param>
        /// <param name="millisecondsBetweenFrame">The milliseconds between each frame in the animation.</param>
        /// <param name="frameWidth">The width of each animation frame.</param>
        /// <param name="frameHeight">The height of each animation frame.</param>
        /// <param name="animationType">The type of animation.</param>
        public Animation(Texture2D spriteSheet, Vector2 position, Vector2 scale, int millisecondsBetweenFrame, int frameWidth, int frameHeight, AnimationType animationType) :
            base(spriteSheet, position, scale)
        {
            _millisecondsBetweenFrame = millisecondsBetweenFrame;
            _frameWidth = frameWidth;
            _frameHeight = frameHeight;
            _sheetFrameWidth = _texture.Width / frameWidth;
            _sheetFrameHeight = _texture.Height / frameHeight;
            _animType = animationType;
        }

        /// <summary>
        /// Draws the animation to the screen.
        /// </summary>
        public void Draw()
        {
            GameUtils.GetUtil<SpriteBatch>().Draw(_texture, Position, _frameRect, Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 1f);
        }

        /// <summary>
        /// Updates the animation, changing frames where neccesary.
        /// </summary>
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

        /// <summary>
        /// The size of an individual animation frame.
        /// </summary>
        public Rectangle FrameSize { get { return new Rectangle(0, 0, _frameWidth, _frameHeight); } }
    }
}
