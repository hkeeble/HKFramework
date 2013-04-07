using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using HKFramework;
using HKFramework.TwoD;

namespace HKFramework
{
    /// <summary>
    /// Represents an entire HUD, made up of HUD.Component objects.
    /// </summary>
    public class HUD
    {
        /// <summary>
        /// Represents an individual component.
        /// </summary>
        public class Component
        {
            /// <summary>
            /// Represents a TextComponent.
            /// </summary>
            protected class TextComponent
            {
                #region Declarations
                public string Text;
                private Vector2 _relativePosition;
                private SpriteFont _font;
                private Color _color;
                private float _scale;
                private Component _parent;
                #endregion

                /// <summary>
                /// Instantiates a new TextComponent.
                /// </summary>
                public TextComponent()
                {
                    Text = "";
                }

                /// <summary>
                /// Instantiates a new TextComponent.
                /// </summary>
                /// <param name="relativePosition">Position relative to graphic parent.</param>
                /// <param name="text">Text to display.</param>
                /// <param name="color">Color of text.</param>
                /// <param name="font">SpriteFont to use.</param>
                /// <param name="parent">Parent of component.</param>
                /// <param name="scale">Scale of text.</param>
                public TextComponent(Vector2 relativePosition, string text, Color color, SpriteFont font, Component parent, float scale = 1f)
                {
                    Text = text;
                    _color = color;
                    _font = font;
                    _scale = scale;
                    _relativePosition = relativePosition;
                    _parent = parent;
                }

                /// <summary>
                /// Draws the text component.
                /// </summary>
                public void Draw()
                {
                    GameUtils.GetUtil<SpriteBatch>().DrawString(_font, Text, _parent.Position + _relativePosition, _color, 0f,
                        new Vector2(_font.MeasureString(Text).X / 2, _font.MeasureString(Text).Y / 2),
                        _scale, SpriteEffects.None, 1f);
                }

                /// <summary>
                /// The font being used by the TextComponent.
                /// </summary>
                public SpriteFont Font { get { return _font; } }

                public float Width { get { return _font.MeasureString(Text).X; } }
                public float Height { get { return _font.MeasureString(Text).Y; } }
            }

            #region Declarations
            protected Graphic _graphic;
            protected TextComponent _text;
            #endregion

            /// <summary>
            /// Instantiates a new component.
            /// </summary>
            /// <param name="position">The position of the component.</param>
            /// <param name="texture">The texture for the component to use.</param>
            /// <param name="applyAlpha">Whether or not to apply the global HUD alpha to this component</param>
            public Component(Vector2 position, bool applyAlpha, Texture2D texture = null)
            {
                if (texture != null)
                {
                    if (applyAlpha)
                    {
                        Color[] data = new Color[texture.Width * texture.Height];
                        texture.GetData<Color>(data);
                        for (int i = 0; i < data.Length; i++)
                            data[i].A = HUD_ALPHA;
                        texture.SetData<Color>(data);
                    }

                    _graphic = new Graphic(texture, position);
                    _text = new TextComponent();
                }
            }

            /// <summary>
            /// Draws the component.
            /// </summary>
            public virtual void Draw()
            {
                if (_graphic != null)
                    _graphic.Draw();
                if (_text.Text != null)
                    _text.Draw();
            }

            /// <summary>
            /// Updates a component. (Override for additional behaviour)
            /// </summary>
            public virtual void Update(GameTime gameTime, GraphicsDevice device)
            {

            }

            /// <summary>
            /// Sets the text to display in the component.
            /// </summary>
            /// <param name="position">Position relative to component.</param>
            /// <param name="text">Text to display.</param>
            /// <param name="font">SpriteFont to use.</param>
            /// <param name="textColor">Color of text.</param>
            /// <param name="scale">Scale of text.</param>
            protected void SetText(Vector2 position, string text, SpriteFont font, Color textColor, float scale = 1f)
            {
                _text = new TextComponent(position, text, textColor, font, this, scale);
            }

            /// <summary>
            /// The position of the component.
            /// </summary>
            public Vector2 Position { get { return _graphic.Position; } set { _graphic.Position = value; } }
        }

        #region Declarations
        public const int HUD_ALPHA = 160;
        List<Component> components;
        #endregion

        /// <summary>
        /// Initializes a new HUD.
        /// </summary>
        public HUD()
        {
            components = new List<Component>();
        }

        /// <summary>
        /// Adds a new component to the HUD.
        /// </summary>
        /// <param name="component">The component to be added to the HUD.</param>
        public void AddComponent(Component component)
        {
            components.Add(component);
        }

        /// <summary>
        /// Removes the specified component from the HUD.
        /// </summary>
        /// <param name="component">The component to remove.</param>
        public void RemoveComponent(Component component)
        {
            components.Remove(component);
        }

        /// <summary>
        /// Removes the last component added to the HUD.
        /// </summary>
        public void RemoveLastComponent()
        {
            components.Remove(components[components.Count-1]);
        }

        /// <summary>
        /// Draws all HUD components.
        /// </summary>
        public void Draw()
        {
            GameUtils.GetUtil<SpriteBatch>().Begin();
            foreach (Component component in components)
                component.Draw();
            GameUtils.GetUtil<SpriteBatch>().End();
        }

        /// <summary>
        /// Updates all HUD components.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            foreach (Component component in components)
                component.Update(gameTime, GameUtils.GetUtil<GraphicsDevice>());
        }
    }
}
