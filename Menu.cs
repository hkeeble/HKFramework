using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HKFramework.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HKFramework
{
    public class Menu
    {
        /// <summary>
        /// An individual option on the menu.
        /// </summary>
        private class Option : TextObject
        {
            #region Declarations
            private Menu _parent;
            private int _index;
            private bool _selectedOption;
            private int _spacing;
            #endregion

            /// <summary>
            /// Constructs a new menu option.
            /// </summary>
            /// <param name="index">The index of the option in the list.</param>
            /// <param name="text">The text of the option.</param>
            /// <param name="action">The action the option performs.</param>
            public Option(Menu parent, int index, SpriteFont font, int spacing, string text, Color color)
                : base(new Vector2(0, font.MeasureString(text).Y+spacing), text, font, color)
            {
                _selectedOption = false;
                _index = index;
                _parent = parent;
                _spacing = spacing;
            }

            /// <summary>
            /// Draws an option at a position relative to it's parent menu.
            /// </summary>
            public override void Draw()
            {
                if(_selectedOption == false)
                    GameUtils.GetUtil<SpriteBatch>().DrawString(_font, Text, (Position - (_font.MeasureString(Text) / 2)) + _parent.Position + new Vector2(0, _spacing), _drawColor);
                else
                    GameUtils.GetUtil<SpriteBatch>().DrawString(_font, Text, (Position - (_font.MeasureString(Text) / 2)) + _parent.Position + new Vector2(0, _spacing), _drawColor, 0f, Vector2.Zero, 1.3f,
                        SpriteEffects.None, 1f);
            }

            /// <summary>
            /// Updates the draw color of an option if it is the selected index.
            /// </summary>
            /// <param name="selectedIndex">The index of the currently selected item.</param>
            public void Update(int selectedIndex)
            {
                if (_index == selectedIndex)
                {
                    _drawColor = _parent._selectedColor;
                    _selectedOption = true;
                }
                else
                {
                    _selectedOption = false;
                    _drawColor = _parent._mainColor;
                }
                base.Update();
            }
        }
        
        #region Declarations
        public Vector2 Position;
        protected Color _selectedColor, _mainColor;
        private List<Option> _options;
        private int _selectedIndex;
        #endregion

        /// <summary>
        /// Constructs a new menu.
        /// </summary>
        /// <param name="options">An array of the options to include in the menu.</param>
        /// <param name="font">The font to use to draw the menu.</param>
        /// <param name="position">The screen coordinates of the menu.</param>
        /// <param name="mainColor">The color to use to draw un-selected items in the menu.</param>
        /// <param name="selectedItemColor">The color to use to draw the currently selected item in the menu.</param>
        /// <param name="itemSpacing">The Y coordinate spacing between each menu element.</param>
        public Menu(string[] options, SpriteFont font, Vector2 position, Color mainColor, Color selectedItemColor, int itemSpacing)
        {
            _selectedIndex = 0;
            _selectedColor = selectedItemColor;
            _mainColor = mainColor;
            Position = position;

            _options = new List<Option>();

            for (int i = 0; i < options.Length; i++)
                _options.Add(new Option(this, i, font, itemSpacing*i, options[i], mainColor));
        }

        /// <summary>
        /// Draws all the menu's options to the screen.
        /// </summary>
        public void Draw()
        {
            foreach (Option o in _options)
                o.Draw();
        }

        /// <summary>
        /// Updates all menu options.
        /// </summary>
        public void Update()
        {
            foreach (Option o in _options)
                o.Update(_selectedIndex);
        }

        /// <summary>
        /// Changes the currently selected item.
        /// </summary>
        /// <param name="direction">The direction to move. -1 for up, 1 for down.</param>
        public void ChangeSelection(int direction)
        {
            if (direction > 0)
                _selectedIndex++;
            if (direction < 0)
                _selectedIndex--;

            if (_selectedIndex < 0)
                _selectedIndex = _options.Count - 1;
            else if (_selectedIndex > _options.Count - 1)
                _selectedIndex = 0;
        }

        /// <summary>
        /// Gets the currently selected option.
        /// </summary>
        public string SelectedOption { get { return _options[_selectedIndex].Text; } }

        /// <summary>
        /// Resets a menu for re-use.
        /// </summary>
        public void Reset()
        {
            _selectedIndex = 0;
        }
    }
}
