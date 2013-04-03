using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using HKFramework;
using HKFramework.TwoD.Entity;

namespace HKFramework.HUD
{
    public class HUD
    {
        public class Component
        {
            protected class TextComponent
            {
                public string Text;
                private Vector2 _position;
                private SpriteFont _font;
                private Color _color;
                private float _scale;
                private Component _parent;

                public TextComponent()
                {
                    Text = "";
                }

                public TextComponent(Vector2 position, string text, Color color, SpriteFont font, Component parent, float scale = 1f)
                {
                    Text = text;
                    _color = color;
                    _font = font;
                    _scale = scale;
                    _position = position;
                    _parent = parent;
                }

                public void Draw()
                {
                    GameUtils.GetUtil<SpriteBatch>().DrawString(_font, Text, _parent.Position + _position, _color, 0f, new Vector2(_font.MeasureString(Text).X / 2, _font.MeasureString(Text).Y / 2),
                        _scale, SpriteEffects.None, 1f);
                }

                public SpriteFont Font { get { return _font; } }

                public float Width { get { return _font.MeasureString(Text).X; } }
                public float Height { get { return _font.MeasureString(Text).Y; } }
            }

            protected Vector2 _position;
            protected SpriteEntity _graphic;
            protected TextComponent _text;

            public Component(Vector2 position, Texture2D texture = null)
            {
                Color[] data = new Color[texture.Width * texture.Height];
                texture.GetData<Color>(data);
                for (int i = 0; i < data.Length; i++)
                    data[i].A = 180;
                texture.SetData<Color>(data);

                _position = position;
                if (texture != null)
                    _graphic = new SpriteEntity(texture, position);
                _text = new TextComponent();
            }

            public virtual void Draw()
            {
                if (_graphic != null)
                    _graphic.Draw();
                if (_text.Text != null)
                    _text.Draw();
            }

            public virtual void Update(GameTime gameTime, GraphicsDevice device)
            {

            }

            protected void SetText(Vector2 position, string text, SpriteFont font, Color textColor, float scale = 1f)
            {
                _text = new TextComponent(position, text, textColor, font, this, scale);
            }

            public Vector2 Position { get { return _position; } }
        }

        public const int HUD_ALPHA = 160;

        List<Component> components;

        public HUD()
        {
            components = new List<Component>();

            components.Add(new Score());

            Texture2D healthBar = GameUtils.GetUtil<ContentManager>().Load<Texture2D>("2DAssets/GUI/HealthBar");
            components.Add(new HealthBar(new Vector2(Config.Width - healthBar.Width, Config.Height - healthBar.Height), GameUtils.GetUtil<GraphicsDevice>()));
        }

        public void Draw()
        {
            GameUtils.GetUtil<SpriteBatch>().Begin();
            foreach (Component component in components)
                component.Draw();
            GameUtils.GetUtil<SpriteBatch>().End();
        }

        public void Update(GameTime gameTime)
        {
            foreach (Component component in components)
                component.Update(gameTime, GameUtils.GetUtil<GraphicsDevice>());
        }
    }
}
