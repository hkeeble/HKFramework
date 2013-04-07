using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HKFramework.Text
{
    /// <summary>
    /// Represents a text entity that can interact in physical space with a collision rectangle, and also contains a direction
    /// </summary>
    public class TextEntity : TextObject
    {
        #region Declarations
        public Vector2 Direction;
        public float MoveSpeed;
        private Rectangle _collisionRect;
        #endregion

        /// <summary>
        /// Constructs a new TextEntity, a text object with a collision rect and a direction.
        /// </summary>
        /// <param name="position">Position of the entity.</param>
        /// <param name="text">Text to display in the entity.</param>
        /// <param name="font">Font for the entity to use.</param>
        public TextEntity(Vector2 position, string text, SpriteFont font) : base(position, text, font)
        { Direction = Vector2.Zero; MoveSpeed = 1f;  }

        /// <summary>
        /// Constructs a new TextEntity, a text object with a collision rect and a direction.
        /// </summary>
        /// <param name="position">Position of the entity.</param>
        /// <param name="text">Text to display in the entity.</param>
        /// <param name="font">Font for the entity to use.</param>
        /// <param name="color">Color of the text.</param>
        public TextEntity(Vector2 position, string text, SpriteFont font, Color color) : this(position, text, font)
        { _drawColor = color;}

        /// <summary>
        /// Constructs a new TextEntity, a text object with a collision rect and a direction.
        /// </summary>
        /// <param name="position">Position of the entity.</param>
        /// <param name="text">Text to display in the entity.</param>
        /// <param name="font">Font for the entity to use.</param>
        /// <param name="direction">Initial direction for the text to move.</param>
        public TextEntity(Vector2 position, string text, SpriteFont font, Vector2 direction) : this(position, text, font)
        { Direction = direction; }

        /// <summary>
        /// Constructs a new TextEntity, a text object with a collision rect and a direction.
        /// </summary>
        /// <param name="position">Position of the entity.</param>
        /// <param name="text">Text to display in the entity.</param>
        /// <param name="font">Font for the entity to use.</param>
        /// <param name="direction">Initial direction for the text to move.</param>
        /// <param name="color">Color of the text.</param>
        public TextEntity(Vector2 position, string text, SpriteFont font, Color color, Vector2 direction) : this(position, text, font, direction)
        { _drawColor = color; }

        /// <summary>
        /// Constructs a new TextEntity, a text object with a collision rect and a direction.
        /// </summary>
        /// <param name="position">Position of the entity.</param>
        /// <param name="text">Text to display in the entity.</param>
        /// <param name="font">Font for the entity to use.</param>
        /// <param name="moveSpeed">Speed the text moves at.</param>
        public TextEntity(Vector2 position, string text, SpriteFont font, float moveSpeed) : this(position, text, font)
        { MoveSpeed = moveSpeed; }

        /// <summary>
        /// Constructs a new TextEntity, a text object with a collision rect and a direction.
        /// </summary>
        /// <param name="position">Position of the entity.</param>
        /// <param name="text">Text to display in the entity.</param>
        /// <param name="font">Font for the entity to use.</param>
        /// <param name="direction">Initial direction for the text to move.</param>
        /// <param name="color">Color of the text.</param>
        /// <param name="moveSpeed">Speed the text moves at.</param>
        public TextEntity(Vector2 position, string text, SpriteFont font, Vector2 direction, Color color, float moveSpeed) : this(position, text, font, color, direction)
        { MoveSpeed = moveSpeed; }

        /// <summary>
        /// Updates the entity's collision rectangle and moves it in the given direction. (Override for additional behaviour)
        /// </summary>
        public override void Update()
        {
            Position += Direction * MoveSpeed;
            _collisionRect = new Rectangle((int)Position.X, (int)Position.Y, (int)Width, (int)Height);
        }

        /// <summary>
        /// Returns whether or not the text object is off screen.
        /// </summary>
        public bool IsOffScreen { get { return !(_collisionRect.Intersects(Config.WindowBounds)); } }
    }
}
