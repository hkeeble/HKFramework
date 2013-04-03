using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HKFramework.TwoD.Entity
{
    public class SpriteEntity
    {
        protected Vector2 _position;
        protected Texture2D _texture;
        protected Rectangle _collisionRect;
        protected SpriteEffects _currentEffect;
        protected float _rotation;

        public SpriteEntity(Texture2D texture, Vector2 position)
        {
            _position = position;
            _texture = texture;
            _currentEffect = SpriteEffects.None;
            _rotation = 0;
        }

        public virtual void Draw()
        {
            GameUtils.GetUtil<SpriteBatch>().Draw(_texture, _position, null, Color.White, _rotation, Vector2.Zero, 1f, _currentEffect, 1f);
        }

        public virtual void Update(GameTime gameTime)
        {
            _collisionRect = new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
        }

        public Vector2 Position { get { return _position; } set { _position = value; } }
        public SpriteEffects SpriteEffect { set { _currentEffect = value; } }
        public Texture2D Texture { get { return _texture; } }
    }
}
