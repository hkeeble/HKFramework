using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HKFramework.TwoD
{
    public class Graphic
    {
        #region Declarations
        public Vector2 Position;
        protected Texture2D _texture;
        protected SpriteEffects _currentEffect;
        protected float _rotation;
        protected Vector2 _scale;
        protected Color _drawColor;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates new entity that is drawable on-screen.
        /// </summary>
        /// <param name="texture">The texture to use for the entity.</param>
        /// <param name="position">The entity's initial position.</param>
        public Graphic(Texture2D texture, Vector2 position)
        {
            _scale = Vector2.One;
            _drawColor = Color.White;
            _texture = texture;
            Position = position;
            _currentEffect = SpriteEffects.None;
            _rotation = 0;
        }

        /// <summary>
        /// Creates new entity that is drawable on-screen.
        /// </summary>
        /// <param name="texture">The texture to use for the entity.</param>
        /// <param name="position">The entity's initial position.</param>
        /// <param name="scale">The scale of the graphic.</param>
        public Graphic(Texture2D texture, Vector2 position, float scale) : this(texture, position, new Vector2(scale, scale)) { }

        /// <summary>
        /// Creates new entity that is drawable on-screen.
        /// </summary>
        /// <param name="texture">The texture to use for the entity.</param>
        /// <param name="position">The entity's initial position.</param>
        /// <param name="scale">The scale of the graphic.</param>
        public Graphic(Texture2D texture, Vector2 position, Vector2 scale)
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
        public Graphic(Texture2D texture, Vector2 position, Color color)
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
        public Graphic(Texture2D texture, Vector2 position, float scale, Color color) : this(texture, position, color, new Vector2(scale, scale)) { }

        /// <summary>
        /// Creates new entity that is drawable on-screen.
        /// </summary>
        /// <param name="texture">The texture to use for the entity.</param>
        /// <param name="position">The entity's initial position.</param>
        /// <param name="scale">The scale of the graphic.</param>
        /// <param name="color">The draw color of the graphic.</param>
        public Graphic(Texture2D texture, Vector2 position, Color color, Vector2 scale)
            : this(texture, position)
        {
            _drawColor = color;
            _scale = scale;
        }
        #endregion

        /// <summary>
        /// Draws the graphic to the screen.
        /// </summary>
        public virtual void Draw()
        {
            GameUtils.GetUtil<SpriteBatch>().Draw(_texture, Position, null, _drawColor, _rotation, Center, _scale, _currentEffect, 1f);
        }

        /// <summary>
        /// Updates the graphic. (Override for additional behaviour)
        /// </summary>
        /// <param name="gameTime">The game time elapsed since the last update call.</param>
        public virtual void Update(GameTime gameTime)
        {
            
        }

        /// <summary>
        /// The current effect being applied to the graphic on drawing.
        /// </summary>
        public SpriteEffects SpriteEffect { set { _currentEffect = value; } }

        /// <summary>
        /// The graphic's texture.
        /// </summary>
        public Texture2D Texture { get { return _texture; } }

        /// <summary>
        /// The graphic's center.
        /// </summary>
        protected virtual Vector2 Center { get { return new Vector2(_texture.Width / 2, _texture.Height / 2); } }

        /// <summary>
        /// Gets or sets the scale of the graphic.
        /// </summary>
        public Vector2 Scale { get { return _scale; } set { _scale = value; } }

        /// <summary>
        /// Gets or sets the draw color of the graphic.
        /// </summary>
        public Color Color { get { return _drawColor; } set { _drawColor = value; } }

        /// <summary>
        /// The graphic's origin, relative to position.
        /// </summary>
        public Vector2 Origin { get { return Position + Center; } }
    }
}
