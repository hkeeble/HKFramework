using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HKFramework.ThreeD.Entity
{
    public class TexturedVertexEntity : VertexEntity
    {
        protected new VertexPositionNormalTexture[] _vertices { get; set; }
        protected Texture2D _texture;

        public TexturedVertexEntity(Vector3 position, Vector3 rotation, GraphicsDevice graphics, PrimitiveType primitiveType, Texture2D texture, Vector3 scale)
            : base(position, rotation, primitiveType)
        {
            _texture = texture;
        }

        public TexturedVertexEntity(Vector3 position, Vector3 rotation, PredefinedObject shape, Color color, Vector3 scale, Texture2D texture)
            : base(position, rotation, PrimitiveType.TriangleList)
        {
            if (shape == PredefinedObject.Plane)
            {
                _texture = texture;
                _vertices = new VertexPositionNormalTexture[6];

                _vertices[0].Position = new Vector3(-1.0f, 1.0f, 0.0f) * scale;
                _vertices[1].Position = new Vector3(1.0f, 1.0f, 0.0f) * scale;
                _vertices[2].Position = new Vector3(1.0f, -1.0f, 0.0f) * scale;
                _vertices[3].Position = new Vector3(-1.0f, 1.0f, 0.0f) * scale;
                _vertices[4].Position = new Vector3(1.0f, -1.0f, 0.0f) * scale;
                _vertices[5].Position = new Vector3(-1.0f, -1.0f, 0.0f) * scale;

                _vertices[0].TextureCoordinate = new Vector2(0, 0);
                _vertices[1].TextureCoordinate = new Vector2(1, 0);
                _vertices[2].TextureCoordinate = new Vector2(1, 1);
                _vertices[3].TextureCoordinate = new Vector2(0, 0);
                _vertices[4].TextureCoordinate = new Vector2(1, 1);
                _vertices[5].TextureCoordinate = new Vector2(0, 1);

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

        protected override void InitBuffers()
        {
            _vertexBuffer = new VertexBuffer(GameUtils.GetUtil<GraphicsDevice>(), VertexPositionNormalTexture.VertexDeclaration, _vertices.Length, BufferUsage.None);
            _vertexBuffer.SetData<VertexPositionNormalTexture>(_vertices);
            _indexBuffer = new IndexBuffer(GameUtils.GetUtil<GraphicsDevice>(), IndexElementSize.ThirtyTwoBits, _indices.Length, BufferUsage.None);
            _indexBuffer.SetData<int>(_indices);
        }

        public override void Draw()
        {
            //_effect.World = RotationMatrix * Matrix.CreateTranslation(Position);
            //_effect.View = Camera.View;
            //_effect.Projection = Camera.Projection;
            //_effect.EnableDefaultLighting();
            //_effect.VertexColorEnabled = false;
            //_effect.TextureEnabled = true;
            //_effect.Texture = _texture;
            //GameUtils.GetUtil<GraphicsDevice>().SetVertexBuffer(_vertexBuffer);
            //GameUtils.GetUtil<GraphicsDevice>().Indices = _indexBuffer;

            //foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            //{
            //    pass.Apply();
            //    GameUtils.GetUtil<GraphicsDevice>().DrawUserIndexedPrimitives(_primType, _vertices, 0, _vertices.Length, _indices, 0, _primCount, VertexPositionNormalTexture.VertexDeclaration);
            //}
        }
    }
}
