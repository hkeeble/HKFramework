using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HKFramework.ThreeD
{
    public class Camera : Microsoft.Xna.Framework.GameComponent
    {
        private static Vector3 _position;
        private static float _aspectRatio;
        private static Matrix _view;
        private static Matrix _projection;
        private static Vector3 _up;
        private static Vector3 _target;
        private static Vector3 _direction;

        #region Game Specific
        private static bool _shakeActive;
        private static float _shakeStrength;
        private static Vector3 _shakeOffset;
        private static Random rand;
        private static BoundingSphere _boundingSphere;
        private static float _playerHeight;
        #endregion

        private static float _speed;

        public Camera(Game game, Vector3 position, Vector3 direction, Vector3 up, float moveSpeed)
            : base(game)
        {
            _aspectRatio = GameUtils.GetUtil<GraphicsDevice>().Viewport.AspectRatio;
            _position = position;
            _up = up;
            _direction = direction;
            _direction.Normalize();
            _speed = moveSpeed;

            _playerHeight = position.Y;

            CreateLookAt();
            rand = new Random(DateTime.Now.Millisecond);
            _projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), _aspectRatio, 1, 1500);
        }
        
        private float NextFloat()
        {
            return (float)rand.NextDouble();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (_shakeActive)
            {
                _shakeOffset = new Vector3(NextFloat(), NextFloat(), NextFloat() * _shakeStrength);
                _direction += _shakeOffset;
            }
            CreateLookAt();

            base.Update(gameTime);
        }

        public static void SetPosition(Vector3 newPosition)
        {
            _position = newPosition;
            _direction = _target - _position;
            _direction.Normalize();
        }

        public static void SetDirection(Vector3 newDirection)
        {
            _direction = newDirection;
            _direction.Normalize();
        }

        public static void Move(float direction)
        {
            _position += (_direction * direction) * _speed;
        }

        public static void Rotate(Vector3 rotation)
        {
            _direction = Vector3.Transform(_direction, Matrix.CreateFromAxisAngle(_up, rotation.Y));
            _direction = Vector3.Transform(_direction, Matrix.CreateFromAxisAngle(Vector3.Cross(_up, _direction), rotation.X));
            _direction.Normalize();
        }

        public static void RotateY(float rotation)
        {
            _direction = Vector3.Transform(_direction, Matrix.CreateRotationY(MathHelper.ToRadians(rotation)));
            _direction.Normalize();
        }

        private void CreateLookAt()
        {
            _view = Matrix.CreateLookAt(_position, _position + _direction, _up);
        }

        public static void ToggleShake()
        {
            _shakeActive = true;
        }

        public static Vector3 Target { get { return _target; } set { _target = value; } }
        public static Vector3 Position { get { return _position; } }
        public static float AspectRatio { get { return _aspectRatio; } set { _aspectRatio = value; } }
        public static Matrix View { get { return _view; } set { _view = value; } }
        public static Matrix Projection { get { return _projection; } set { _projection = value; } }
        public static Vector3 Direction { get { return _direction; } }

        public static BoundingSphere BoundingSphere { get { return _boundingSphere; } }
        public static float PlayerHeight { get { return _playerHeight; } }
        public static float ShakeStrength { set { _shakeStrength = value; } }
    }
}