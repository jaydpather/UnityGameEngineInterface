using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.UnityProject.UnityTypeWrappers
{
    public class InputWrapper : IInput
    {
        public int TouchCount
        {
            get
            {
                return Input.touchCount;
            }
        }

        public float GetAxis(string axisName)
        {
            return Input.GetAxis(axisName);
        }

        public bool GetButtonUp(string buttonName)
        {
            return Input.GetButtonUp(buttonName);
        }

        public ITouch GetTouch(int index)
        {
            var touch = Input.GetTouch(index);
            return new TouchWrapper(touch);
        }
    }
}
