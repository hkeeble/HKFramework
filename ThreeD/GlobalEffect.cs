using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HKFramework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HKFramework.ThreeD
{
    /// <summary>
    /// Used to represent a global effect. Cannot be instantiated.
    /// </summary>
    public static class GlobalEffect
    {
        private static Effect _effect;

        /// <summary>
        /// Returns the currently loaded effect, readonly.
        /// </summary>
        public static Effect Effect { get { return _effect; } }

        /// <summary>
        /// Loads an effect file
        /// </summary>
        /// <param name="filepath">The filepath of the effect (relative to default XNA content pipeline)</param>
        public static void LoadEffect(string filepath)
        {
            _effect = GameUtils.GetUtil<ContentManager>().Load<Effect>(filepath);
        }

        /// <summary>
        /// Sets current shader technique.
        /// </summary>
        /// <param name="techniqueName">Name of technique.</param>
        public static void SetTechnique(string techniqueName)
        {
            _effect.CurrentTechnique = _effect.Techniques[techniqueName];
        }

        /// <summary>
        /// Sets the WorldViewProjection Matrix of the effect. Will also set the inverse world matrix.
        /// </summary>
        /// <param name="world"></param>
        public static void SetWVPMatrix(Matrix world)
        {
            _effect.Parameters["worldViewProjMatrix"].SetValue(world);
            _effect.Parameters["inverseWorldMatrix"].SetValue(Matrix.Invert(world));
        }

        /// <summary>
        /// Sets the ambient light to use for the entire scene.
        /// </summary>
        /// <param name="color">The color in RGBA format.</param>
        public static void SetAmbientLight(Vector4 color)
        {
            _effect.Parameters["Light_Ambient"].SetValue(color);
        }

        public static void SetDominantDirectionalLight(Vector4 position, Vector4 color)
        {
            _effect.Parameters["Dominant_Light_Position"].SetValue(position);
            _effect.Parameters["Dominant_Light_Color"].SetValue(color);
        }

        public static void SetEyePosition(Vector4 position)
        {
            _effect.Parameters["Eye_Position"].SetValue(position);
        }

        public static EffectPassCollection Passes { get { return _effect.CurrentTechnique.Passes; } }
    }
}
