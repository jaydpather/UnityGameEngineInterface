using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.UnityProject.UnityTypeWrappers
{
    public class ScreenWrapper : IScreen
    {
        public int Width
        {
            get
            {
                return Screen.width;
            }
        }

        public int Height
        {
            get
            {
                return Screen.height;
            }
        }
    }
}
