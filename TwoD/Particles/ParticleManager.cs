using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HKFramework.TwoD.Particles
{
    public class ParticleManager
    {
        private List<Emitter> _emitters;

        public ParticleManager()
        {
            _emitters = new List<Emitter>();
        }

        public void AddEmitter(Emitter emitter)
        {
            _emitters.Add(emitter);
        }

        public void Clear()
        {
            _emitters.Clear();
        }

        public void Draw()
        {
            for (int i = 0; i < _emitters.Count; i++)
                _emitters[i].Draw();
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < _emitters.Count; i++)
            {
                _emitters[i].Update(gameTime);
                if (_emitters[i].Disposable && _emitters[i].ParticleCount == 0)
                    _emitters.Remove(_emitters[i]);
            }
        }
    }
}
