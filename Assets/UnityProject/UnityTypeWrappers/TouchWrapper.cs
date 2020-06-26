using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.UnityProject.UnityTypeWrappers
{
    public class TouchWrapper : ITouch
    {
        private Touch _touch;
        private Vector2Wrapper _position;

        public TouchWrapper(Touch touch)
        {
            _touch = touch;
            _position = new Vector2Wrapper(touch.position);
        }

        public IVector2 Position
        {
            get
            {
                return _position;
            }
        }
    }
}
