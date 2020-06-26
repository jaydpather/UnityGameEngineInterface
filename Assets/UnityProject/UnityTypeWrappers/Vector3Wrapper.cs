using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.UnityProject.UnityTypeWrappers
{
    public class Vector3Wrapper : IVector3
    {
        private Vector3 _vector3;

        public Vector3Wrapper(Vector3 vector3)
        {
            _vector3 = vector3;
        }

        public float X
        {
            get
            {
                return _vector3.x;
            }

            set
            {
                _vector3.x = value;
            }
        }

        public float Y
        {
            get
            {
                return _vector3.y;
            }

            set
            {
                _vector3.y = value;
            }
        }

        public float Z
        {
            get
            {
                return _vector3.z;
            }

            set
            {
                _vector3.z = value;
            }
        }
    }
}
