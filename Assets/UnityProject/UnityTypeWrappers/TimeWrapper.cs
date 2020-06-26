using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.UnityProject.UnityTypeWrappers
{
    internal class TimeWrapper : ITime
    {
        public float DeltaTime
        {
            get
            {
                return Time.deltaTime;
            }
        }
    }
}
