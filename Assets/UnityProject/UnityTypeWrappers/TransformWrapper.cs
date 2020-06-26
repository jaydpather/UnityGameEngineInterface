using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.UnityProject.UnityTypeWrappers
{
    public class TransformWrapper : ITransform
    {
        private Transform _transform;
        private Vector3Wrapper _position = new Vector3Wrapper(new Vector3(0, 0, 0));
        private Vector3 _eulerAngles = new Vector3(0, 0, 0);
        private Vector3 _rotationAngles = new Vector3(0, 0, 0);

        public Transform Transform
        {
            get
            {
                return _transform;
            }

            set
            {
                _transform = value;
            }
        }

        public TransformWrapper(Transform transform)
        {
            _transform = transform;
        }

        public float EulerAngleX
        {
            set
            {
                _eulerAngles.x = value;
                _eulerAngles.y = _transform.eulerAngles.y;
                _eulerAngles.z = _transform.eulerAngles.z;

                _transform.eulerAngles = _eulerAngles;
            }
        }

        public void Translate(float x, float y, float z)
        {
            _transform.Translate(x, y, z, Space.World);
        }

        public void Scale(float x, float y, float z)
        {
            _transform.localScale = new Vector3(x, y, z);
        }

        public void Rotate(IVector3 rotations)
        {
            _rotationAngles.x = rotations.X;
            _rotationAngles.y = rotations.Y;
            _rotationAngles.z = rotations.Z;

            _transform.Rotate(_rotationAngles, Space.World); //todo: memory allocation
        }

        public IVector3 Position
        {
            get
            {
                _position.X = _transform.position.x;
                _position.Y = _transform.position.y;
                _position.Z = _transform.position.z;

                return _position;
            }

            set
            {
                _transform.position = new Vector3(value.X, value.Y, value.Z);
            }
        }
    }
}
