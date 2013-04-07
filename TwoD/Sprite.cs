using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HKFramework.TwoD
{
    public class Sprite : GraphicEntity
    {
        #region Declarations
        private TimeSpan _timeToNextFrame;
        private int _millisecondsBetweenFrame;
        private Point _currentFrame;

        AnimationType _animType;

        private int _animDir = 1;

        protected Rectangle _frameRect;
        private int _frameWidth, _frameHeight;
        private int _sheetFrameWidth, _sheetFrameHeight;

        /// <summary>
        /// Whether or not the sprite's animation is playing.
        /// </summary>
        public bool Playing = true;
        #endregion

        /// <summary>
        /// Creates a new entity that takes a sprite-sheet, and will animate using the given parameters.
        /// </summary>
        /// <param name="spriteSheet">The sprite sheet to use.</param>
        /// <param name="position">The initial position of the entity.</param>
        /// <param name="millisecondsBetweenFrame">Milliseconds between each frame.</param>
        /// <param name="frameWidth">The width of each sprite sheet frame.</param>
        /// <param name="frameHeight">The height of each sprite sheet frame.</param>
        /// <param name="animationType">The type of animation to use.</param>
        public Sprite(Texture2D spriteSheet, Vector2 position, int millisecondsBetweenFrame, int frameWidth, int frameHeight, AnimationType animationType)
            : base(spriteSheet, position)
        {
            Init(millisecondsBetweenFrame, frameWidth, frameHeight, animationType);
        }

        /// <summary>
        /// Creates a new entity that takes a sprite-sheet, and will animate using the given parameters.
        /// </summary>
        /// <param name="spriteSheet">The sprite sheet to use.</param>
        /// <param name="position">The initial position of the entity.</param>
        /// <param name="millisecondsBetweenFrame">Milliseconds between each frame.</param>
        /// <param name="frameWidth">The width of each sprite sheet frame.</param>
        /// <param name="frameHeight">The height of each sprite sheet frame.</param>
        /// <param name="animationType">The type of animation to use.</param>
        /// <param name="scale">The scale of the sprite.</param>
        public Sprite(Texture2D spriteSheet, Vector2 position, int millisecondsBetweenFrame, int frameWidth, int frameHeight, AnimationType animationType, Vector2 scale)
            : base(spriteSheet, position, scale)
        {
            Init(millisecondsBetweenFrame, frameWidth, frameHeight, animationType);
        }

        /// <summary>
        /// Creates a new entity that takes a sprite-sheet, and will animate using the given parameters.
        /// </summary>
        /// <param name="spriteSheet">The sprite sheet to use.</param>
        /// <param name="position">The initial position of the entity.</param>
        /// <param name="millisecondsBetweenFrame">Milliseconds between each frame.</param>
        /// <param name="frameWidth">The width of each sprite sheet frame.</param>
        /// <param name="frameHeight">The height of each sprite sheet frame.</param>
        /// <param name="animationType">The type of animation to use.</param>
        /// <param name="scale">The scale of the sprite.</param>
        public Sprite(Texture2D spriteSheet, Vector2 position, int millisecondsBetweenFrame, int frameWidth, int frameHeight, AnimationType animationType, float scale)
            : this(spriteSheet, position, millisecondsBetweenFrame, frameWidth, frameHeight, animationType, new Vector2(scale, scale)) { }

        /// <summary>
        /// Creates a new entity that takes a sprite-sheet, and will animate using the given parameters.
        /// </summary>
        /// <param name="spriteSheet">The sprite sheet to use.</param>
        /// <param name="position">The initial position of the entity.</param>
        /// <param name="millisecondsBetweenFrame">Milliseconds between each frame.</param>
        /// <param name="frameWidth">The width of each sprite sheet frame.</param>
        /// <param name="frameHeight">The height of each sprite sheet frame.</param>
        /// <param name="animationType">The type of animation to use.</param>
        /// <param name="color">The draw color of the sprite.</param>
        public Sprite(Texture2D spriteSheet, Vector2 position, int millisecondsBetweenFrame, int frameWidth, int frameHeight, AnimationType animationType, Color color)
            : this(spriteSheet, position, millisecondsBetweenFrame, frameWidth, frameHeight, animationType)
        {
            _drawColor = color;
        }

        /// <summary>
        /// Creates a new entity that takes a sprite-sheet, and will animate using the given parameters.
        /// </summary>
        /// <param name="spriteSheet">The sprite sheet to use.</param>
        /// <param name="position">The initial position of the entity.</param>
        /// <param name="millisecondsBetweenFrame">Milliseconds between each frame.</param>
        /// <param name="frameWidth">The width of each sprite sheet frame.</param>
        /// <param name="frameHeight">The height of each sprite sheet frame.</param>
        /// <param name="animationType">The type of animation to use.</param>
        /// <param name="color">The draw color of the sprite.</param>
        /// <param name="scale">The scale of the sprite.</param>
        public Sprite(Texture2D spriteSheet, Vector2 position, int millisecondsBetweenFrame, int frameWidth, int frameHeight, AnimationType animationType, Vector2 scale, Color color)
            : this(spriteSheet, position, millisecondsBetweenFrame, frameWidth, frameHeight, animationType)
        {
            _scale = scale;
            _drawColor = color;
        }

        /// <summary>
        /// Creates a new entity that takes a sprite-sheet, and will animate using the given parameters.
        /// </summary>
        /// <param name="spriteSheet">The sprite sheet to use.</param>
        /// <param name="position">The initial position of the entity.</param>
        /// <param name="millisecondsBetweenFrame">Milliseconds between each frame.</param>
        /// <param name="frameWidth">The width of each sprite sheet frame.</param>
        /// <param name="frameHeight">The height of each sprite sheet frame.</param>
        /// <param name="animationType">The type of animation to use.</param>
        /// <param name="color">The draw color of the sprite.</param>
        /// <param name="scale">The scale of the sprite.</param>
        public Sprite(Texture2D spriteSheet, Vector2 position, int millisecondsBetweenFrame, int frameWidth, int frameHeight, AnimationType animationType, float scale, Color color)
            : this(spriteSheet, position, millisecondsBetweenFrame, frameWidth, frameHeight, animationType, new Vector2(scale, scale), color) { }

        /// <summary>
        /// Used to initialize animation variables.
        /// </summary>
        private void Init(int millisecondsBetweenFrame, int frameWidth, int frameHeight, AnimationType animationType)
        {
            _frameWidth = frameWidth;
            _frameHeight = frameHeight;
            _currentFrame = Point.Zero;
            _frameRect = new Rectangle(_currentFrame.X * _frameWidth, _currentFrame.Y * _frameHeight, _frameWidth, _frameHeight);
            _millisecondsBetweenFrame = millisecondsBetweenFrame;

            _animType = animationType;

            _sheetFrameWidth = _texture.Width / frameWidth;
            _sheetFrameHeight = _texture.Height / frameHeight;

            _collisionRect = new Rectangle((int)Position.X, (int)Position.Y, _frameWidth, _frameHeight);
        }

        /// <summary>
        /// Sets the sprite sheet to be used by the entity, and resets all other values accordingly.
        /// </summary>
        public void SetSheet(Texture2D newSheet, int millisecondsBetweenFrame, int frameWidth, int frameHeight, AnimationType animationType)
        {
            _texture = newSheet;
            Init(millisecondsBetweenFrame, frameWidth, frameHeight, animationType);
            Playing = true;
        }

        /// <summary>
        /// Draws the sprite to the screen.
        /// </summary>
        public override void Draw()
        {
            GameUtils.GetUtil<SpriteBatch>().Draw(_texture, Position, _frameRect, _drawColor, _rotation, Vector2.Zero, _scale, _currentEffect, 1f);
        }

        /// <summary>
        /// Updates the entity, changing animation frame when neccesary and updating the collision rectangle to account for movement.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            if (Playing)
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

            _collisionRect = new Rectangle((int)Position.X, (int)Position.Y, (int)(_frameWidth*_scale.X), (int)(_frameHeight*_scale.Y));
        }

        /// <summary>
        /// The sprite's center.
        /// </summary>
        protected override Vector2 Center { get { return new Vector2(_frameWidth / 2, _frameHeight / 2); } }
    }
}
