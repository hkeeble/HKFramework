using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using HKFramework;
using HKFramework.MathLib;
using HKFramework.Kinect;
using HKFramework.HUD;

namespace HKFramework.Kinect
{
    public struct LimbCollisionData
    {
        private bool _colliding;
        private Vector2 _velocity;
        private float _speed;
        private BodyPart _bodyPart;

        public LimbCollisionData(bool colliding, Vector2 velocity, BodyPart bodyPart = BodyPart.None)
        {
            _colliding = colliding;
            _velocity = velocity;
            _bodyPart = bodyPart;

            // Calculate the speed of the limb
            _speed = 0;
            if (_velocity.X < 0)
                _speed += (_velocity.X * -1);
            else
                _speed += _velocity.X;
            if (_velocity.Y < 0)
                _speed += (_velocity.Y * -1);
            else
                _speed += _velocity.Y;
        }

        public bool Colliding { get { return _colliding; } }
        public Vector2 Velocity { get { return _velocity; } }
        public float Speed { get { return _speed; } }
        public BodyPart BodyPart { get { return _bodyPart; } }
    }

    public enum BodyPart
    {
        None,
        Body,
        Head,
        Torso,
        LeftForeArm,
        LeftUpperArm,
        RightForeArm,
        RightUpperArm,
        LeftThigh,
        RightThigh,
        LeftShin,
        RightShin
    }

    class SkeletonCollisionTree
    {
        public struct Limb
        {
            private SATRectangle _collisionRect;
            private int _width;
            private Texture2D _texture;
            private Color _drawColor;
            
            private Vector2 _currentMidPoint, _prevMidPoint, _velocity;

            public Limb(int width, Texture2D texture)
            {
                _drawColor = Color.White;
                _width = width;
                _collisionRect = new SATRectangle(Rectangle.Empty, 0f);
                _texture = texture;

                _currentMidPoint = Vector2.Zero;
                _prevMidPoint = Vector2.Zero;
                _velocity = Vector2.Zero;
            }

            public void Update(Vector2 JointA, Vector2 JointB)
            {
                _prevMidPoint = _currentMidPoint;
                _currentMidPoint = MathLibrary.MidPoint(JointA, JointB);
                _currentMidPoint.X -= _width / 2;
                _currentMidPoint.Y -= Vector2.Distance(JointA, JointB) / 2;

                // Calculate new collision rectangle
                _collisionRect = new SATRectangle(new Rectangle((int)_currentMidPoint.X, (int)_currentMidPoint.Y, 60, (int)Vector2.Distance(JointA,
                    JointB)), MathLibrary.AngleBetween(JointA, JointB));

                // Calculate velocity of limb
                _velocity = _currentMidPoint - _prevMidPoint;
            }

            public void Draw()
            {
                Rectangle posAdjusted = new Rectangle(_collisionRect.X + (_collisionRect.Width / 2),
                                                      _collisionRect.Y + (_collisionRect.Height / 2),
                                                      _collisionRect.Width, _collisionRect.Height);

                GameUtils.GetUtil<SpriteBatch>().Draw(_texture, posAdjusted, new Rectangle(0, 0, 2, 6), _drawColor, _collisionRect.Rotation,
                    new Vector2(2 / 2, 6 / 2), SpriteEffects.None, 1f);
            }

            public bool Collides(Rectangle rect)
            {
                if (_collisionRect.Intersects(rect))
                {
                    _drawColor = Color.Purple;
                    return true;
                }
                else
                {
                    _drawColor = Color.White;
                    return false;
                }
            }

            public bool Collides(SATRectangle rect)
            {
                if (_collisionRect.Intersects(rect))
                {
                    _drawColor = Color.Purple;
                    return true;
                }
                else
                {
                    _drawColor = Color.White;
                    return false;
                }
            }

            public Vector2 Velocity { get { return _velocity; } }
        }

        public Rectangle Body;
        public Rectangle Head;
        public Rectangle Torso;
        public Limb LeftForeArm;
        public Limb LeftUpperArm;
        public Limb RightForeArm;
        public Limb RightUpperArm;
        public Limb LeftThigh;
        public Limb RightThigh;
        public Limb LeftShin;
        public Limb RightShin;

        private Texture2D HeadTexture;
        private Texture2D MainTexture;

        // Used to calculate velocity for non-limb body parts
        private Vector2 _headPrevPosition, _torsoPrevPosition;
        private Vector2 _headCurrentPosition, _torsoCurrentPosition;
        private Vector2 _headVelocity, _torsoVelocity;

        /// <summary>
        /// Constructs a new tree to be used to represent a player's limbs for collision.
        /// </summary>
        public SkeletonCollisionTree(ContentManager content)
        {
            MainTexture = content.Load<Texture2D>("2DAssets/Sprites/Player/bodyTex");
            HeadTexture = content.Load<Texture2D>("2DAssets/Sprites/Player/Head");

            Color[] data = new Color[MainTexture.Width * MainTexture.Height];
            MainTexture.GetData<Color>(data);
            for (int i = 0; i < data.Length; i++)
                data[i].A = HUD.HUD.HUD_ALPHA;
            MainTexture.SetData<Color>(data);

            data = new Color[HeadTexture.Width * HeadTexture.Height];
            HeadTexture.GetData<Color>(data);
            for (int i = 0; i < data.Length; i++)
                data[i].A = HUD.HUD.HUD_ALPHA;
            HeadTexture.SetData<Color>(data);

            LeftForeArm = new Limb(40, MainTexture);
            LeftUpperArm = new Limb(40, MainTexture);
            RightForeArm = new Limb(40, MainTexture);
            RightUpperArm = new Limb(40, MainTexture);
            LeftThigh = new Limb(40, MainTexture);
            RightThigh = new Limb(40, MainTexture);
            LeftShin = new Limb(40, MainTexture);
            RightShin = new Limb(40, MainTexture);

            // Initialize non-limb body parts
            _headPrevPosition = Vector2.Zero;
            _torsoPrevPosition = Vector2.Zero;
            _headCurrentPosition = Vector2.Zero;
            _torsoCurrentPosition = Vector2.Zero;
            _headVelocity = Vector2.Zero;
            _torsoVelocity = Vector2.Zero;
        }

        public void Update()
        {
            // Update head
            int width = (int)SkeletonTracker.Joints.RightShoulder.X - (int)SkeletonTracker.Joints.LeftShoulder.X;
            int height = ((int)SkeletonTracker.Joints.LeftShoulder.Y - (int)SkeletonTracker.Joints.Head.Y);
            Head = new Rectangle((int)SkeletonTracker.Joints.Head.X - (width / 2), (int)SkeletonTracker.Joints.Head.Y - (height / 2), width, height);

            _headPrevPosition = _headCurrentPosition;
            _headVelocity = _headCurrentPosition - _headPrevPosition;

            // Update Body
            Body = new Rectangle((int)SkeletonTracker.Joints.LeftMostPoint, (int)SkeletonTracker.Joints.HighestPoint + (Head.Height / 2),
                                   SkeletonTracker.Joints.RightMostPoint - SkeletonTracker.Joints.LeftMostPoint,
                                   SkeletonTracker.Joints.HighestPoint + SkeletonTracker.Joints.LowestPoint);

            // Update Torso
            Torso = new Rectangle((int)SkeletonTracker.Joints.LeftShoulder.X, Head.Y + Head.Height,
                                   (int)Vector2.Distance(SkeletonTracker.Joints.LeftShoulder, SkeletonTracker.Joints.RightShoulder),
                                   (int)Vector2.Distance(new Vector2(Head.X, Head.Y + Head.Height), SkeletonTracker.Joints.LeftHip));

            _torsoPrevPosition = _torsoCurrentPosition;
            _torsoVelocity = _torsoCurrentPosition - _torsoPrevPosition;

            // Update Limbs
            LeftForeArm.Update(SkeletonTracker.Joints.LeftElbow, SkeletonTracker.Joints.LeftHand);
            LeftUpperArm.Update(SkeletonTracker.Joints.LeftElbow, SkeletonTracker.Joints.LeftShoulder);
            RightForeArm.Update(SkeletonTracker.Joints.RightHand, SkeletonTracker.Joints.RightElbow);
            RightUpperArm.Update(SkeletonTracker.Joints.RightShoulder, SkeletonTracker.Joints.RightElbow);
            LeftThigh.Update(SkeletonTracker.Joints.LeftHip, SkeletonTracker.Joints.LeftKnee);
            RightThigh.Update(SkeletonTracker.Joints.RightHip, SkeletonTracker.Joints.RightKnee);
            LeftShin.Update(SkeletonTracker.Joints.LeftKnee, SkeletonTracker.Joints.LeftFoot);
            RightShin.Update(SkeletonTracker.Joints.RightKnee, SkeletonTracker.Joints.RightFoot);
        }

        public void Draw()
        {
            GameUtils.GetUtil<SpriteBatch>().Draw(HeadTexture, Head, Color.White);
            GameUtils.GetUtil<SpriteBatch>().Draw(MainTexture, Torso, Color.White);
            LeftForeArm.Draw();
            LeftUpperArm.Draw();
            RightForeArm.Draw();
            RightUpperArm.Draw();
            LeftThigh.Draw();
            RightThigh.Draw();
            LeftShin.Draw();
            RightShin.Draw();
        }

        /// <summary>
        /// Returns a LimbCollisionData object, showing the name of the limb collided with and it's velocity.
        /// </summary>
        public LimbCollisionData GetCollisionData(Rectangle rect)
        {
            if (Head.Intersects(rect))
                return new LimbCollisionData(true, _headVelocity, BodyPart.Head);
            else if (Body.Intersects(rect))
            {
                if (LeftForeArm.Collides(rect))
                    return new LimbCollisionData(true, LeftForeArm.Velocity, BodyPart.Head);
                else if (LeftUpperArm.Collides(rect))
                    return new LimbCollisionData(true, LeftUpperArm.Velocity, BodyPart.LeftUpperArm);
                else if (RightForeArm.Collides(rect))
                    return new LimbCollisionData(true, RightForeArm.Velocity, BodyPart.RightForeArm);
                else if (RightUpperArm.Collides(rect))
                    return new LimbCollisionData(true, RightUpperArm.Velocity, BodyPart.RightUpperArm);
                else if (LeftThigh.Collides(rect))
                    return new LimbCollisionData(true, LeftThigh.Velocity, BodyPart.LeftThigh);
                else if (RightThigh.Collides(rect))
                    return new LimbCollisionData(true, RightThigh.Velocity, BodyPart.RightThigh);
                else if (LeftShin.Collides(rect))
                    return new LimbCollisionData(true, LeftShin.Velocity, BodyPart.LeftShin);
                else if (RightShin.Collides(rect))
                    return new LimbCollisionData(true, RightShin.Velocity, BodyPart.RightShin);
                else
                    return new LimbCollisionData(false, Vector2.Zero);
            }
            else
                return new LimbCollisionData(false, Vector2.Zero);
        }

        /// <summary>
        /// Returns a LimbCollisionData object, showing the name of the limb collided with and it's velocity.
        /// </summary>
        public LimbCollisionData GetCollisionData(SATRectangle rect)
        {
            if (rect.Intersects(Head))
                return new LimbCollisionData(true, _headVelocity, BodyPart.Head);
            else if (rect.Intersects(Body))
            {
                if (LeftForeArm.Collides(rect))
                    return new LimbCollisionData(true, LeftForeArm.Velocity, BodyPart.Head);
                else if (LeftUpperArm.Collides(rect))
                    return new LimbCollisionData(true, LeftUpperArm.Velocity, BodyPart.LeftUpperArm);
                else if (RightForeArm.Collides(rect))
                    return new LimbCollisionData(true, RightForeArm.Velocity, BodyPart.RightForeArm);
                else if (RightUpperArm.Collides(rect))
                    return new LimbCollisionData(true, RightUpperArm.Velocity, BodyPart.RightUpperArm);
                else if (LeftThigh.Collides(rect))
                    return new LimbCollisionData(true, LeftThigh.Velocity, BodyPart.LeftThigh);
                else if (RightThigh.Collides(rect))
                    return new LimbCollisionData(true, RightThigh.Velocity, BodyPart.RightThigh);
                else if (LeftShin.Collides(rect))
                    return new LimbCollisionData(true, LeftShin.Velocity, BodyPart.LeftShin);
                else if (RightShin.Collides(rect))
                    return new LimbCollisionData(true, RightShin.Velocity, BodyPart.RightShin);
                else
                    return new LimbCollisionData(false, Vector2.Zero);
            }
            else
                return new LimbCollisionData(false, Vector2.Zero);
        }
    }
}
