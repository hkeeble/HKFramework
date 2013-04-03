using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HKFramework.TwoD.Particles
{
    class Particle
    {
        #region Declarations
        private Texture2D _particleBase; // Base texture used to represent the particle
        private Vector2 _position;
        private Vector2 _direction;
        private float _speed;
        private float _radius;
        private Color _color;
        private TimeSpan _life; // Time remaining
        #endregion

        /// <summary>
        /// Instantiates a new particle
        /// </summary>
        /// <param name="particleBase">The base texture for the particle.</param>
        /// <param name="position">The initial position of the particle.</param>
        /// <param name="direction">The direction the particle moves.</param>
        /// <param name="radius">The radius/scale of the particle.</param>
        /// <param name="color">The color of the particle.</param>
        /// <param name="life">The lifespan of the particle in seconds.</param>
        /// <param name="speed">The speed the particle moves at.</param>
        public Particle(Texture2D particleBase, Vector2 position, Vector2 direction, float radius, Color color, TimeSpan life, float speed)
        {
            _particleBase = particleBase;
            _position = position;
            _direction = direction;
            _speed = speed;
            _radius = radius;
            _color = color;
            _life = life;
        }

        /// <summary>
        /// Updates the particle, moving it and subtracting the correct value from it's life.
        /// </summary>
        /// <param name="timeSinceUpdate">The seconds since the last update.</param>
        /// <returns>Returns true if the particle is still within it's lifetime.</returns>
        public bool Update(GameTime gameTime)
        {
            _position += _direction * _speed;
            _life -= gameTime.ElapsedGameTime;
            if (_life > TimeSpan.Zero)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Draws the particle to the screen.
        /// </summary>
        public void Draw()
        {
            GameUtils.GetUtil<SpriteBatch>().Begin();
            GameUtils.GetUtil<SpriteBatch>().Draw(_particleBase, _position, null, _color, 0f, Vector2.Zero, _radius, SpriteEffects.None, 1f);
            GameUtils.GetUtil<SpriteBatch>().End();
        }
    }
}
