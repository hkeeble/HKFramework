using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HKFramework.TwoD.Particles
{
    public class Emitter
    {
        private Vector2 _position; // Position of the emitter
        private Texture2D _particleBase; // Represents the base particle for all particles from this emitter
        private Color _particleColor; // The color of individual particles
        private Range _sizeRange; // The range of sizes given to particles
        private Range _lifeRange; // Range used to determine lifespan in seconds
        private Range _speedRange; // The range of speeds applied to particles
        private Range _maxDistance; // The range of possible maximum distances a particle can be from the emitter
        private Vector2 _dirBoundA, _dirBoundB; // Used to store bounds for the direction particles move
        private bool _dirBounds; // Whether or not direction bounds are supplied

        private TimeSpan _lifeSpan; // The life-span of the emitter

        public bool Disposable = false; // Shows whether or not the emitter is dispoable

        // Time-keepers
        private float _secondsBetweenSpawn;
        TimeSpan _timeSinceSpawn;

        public int MaxParticles;
       
        private List<Particle> _particles; // Currently active particles

        Random random;

        public Emitter(Vector2 position, Texture2D particleBase, Color particleColor, Range sizeRange, int maxParticles, float secondsBetweenSpawn,
            Vector2 directionBoundA, Vector2 directionBoundB, Range lifeRange, TimeSpan lifeSpan, Range speedRange, Range maxDistance)
        {
            _position = position;
            _particleBase = particleBase;
            _particleColor = particleColor;
            _sizeRange = sizeRange;
            _speedRange = speedRange;
            MaxParticles = maxParticles;

            _lifeSpan = lifeSpan;
            _lifeRange = lifeRange;
            _timeSinceSpawn = TimeSpan.Zero;
            _secondsBetweenSpawn = secondsBetweenSpawn;

            _dirBoundA = directionBoundA;
            _dirBoundB = directionBoundB;
            _dirBounds = true;

            _maxDistance = maxDistance;

            _particles = new List<Particle>();
            random = new Random(DateTime.Now.Millisecond);
        }

        public Emitter(Vector2 position, Texture2D particleBase, Color particleColor, Range sizeRange, int maxParticles, float secondsBetweenSpawn, Range lifeRange, TimeSpan lifeSpan,
            Range speedRange, Range maxDistance) : this(position, particleBase, particleColor, sizeRange, maxParticles, secondsBetweenSpawn, Vector2.Zero, Vector2.Zero, lifeRange, lifeSpan,
            speedRange, maxDistance)
        {
            _dirBounds = false;
        }

        public void Update(GameTime gameTime)
        {
            if (Disposable == false)
            {
                _lifeSpan -= gameTime.ElapsedGameTime;
                if (_lifeSpan <= TimeSpan.Zero)
                    Disposable = true;

                if (_particles.Count < MaxParticles)
                {
                    _timeSinceSpawn += gameTime.ElapsedGameTime;

                    if (_timeSinceSpawn > TimeSpan.FromSeconds(_secondsBetweenSpawn))
                    {
                        _timeSinceSpawn = TimeSpan.Zero;

                        if (_dirBounds)
                        {
                            _particles.Add(new Particle(this, _particleBase, _position, Vector2.Lerp(_dirBoundA, _dirBoundB, (float)random.NextDouble()),
                                Maths.LinearInterpolate(_sizeRange.Minimum, _sizeRange.Maximum, random.NextDouble()), _particleColor,
                                new TimeSpan(0, 0, (int)Maths.LinearInterpolate(_lifeRange.Maximum, _lifeRange.Maximum, random.NextDouble())),
                                Maths.LinearInterpolate(_speedRange.Minimum, _speedRange.Maximum, random.NextDouble()), _maxDistance));
                        }
                        else
                        {
                            int dirMultiplier = random.Next(-1, 2);
                            _particles.Add(new Particle(this, _particleBase, _position, new Vector2((float)random.NextDouble() * dirMultiplier, (float)random.NextDouble() * dirMultiplier),
                                            Maths.LinearInterpolate(_sizeRange.Minimum, _sizeRange.Maximum, random.NextDouble()), _particleColor,
                                            new TimeSpan(0, 0, (int)Maths.LinearInterpolate(_lifeRange.Maximum, _lifeRange.Maximum, random.NextDouble())),
                                            Maths.LinearInterpolate(_speedRange.Minimum, _speedRange.Maximum, random.NextDouble()), _maxDistance));
                        }
                    }
                }
            }

            for (int i = 0; i < _particles.Count; i++)
            {
                if (_particles[i].Update(gameTime) == false)
                    _particles.Remove(_particles[i]);
            }
        }

        public void Draw()
        {
            for (int i = 0; i < _particles.Count; i++)
                _particles[i].Draw();
        }

        public int ParticleCount { get { return _particles.Count; } }
        public Vector2 Position { get { return _position; } }
    }
}
