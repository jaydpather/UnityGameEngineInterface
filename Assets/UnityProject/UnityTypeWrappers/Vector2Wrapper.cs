using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.UnityProject.UnityTypeWrappers
{
    public class Vector2Wrapper : IVector2
    {
        private Vector2 _vector2;

        public Vector2Wrapper(Vector2 vector2)
        {
            _vector2 = vector2;
        }

        public float X
        {
            get
            {
                return _vector2.x;
            }

            set
            {
                _vector2.x = value;
            }
        }

        public float Y
        {
            get
            {
                return _vector2.y;
            }

            set
            {
                _vector2.y = value;
            }
        }
    }
}
