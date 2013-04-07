using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HKFramework.Text
{
    /// <summary>
    /// Represents a text object that is not a part of the HUD.
    /// </summary>
    public class TextObject
    {
        #region Declarations
        public Vector2 Position;
        private string _text;
        protected SpriteFont _font;
        protected Color _drawColor;
        #endregion

        public TextObject(Vector2 position, string text, SpriteFont font)
        {
            Position = position;
            _text = text;
            _font = font;
            _drawColor = Color.White;
        }

        public TextObject(Vector2 position, string text, SpriteFont font, Color color) : this(position, text, font)
        {
            _drawColor = color;
        }

        public virtual void Draw()
        {
            GameUtils.GetUtil<SpriteBatch>().DrawString(_font, _text, Position-(_font.MeasureString(_text)/2), _drawColor);
        }

        public virtual void Update()
        {

        }

        /// <summary>
        /// Gets of sets the text objects text.
        /// </summary>
        public string Text { get { return _text; } set { _text = value; } }

        /// <summary>
        /// Gets or sets the color of the text object.
        /// </summary>
        public Color Color { get { return _drawColor; } set { _drawColor = value;}  }

        /// <summary>
        /// Gets the width of the text object.
        /// </summary>
        public float Width { get { return _font.MeasureString(Text).X; } }

        /// <summary>
        /// Gets the height of the text object.
        /// </summary>
        public float Height { get { return _font.MeasureString(Text).Y; } }
    }
}
