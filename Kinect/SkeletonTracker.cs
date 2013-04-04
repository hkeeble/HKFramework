using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Kinect;
using HKFramework;

namespace HKFramework.Kinect
{
    public class SkeletonTracker : Microsoft.Xna.Framework.GameComponent
    {
        /// <summary>
        /// Represents a player's skeleton
        /// </summary>
        public struct JointPositions
        {
            public Vector2 RightHand;
            public Vector2 LeftHand;
            public Vector2 Head;
            public Vector2 LeftFoot;
            public Vector2 RightFoot;
            public Vector2 LeftShoulder;
            public Vector2 RightShoulder;
            public Vector2 RightElbow;
            public Vector2 LeftElbow;
            public Vector2 LeftHip;
            public Vector2 RightHip;
            public Vector2 LeftKnee;
            public Vector2 RightKnee;

            /// <summary>
            /// Returns the highest point in the skeleton.
            /// </summary>
            public int HighestPoint
            {
                get
                {
                    float highPoint = Head.Y;
                    if (LeftHand.Y < highPoint)
                        highPoint = LeftHand.Y;
                    if (RightHand.Y < highPoint)
                        highPoint = RightHand.Y;
                    if (LeftShoulder.Y < highPoint)
                        highPoint = LeftShoulder.Y;
                    if (RightShoulder.Y < highPoint)
                        highPoint = LeftShoulder.Y;
                    if (LeftElbow.Y < highPoint)
                        highPoint = LeftElbow.Y;
                    if (RightElbow.Y < highPoint)
                        highPoint = RightElbow.Y;
                    if (LeftHip.Y < highPoint)
                        highPoint = LeftHip.Y;
                    if (RightHip.Y < highPoint)
                        highPoint = RightHip.Y;
                    if (RightKnee.Y < highPoint)
                        highPoint = RightKnee.Y;
                    if (LeftKnee.Y < highPoint)
                        highPoint = LeftKnee.Y;
                    return (int)highPoint;
                }
            }

            /// <summary>
            /// Returns the lowest point in the skeleton.
            /// </summary>
            public int LowestPoint
            {
                get
                {
                    float lowPoint = Head.Y;
                    if (LeftHand.Y > lowPoint)
                        lowPoint = LeftHand.Y;
                    if (RightHand.Y > lowPoint)
                        lowPoint = RightHand.Y;
                    if (LeftShoulder.Y > lowPoint)
                        lowPoint = LeftShoulder.Y;
                    if (RightShoulder.Y > lowPoint)
                        lowPoint = LeftShoulder.Y;
                    if (LeftElbow.Y > lowPoint)
                        lowPoint = LeftElbow.Y;
                    if (RightElbow.Y > lowPoint)
                        lowPoint = RightElbow.Y;
                    if (LeftHip.Y > lowPoint)
                        lowPoint = LeftHip.Y;
                    if (RightHip.Y > lowPoint)
                        lowPoint = RightHip.Y;
                    if (RightKnee.Y > lowPoint)
                        lowPoint = RightKnee.Y;
                    if (LeftKnee.Y > lowPoint)
                        lowPoint = LeftKnee.Y;
                    if (lowPoint > Config.Height)
                        lowPoint = Config.Height;
                    return (int)lowPoint;
                }
            }

            /// <summary>
            /// Returns the left-most point in the skeleton.
            /// </summary>
            public int LeftMostPoint
            {
                get
                {
                    float leftPoint = Head.X;
                    if (LeftHand.X < leftPoint)
                        leftPoint = LeftHand.X;
                    if (RightHand.X < leftPoint)
                        leftPoint = RightHand.X;
                    if (LeftShoulder.X < leftPoint)
                        leftPoint = LeftShoulder.X;
                    if (RightShoulder.X < leftPoint)
                        leftPoint = LeftShoulder.X;
                    if (LeftElbow.X < leftPoint)
                        leftPoint = LeftElbow.X;
                    if (RightElbow.X < leftPoint)
                        leftPoint = RightElbow.X;
                    if (LeftHip.X < leftPoint)
                        leftPoint = LeftHip.X;
                    if (RightHip.X < leftPoint)
                        leftPoint = RightHip.X;
                    if (RightKnee.X < leftPoint)
                        leftPoint = RightKnee.X;
                    if (LeftKnee.X < leftPoint)
                        leftPoint = LeftKnee.X;
                    if (leftPoint < 0)
                        leftPoint = 0;
                    return (int)leftPoint;
                }
            }

            /// <summary>
            /// Returns the right-most point in the skeleton.
            /// </summary>
            public int RightMostPoint
            {
                get
                {
                    float rightPoint = Head.X;
                    if (LeftHand.X > rightPoint)
                        rightPoint = LeftHand.X;
                    if (RightHand.X > rightPoint)
                        rightPoint = RightHand.X;
                    if (LeftShoulder.X > rightPoint)
                        rightPoint = LeftShoulder.X;
                    if (RightShoulder.X > rightPoint)
                        rightPoint = LeftShoulder.X;
                    if (LeftElbow.X > rightPoint)
                        rightPoint = LeftElbow.X;
                    if (RightElbow.X > rightPoint)
                        rightPoint = RightElbow.X;
                    if (LeftHip.X > rightPoint)
                        rightPoint = LeftHip.X;
                    if (RightHip.X > rightPoint)
                        rightPoint = RightHip.X;
                    if (RightKnee.X > rightPoint)
                        rightPoint = RightKnee.X;
                    if (LeftKnee.X > rightPoint)
                        rightPoint = LeftKnee.X;
                    if (rightPoint > Config.Width)
                        rightPoint = Config.Width;
                    return (int)rightPoint;
                }
            }
        }

        private static KinectSensor kinectSensor;
        private static string connectedStatus;
        private static bool playerOnScreen = false;

        private static JointPositions jPositions;
        private static Texture2D jointTracker;

        public SkeletonTracker(Game game)
            : base(game)
        {
            KinectSensor.KinectSensors.StatusChanged += new EventHandler<StatusChangedEventArgs>(KinectSensors_StatusChanged);
            DiscoverKinectSensor();
            jointTracker = Game.Content.Load<Texture2D>("2DAssets\\kinectJointTracker");
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        private void DiscoverKinectSensor()
        {
            foreach (KinectSensor sensor in KinectSensor.KinectSensors)
            {
                if (sensor.Status == KinectStatus.Connected)
                    kinectSensor = sensor;
            }

            if (kinectSensor == null)
            {
                connectedStatus = "No Kinect's found.";
                return;
            }

            switch (kinectSensor.Status)
            {
                case KinectStatus.Connected:
                    InitializeKinect();
                    connectedStatus = "Kinect Connected.";
                    break;
                case KinectStatus.Disconnected:
                    connectedStatus = "Kinect Disconnected.";
                    break;
                case KinectStatus.NotPowered:
                    connectedStatus = "Kinect not powered.";
                    break;
                case KinectStatus.Error:
                    connectedStatus = "Error detected.";
                    break;
                default:
                    connectedStatus = "Unknown error.";
                    break;
            }
        }

        private bool InitializeKinect()
        {
            kinectSensor.SkeletonStream.Enable(new TransformSmoothParameters()
            {
                Smoothing = 0.9f,
                Correction = 0.9f,
                Prediction = 0.9f,
                JitterRadius = 0.05f,
                MaxDeviationRadius = 0.04f
            });
            kinectSensor.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(kinectSensor_SkeletonFrameReady);

            try { kinectSensor.Start(); }
            catch
            {
                connectedStatus = "Could not start Kinect sensor.";
                return false;
            }
            return true;
        }

        void KinectSensors_StatusChanged(object sender, StatusChangedEventArgs args)
        {
            if (kinectSensor == args.Sensor)
            {
                if (args.Status == KinectStatus.Disconnected || args.Status == KinectStatus.NotPowered)
                {
                    kinectSensor = null;
                    DiscoverKinectSensor();
                }
            }
        }

        void kinectSensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs args)
        {
            using (SkeletonFrame skeletonFrame = args.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    Skeleton[] skeletonData = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(skeletonData);
                    Skeleton playerSkeleton = (from s in skeletonData where s.TrackingState == SkeletonTrackingState.Tracked select s).FirstOrDefault();
                    if (playerSkeleton != null)
                    {
                        playerOnScreen = true;

                        Joint rightHand = playerSkeleton.Joints[JointType.HandRight];
                        Joint leftHand = playerSkeleton.Joints[JointType.HandLeft];
                        Joint head = playerSkeleton.Joints[JointType.Head];
                        Joint leftShoulder = playerSkeleton.Joints[JointType.ShoulderLeft];
                        Joint rightShoulder = playerSkeleton.Joints[JointType.ShoulderRight];
                        Joint leftFoot = playerSkeleton.Joints[JointType.FootLeft];
                        Joint rightFoot = playerSkeleton.Joints[JointType.FootRight];
                        Joint leftHip = playerSkeleton.Joints[JointType.HipLeft];
                        Joint rightHip = playerSkeleton.Joints[JointType.HipRight];
                        Joint leftElbow = playerSkeleton.Joints[JointType.ElbowLeft];
                        Joint rightElbow = playerSkeleton.Joints[JointType.ElbowRight];
                        Joint leftKnee = playerSkeleton.Joints[JointType.KneeLeft];
                        Joint rightKnee = playerSkeleton.Joints[JointType.KneeRight];

                        jPositions.RightHand = Get2DJointPosition(new Vector2(rightHand.Position.X, rightHand.Position.Y));
                        jPositions.LeftHand = Get2DJointPosition(new Vector2(leftHand.Position.X, leftHand.Position.Y));
                        jPositions.Head = Get2DJointPosition(new Vector2(head.Position.X, head.Position.Y));
                        jPositions.LeftShoulder = Get2DJointPosition(new Vector2(leftShoulder.Position.X, leftShoulder.Position.Y));
                        jPositions.RightShoulder = Get2DJointPosition(new Vector2(rightShoulder.Position.X, rightShoulder.Position.Y));
                        jPositions.LeftFoot = Get2DJointPosition(new Vector2(leftFoot.Position.X, leftFoot.Position.Y));
                        jPositions.RightFoot = Get2DJointPosition(new Vector2(rightFoot.Position.X, rightFoot.Position.Y));
                        jPositions.RightHip = Get2DJointPosition(new Vector2(rightHip.Position.X, rightHip.Position.Y));
                        jPositions.LeftHip = Get2DJointPosition(new Vector2(leftHip.Position.X, leftHip.Position.Y));
                        jPositions.LeftElbow = Get2DJointPosition(new Vector2(leftElbow.Position.X, leftElbow.Position.Y));
                        jPositions.RightElbow = Get2DJointPosition(new Vector2(rightElbow.Position.X, rightElbow.Position.Y));
                        jPositions.LeftKnee = Get2DJointPosition(new Vector2(leftKnee.Position.X, leftKnee.Position.Y));
                        jPositions.RightKnee = Get2DJointPosition(new Vector2(rightKnee.Position.X, rightKnee.Position.Y));
                    }
                    else
                    {
                        playerOnScreen = false;
                    }
                }
            }
        }

        /// <summary>
        /// Takes a Vector2 position and converts it into a 2D screen co-ordinate that can be anywhere on the screen.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private Vector2 Get2DJointPosition(Vector2 position)
        {
            return new Vector2(((0.5f * position.X) + 0.5f) * Config.Width, ((-0.5f * position.Y) + 0.5f) * Config.Height);
        }

        public static void DrawSkeleton()
        {
            Vector2 trackerCenter = new Vector2(jointTracker.Width / 2, jointTracker.Height / 2);
            GameUtils.GetUtil<SpriteBatch>().Draw(jointTracker, jPositions.RightHand, null, Color.White, 0f, trackerCenter, 1f, SpriteEffects.None, 0f);
            GameUtils.GetUtil<SpriteBatch>().Draw(jointTracker, jPositions.LeftHand, null, Color.White, 0f, trackerCenter, 1f, SpriteEffects.None, 0f);
            GameUtils.GetUtil<SpriteBatch>().Draw(jointTracker, jPositions.RightHip, null, Color.White, 0f, trackerCenter, 1f, SpriteEffects.None, 0f);
            GameUtils.GetUtil<SpriteBatch>().Draw(jointTracker, jPositions.LeftHip, null, Color.White, 0f, trackerCenter, 1f, SpriteEffects.None, 0f);
            GameUtils.GetUtil<SpriteBatch>().Draw(jointTracker, jPositions.RightFoot, null, Color.White, 0f, trackerCenter, 1f, SpriteEffects.None, 0f);
            GameUtils.GetUtil<SpriteBatch>().Draw(jointTracker, jPositions.LeftFoot, null, Color.White, 0f, trackerCenter, 1f, SpriteEffects.None, 0f);
            GameUtils.GetUtil<SpriteBatch>().Draw(jointTracker, jPositions.RightShoulder, null, Color.White, 0f, trackerCenter, 1f, SpriteEffects.None, 0f);
            GameUtils.GetUtil<SpriteBatch>().Draw(jointTracker, jPositions.LeftShoulder, null, Color.White, 0f, trackerCenter, 1f, SpriteEffects.None, 0f);
            GameUtils.GetUtil<SpriteBatch>().Draw(jointTracker, jPositions.Head, null, Color.White, 0f, trackerCenter, 1f, SpriteEffects.None, 0f);
            GameUtils.GetUtil<SpriteBatch>().Draw(jointTracker, jPositions.LeftElbow, null, Color.White, 0f, trackerCenter, 1f, SpriteEffects.None, 0f);
            GameUtils.GetUtil<SpriteBatch>().Draw(jointTracker, jPositions.RightElbow, null, Color.White, 0f, trackerCenter, 1f, SpriteEffects.None, 0f);
            GameUtils.GetUtil<SpriteBatch>().Draw(jointTracker, jPositions.LeftKnee, null, Color.White, 0f, trackerCenter, 1f, SpriteEffects.None, 0f);
            GameUtils.GetUtil<SpriteBatch>().Draw(jointTracker, jPositions.RightKnee, null, Color.White, 0f, trackerCenter, 1f, SpriteEffects.None, 0f);
        }

        public void Unload()
        {
            kinectSensor.Stop();
            kinectSensor.Dispose();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public static JointPositions Joints { get { return jPositions; } }
        public static bool PlayerOnScreen { get { return playerOnScreen; } }
    }
}
