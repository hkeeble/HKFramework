using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HKFramework.ThreeD.Entity
{
    public class ModelEntity : Entity
    {
        protected Model _model;
        protected Vector3 _scale;

        public ModelEntity(Model model, Vector3 position, Vector3 rotation, Vector3 scale)
            : base(position, rotation)
        {
            _model = model;
            _scale = scale;
        }

        public virtual void Draw()
        {
            Matrix[] transforms = new Matrix[_model.Bones.Count];
            _model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh m in _model.Meshes)
            {
                foreach (BasicEffect e in m.Effects)
                {
                    e.EnableDefaultLighting();
                    e.World = Matrix.CreateScale(_scale) * transforms[m.ParentBone.Index] * RotationMatrix * Matrix.CreateTranslation(Position);
                    e.View = Camera.View;
                    e.Projection = Camera.Projection;
                }
                m.Draw();
            }
        }

        public BoundingSphere BoundingSphere
        {
            get
            {
                BoundingSphere BSphere = _model.Meshes[0].BoundingSphere;
                BSphere.Center += Position;
                return BSphere;
            }
        }

        public Model model { get { return _model; } }

        /// <summary>
        /// Returns the collective size of all the model's meshes, using it's boundingsphere radius as measurement.
        /// </summary>
        public float Size
        {
            get
            {
                float size = 0.0f;
                foreach (ModelMesh m in _model.Meshes)
                {
                    size += (m.BoundingSphere.Radius * 2);
                }
                return size;
            }
        }
    }
}
