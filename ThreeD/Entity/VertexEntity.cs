using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HKFramework.ThreeD.Entity
{
    public class VertexEntity : Entity
    {
        public enum PredefinedObject
        {
            Plane
        };

        protected virtual VertexPositionColorNormal[] _vertices { get; set; }
        protected VertexBuffer _vertexBuffer;
        protected int[] _indices;
        protected IndexBuffer _indexBuffer;
        protected BasicEffect _effect;
        protected PrimitiveType _primType;
        protected int _primCount;

        public VertexEntity(Vector3 position, Vector3 rotation, PrimitiveType primitiveType)
            : base(position, rotation)
        {
            _effect = new BasicEffect(GameUtils.GetUtil<GraphicsDevice>());
            _primType = primitiveType;
        }

        public VertexEntity(Vector3 position, Vector3 rotation, PredefinedObject shape, Color color, Vector3 scale)
            : this(position, rotation, PrimitiveType.TriangleList)
        {
            if (shape == PredefinedObject.Plane)
            {
                _vertices = new VertexPositionColorNormal[6];

                _vertices[0].Position = new Vector3(-1.0f, 1.0f, 0.0f) * scale;
                _vertices[1].Position = new Vector3(1.0f, 1.0f, 0.0f) * scale;
                _vertices[2].Position = new Vector3(1.0f, -1.0f, 0.0f) * scale;
                _vertices[3].Position = new Vector3(-1.0f, 1.0f, 0.0f) * scale;
                _vertices[4].Position = new Vector3(1.0f, -1.0f, 0.0f) * scale;
                _vertices[5].Position = new Vector3(-1.0f, -1.0f, 0.0f) * scale;

                _vertices[0].Color = color;
                _vertices[1].Color = color;
                _vertices[2].Color = color;
                _vertices[3].Color = color;
                _vertices[4].Color = color;
                _vertices[5].Color = color;

                _vertices[0].Normal = new Vector3(0, 1, 0);
                _vertices[1].Normal = new Vector3(0, 1, 0);
                _vertices[2].Normal = new Vector3(0, 1, 0);
                _vertices[3].Normal = new Vector3(0, 1, 0);
                _vertices[4].Normal = new Vector3(0, 1, 0);
                _vertices[5].Normal = new Vector3(0, 1, 0);

                _indices = new int[] {0, 1, 2, 3, 4, 5};
                _primCount = 2;

                InitBuffers();
            }
        }

        public virtual void Draw()
        {
            _effect.World = RotationMatrix * Matrix.CreateTranslation(Position);
            _effect.View = Camera.View;
            _effect.Projection = Camera.Projection;
            _effect.VertexColorEnabled = true;
            _effect.EnableDefaultLighting();
            GameUtils.GetUtil<GraphicsDevice>().SetVertexBuffer(_vertexBuffer);
            GameUtils.GetUtil<GraphicsDevice>().Indices = _indexBuffer;

            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GameUtils.GetUtil<GraphicsDevice>().DrawUserIndexedPrimitives(_primType, _vertices, 0, _vertices.Length, _indices, 0, _primCount, VertexPositionColorNormal.VertexDeclaration);
            }
        }

        protected virtual void InitBuffers()
        {
            _vertexBuffer = new VertexBuffer(GameUtils.GetUtil<GraphicsDevice>(), VertexPositionColorNormal.VertexDeclaration, _vertices.Length, BufferUsage.None);
            _vertexBuffer.SetData<VertexPositionColorNormal>(_vertices);
            _indexBuffer = new IndexBuffer(GameUtils.GetUtil<GraphicsDevice>(), IndexElementSize.ThirtyTwoBits, _indices.Length, BufferUsage.None);
            _indexBuffer.SetData<int>(_indices);
        }
    }
}
