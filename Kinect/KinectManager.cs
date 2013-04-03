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


namespace KinectGame.Components
{
    public class KinectManager : Microsoft.Xna.Framework.GameComponent
    {
        private static Texture2D RGBvideo;
        private static Texture2D depthData;
        private static KinectSensor kinectSensor;
        private static string connectedStatus;

        public KinectManager(Game game)
            : base(game)
        {
            KinectSensor.KinectSensors.StatusChanged += new EventHandler<StatusChangedEventArgs>(KinectSensors_StatusChanged);
            DiscoverKinectSensor();
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
            kinectSensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
            kinectSensor.DepthFrameReady += new EventHandler<DepthImageFrameReadyEventArgs>(kinectSensor_DepthImageFrameReady);

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

        void kinectSensor_DepthImageFrameReady(object sender, DepthImageFrameReadyEventArgs args)
        {
            using (DepthImageFrame depthImageFrame = args.OpenDepthImageFrame())
            {
                if (depthImageFrame != null)
                {
                    short[] pixelsFromFrame = new short[depthImageFrame.PixelDataLength];

                    depthImageFrame.CopyPixelDataTo(pixelsFromFrame);
                    byte[] convertedPixels = ConvertDepthFrame(pixelsFromFrame, ((KinectSensor)sender).DepthStream, 640 * 480 * 4);

                    depthData = new Texture2D(GameUtils.GetUtil<GraphicsDevice>(), depthImageFrame.Width, depthImageFrame.Height);

                    depthData.SetData<byte>(convertedPixels);
                }
            }
        }

        private byte[] ConvertDepthFrame(short[] depthFrame, DepthImageStream depthStream, int depthFrame32Length)
        {
            int RedIndex = 2;
            int GreenIndex = 1;
            int BlueIndex = 0;

            int[] IntensityShiftByPlayerR = { 7, 7, 1, 1, 1, 1, 1, 1 };
            int[] IntensityShiftByPlayerG = { 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] IntensityShiftByPlayerB = { 7, 7, 1, 1, 1, 1, 1, 1 };

            byte[] depthFrame32 = new byte[depthFrame32Length];

            for (int i16 = 0, i32 = 0; i16 < depthFrame.Length && i32 < depthFrame32Length; i16++, i32 += 4)
            {
                int player = depthFrame[i16] & DepthImageFrame.PlayerIndexBitmask;
                int realDepth = depthFrame[i16] >> DepthImageFrame.PlayerIndexBitmaskWidth;

                byte intensity = (byte)(~(realDepth >> 4));

                if (player == 0 && realDepth == 0)
                {
                    // White
                    depthFrame32[i32 + RedIndex] = 255;
                    depthFrame32[i32 + GreenIndex] = 255;
                    depthFrame32[i32 + BlueIndex] = 255;
                }
                if (player == 0 && realDepth == depthStream.TooFarDepth)
                {
                    // Dark Purple
                    depthFrame32[i32 + RedIndex] = 66;
                    depthFrame32[i32 + GreenIndex] = 0;
                    depthFrame32[i32 + BlueIndex] = 66;
                }
                if (player == 0 && realDepth == depthStream.UnknownDepth)
                {
                    // Dark Brown
                    depthFrame32[i32 + RedIndex] = 66;
                    depthFrame32[i32 + GreenIndex] = 66;
                    depthFrame32[i32 + BlueIndex] = 33;
                }
                else
                {
                    depthFrame32[i32 + RedIndex] = (byte)(intensity >> IntensityShiftByPlayerR[player]);
                    depthFrame32[i32 + GreenIndex] = (byte)(intensity >> IntensityShiftByPlayerG[player]);
                    depthFrame32[i32 + BlueIndex] = (byte)(intensity >> IntensityShiftByPlayerB[player]);
                }
            }
            return depthFrame32;
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

        public static Texture2D VideoData { get { return RGBvideo; } }
        public static Texture2D DepthData { get { return depthData; } }
    }
}
