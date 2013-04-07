using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace HKFramework
{
    /// <summary>
    /// A rectangle that can be rotated and uses the SAT algorithm for collision detection.
    /// </summary>
    public class SATRectangle
    {
        private Rectangle _collisionRectangle;
        private float _rotation;
        public Vector2 Origin;

        public SATRectangle(Rectangle initialRect, float initialRotation)
        {
            _collisionRectangle = initialRect;
            _rotation = MathHelper.ToRadians(initialRotation);
            Origin = new Vector2((int)initialRect.Width / 2, (int)initialRect.Height / 2);
        }

        public bool Intersects(Rectangle otherEntity)
        {
            return Intersects(new SATRectangle(otherEntity, 0f));
        }

        public bool Intersects(SATRectangle otherEntity)
        {
            // Calculate the axes to be projected
            List<Vector2> axes = new List<Vector2>();
            axes.Add(TopRightCorner - TopLeftCorner);
            axes.Add(TopRightCorner - BottomRightCorner);
            axes.Add(otherEntity.TopLeftCorner - otherEntity.BottomLeftCorner);
            axes.Add(otherEntity.TopLeftCorner - otherEntity.TopRightCorner);

            foreach (Vector2 axis in axes)
            {
                if (!AxisCollision(axis, otherEntity))
                    return false;
            }
            return true;
        }

        public bool AxisCollision(Vector2 axis, SATRectangle otherEntity)
        {
            List<int> ScalarsA = new List<int>();
            List<int> ScalarsB = new List<int>();

            ScalarsA.Add(GenerateScalar(otherEntity.TopLeftCorner, axis));
            ScalarsA.Add(GenerateScalar(otherEntity.TopRightCorner, axis));
            ScalarsA.Add(GenerateScalar(otherEntity.BottomLeftCorner, axis));
            ScalarsA.Add(GenerateScalar(otherEntity.BottomRightCorner, axis));

            ScalarsB.Add(GenerateScalar(TopLeftCorner, axis));
            ScalarsB.Add(GenerateScalar(TopRightCorner, axis));
            ScalarsB.Add(GenerateScalar(BottomLeftCorner, axis));
            ScalarsB.Add(GenerateScalar(BottomRightCorner, axis));

            int MinA = ScalarsA.Min();
            int MaxA = ScalarsA.Max();
            int MinB = ScalarsB.Min();
            int MaxB = ScalarsB.Max();

            if (MinB <= MaxA && MaxB >= MaxA)
                return true;
            else if (MinA <= MaxB && MaxA >= MaxB)
                return true;

            return false;
        }

        private int GenerateScalar(Vector2 RectCorner, Vector2 axis)
        {
            // Vector Projection - project corner onto axis
            float dotProduct = Vector2.Dot(RectCorner, axis);
            float magnitude = (axis.X * axis.X) + (axis.Y * axis.Y);
            Vector2 projection = new Vector2((dotProduct / magnitude) * axis.X, (dotProduct / magnitude) * axis.Y); // Multiply by axis to create projection

            // Scale projection to axis for easier comparison
            return (int)((axis.X * projection.X) + (axis.Y * projection.Y));
        }

        /// <summary>
        /// Moves the rectangle.
        /// </summary>
        public void Move(int X, int Y)
        {
            _collisionRectangle.X += X;
            _collisionRectangle.Y += Y;
        }

        /// <summary>
        /// Rotates the rectangle around it's origin.
        /// </summary>
        public void Rotate(float angle)
        {
            angle = MathHelper.ToRadians(angle); // Convert the given angle into radians
            _rotation += angle;
        }

        /// <summary>
        /// Used to rotate a point around a given origin.
        /// </summary>
        private Vector2 RotatePoint(Vector2 point, Vector2 origin, float rotation)
        {
            return new Vector2(
                (float)(origin.X + (point.X - origin.X) * Math.Cos(rotation) - (point.Y - origin.Y) * Math.Sin(rotation)),
                (float)(origin.Y + (point.Y - origin.Y) * Math.Cos(rotation) + (point.X - origin.X) * Math.Sin(rotation)));
        }

        private Vector2 TopLeftCorner
        { get { return RotatePoint(new Vector2(_collisionRectangle.Left, _collisionRectangle.Top), new Vector2(_collisionRectangle.Left, _collisionRectangle.Top) + Origin, _rotation); } }
        private Vector2 TopRightCorner
        { get { return RotatePoint(new Vector2(_collisionRectangle.Right, _collisionRectangle.Top), new Vector2(_collisionRectangle.Right, _collisionRectangle.Top) + new Vector2(-Origin.X, Origin.Y), _rotation); } }
        private Vector2 BottomLeftCorner
        { get { return RotatePoint(new Vector2(_collisionRectangle.Left, _collisionRectangle.Bottom), new Vector2(_collisionRectangle.Left, _collisionRectangle.Bottom) + new Vector2(Origin.X, -Origin.Y), _rotation); } }
        private Vector2 BottomRightCorner
        { get { return RotatePoint(new Vector2(_collisionRectangle.Right, _collisionRectangle.Bottom), new Vector2(_collisionRectangle.Right, _collisionRectangle.Bottom) + new Vector2(-Origin.X, -Origin.Y), _rotation); } }

        public int Width { get { return _collisionRectangle.Width; } }
        public int Height { get { return _collisionRectangle.Height; } }
        public int X { get { return _collisionRectangle.X; } }
        public int Y { get { return _collisionRectangle.Y; } }
        public Rectangle CollisionRectangle { get { return _collisionRectangle; } }
        public float Rotation { get { return _rotation; } }
    }
}
