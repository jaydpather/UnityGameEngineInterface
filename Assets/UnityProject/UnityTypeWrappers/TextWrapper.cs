using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.UnityProject.UnityTypeWrappers
{
    public class TextWrapper : IText
    {
        Text _text;

        public TextWrapper(Text text)
        {
            _text = text;
        }

        public string Text
        {
            get
            {
                return _text.text;
            }

            set
            {
                _text.text = value;

            }
        }

        public void SetColor(float r, float g, float b)
        {
            _text.color = new Color(r, g, b);
        }
    }
}
