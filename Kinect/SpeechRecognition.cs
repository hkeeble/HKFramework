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
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
using System.IO;


namespace HKFramework.Kinect
{
    public class SpeechRecognition : Microsoft.Xna.Framework.GameComponent
    {
        private static KinectSensor kinectSensor;
        private static SpeechRecognitionEngine speechRecognizer;

        private static RecognizerInfo GetKinectRecognizer()
        {
            Func<RecognizerInfo, bool> machtingFunc = r =>
                {
                    string value;
                    r.AdditionalInfo.TryGetValue("Kinect", out value);
                    return "True".Equals(value, StringComparison.InvariantCultureIgnoreCase) && "en-US".Equals(r.Culture.Name, StringComparison.InvariantCultureIgnoreCase);
                };
            return SpeechRecognitionEngine.InstalledRecognizers().Where(machtingFunc).FirstOrDefault();
        }

        public SpeechRecognition(Game game)
            : base(game)
        {
            kinectSensor = KinectSensor.KinectSensors.FirstOrDefault(s => s.Status == KinectStatus.Connected);
            speechRecognizer = CreateSpeechRecognizer();
            kinectSensor.Start();
            base.Initialize();
        }

        public override void Initialize()
        {

        }

        public static void StartAudioStream()
        {
            var audioSource = kinectSensor.AudioSource;
            audioSource.BeamAngleMode = BeamAngleMode.Adaptive;
            var kinectStream = audioSource.Start();
            speechRecognizer.SetInputToAudioStream(kinectStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
            speechRecognizer.RecognizeAsync(RecognizeMode.Multiple);
            kinectSensor.AudioSource.EchoCancellationMode = EchoCancellationMode.None;
            kinectSensor.AudioSource.AutomaticGainControlEnabled = false;
        }

        private SpeechRecognitionEngine CreateSpeechRecognizer()
        {
            RecognizerInfo ri = GetKinectRecognizer();
            SpeechRecognitionEngine sre;
            sre = new SpeechRecognitionEngine(ri.Id);

            var grammar = new Choices();
            grammar.Add("Start");
            grammar.Add("Exit");

            var gb = new GrammarBuilder { Culture = ri.Culture };
            gb.Append(grammar);

            var g = new Grammar(gb);
            sre.LoadGrammar(g);

            sre.SpeechRecognized += SreSpeechRecognized;
            sre.SpeechRecognitionRejected += SreSpeechRecognitionRejected;
            return sre;
        }

        private void RejectSpeech(RecognitionResult result)
        {
            Console.WriteLine("lolwut?");
        }

        private void SreSpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs args)
        {
            RejectSpeech(args.Result);
        }

        private void SreSpeechRecognized(object sender, SpeechRecognizedEventArgs args)
        {
            if (args.Result.Confidence < .4)
                RejectSpeech(args.Result);

            switch (args.Result.Text.ToUpperInvariant())
            {
                case "START":
                    KinectGame.Main.SetState(typeof(KinectGame.Components.GameHandler));
                    break;
                default:
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
