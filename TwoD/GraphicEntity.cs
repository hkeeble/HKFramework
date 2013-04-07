using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HKFramework.TwoD
{
    /// <summary>
    /// Represents a graphical entity in world space, that can interact with other entites through a collision rectangle.
    /// </summary>
    public class GraphicEntity : Graphic
    {
        protected Rectangle _collisionRect;
        protected Rectangle _bounds; // An entity can set it's own bounds

        #region Constructors
        /// <summary>
        /// Creates new entity that is drawable on-screen.
        /// </summary>
        /// <param name="texture">The texture to use for the entity.</param>
        /// <param name="position">The entity's initial position.</param>
        public GraphicEntity(Texture2D texture, Vector2 position) : base(texture, position)
        {
            _bounds = Config.WindowBounds;
        }

        /// <summary>
        /// Creates new entity that is drawable on-screen.
        /// </summary>
        /// <param name="texture">The texture to use for the entity.</param>
        /// <param name="position">The entity's initial position.</param>
        /// <param name="scale">The scale of the graphic.</param>
        public GraphicEntity(Texture2D texture, Vector2 position, float scale) : this(texture, position, new Vector2(scale, scale)) { }

        /// <summary>
        /// Creates new entity that is drawable on-screen.
        /// </summary>
        /// <param name="texture">The texture to use for the entity.</param>
        /// <param name="position">The entity's initial position.</param>
        /// <param name="scale">The scale of the graphic.</param>
        public GraphicEntity(Texture2D texture, Vector2 position, Vector2 scale)
            : this(texture, position)
        {
            _scale = scale;
        }

        /// <summary>
        /// Creates new entity that is drawable on-screen.
        /// </summary>
        /// <param name="texture">The texture to use for the entity.</param>
        /// <param name="position">The entity's initial position.</param>
        /// <param name="color">The draw color of the graphic.</param>
        public GraphicEntity(Texture2D texture, Vector2 position, Color color)
            : this(texture, position)
        {
            _drawColor = color;
        }

        /// <summary>
        /// Creates new entity that is drawable on-screen.
        /// </summary>
        /// <param name="texture">The texture to use for the entity.</param>
        /// <param name="position">The entity's initial position.</param>
        /// <param name="scale">The scale of the graphic.</param>
        /// <param name="color">The draw color of the graphic.</param>
        public GraphicEntity(Texture2D texture, Vector2 position, float scale, Color color) : this(texture, position, color, new Vector2(scale, scale)) { }

        /// <summary>
        /// Creates new entity that is drawable on-screen.
        /// </summary>
        /// <param name="texture">The texture to use for the entity.</param>
        /// <param name="position">The entity's initial position.</param>
        /// <param name="scale">The scale of the graphic.</param>
        /// <param name="color">The draw color of the graphic.</param>
        public GraphicEntity(Texture2D texture, Vector2 position, Color color, Vector2 scale)
            : this(texture, position)
        {
            _drawColor = color;
            _scale = scale;
        }

        /// <summary>
        /// Creates new entity that is drawable on-screen.
        /// </summary>
        /// <param name="texture">The texture to use for the entity.</param>
        /// <param name="position">The entity's initial position.</param>
        /// <param name="scale">The scale of the graphic.</param>
        /// <param name="color">The draw color of the graphic.</param>
        /// <param name="bounds">The bounds of the graphic entity.</param>
        public GraphicEntity(Texture2D texture, Vector2 position, Color color, Vector2 scale, Rectangle bounds)
            : this(texture, position, color, scale)
        {
            _bounds = bounds;
        }
        #endregion

        /// <summary>
        /// Updates the entity's collision rect. (Override for additional behaviour)
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            _collisionRect = new Rectangle((int)Position.X - (_texture.Width / 2), (int)Position.Y - (_texture.Height / 2), (int)(_texture.Width*_scale.X), (int)(_texture.Height*_scale.X));
            base.Update(gameTime);
        }

        /// <summary>
        /// Returns whether or not the entity is out of it's bounds.
        /// </summary>
        public bool IsOutOfBounds { get { return !(_collisionRect.Intersects(_bounds)); } }

        /// <summary>
        /// Returns whether or not the entity is off-screen.
        /// </summary>
        public bool IsOffScreen { get { return !(_collisionRect.Intersects(Config.WindowBounds)); } }

        /// <summary>
        /// Returns true if the right of the collision rect is beyond it's right bound.
        /// </summary>
        public bool EdgeOfBoundsRight
        {
            get
            {
                if (_collisionRect.Right >= _bounds.Right)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Returns true if the right of the collision rect is beyond it's left bound.
        /// </summary>
        public bool EdgeOfBoundsLeft
        {
            get
            {
                if (_collisionRect.Left <= _bounds.Left)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Returns true if the right of the collision rect is beyond it's top bound.
        /// </summary>
        public bool EdgeOfBoundsTop
        {
            get
            {
                if (_collisionRect.Top <= _bounds.Top)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Returns true if the right of the collision rect is beyond it's bottom bound.
        /// </summary>
        public bool EdgeOfBoundsBottom
        {
            get
            {
                if (_collisionRect.Bottom >= _bounds.Bottom)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Returns true if the right of the collision rect is beyond the right window bound.
        /// </summary>
        public bool EdgeOfScreenRight
        {
            get
            {
                if (_collisionRect.Right >= Config.WindowBounds.Right)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Returns true if the left of the collision rect is beyond the left window bound.
        /// </summary>
        public bool EdgeOfScreenLeft
        {
            get
            {
                if (_collisionRect.Left <= Config.WindowBounds.Left)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Returns true if the top of the collision rect is beyond the top window bound.
        /// </summary>
        public bool EdgeOfScreenTop
        {
            get
            {
                if (_collisionRect.Top <= Config.WindowBounds.Top)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Returns true if the bottom of the collision rect is beyond the bottom window bound.
        /// </summary>
        public bool EdgeOfScreenBottom
        {
            get
            {
                if (_collisionRect.Bottom >= Config.WindowBounds.Bottom)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Gets or sets the entity's individual screen bounds.
        /// </summary>
        public Rectangle Bounds { get { return _bounds; } set { _bounds = value; } }
    }
}
