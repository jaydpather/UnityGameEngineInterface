using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.UnityProject
{
    public class ScreenUtils : IScreenUtils
    {
        public IVector3 GetScreenPointFromWorldPoint(IVector3 worldPoint)
        {
            var vector3 = Camera.main.WorldToScreenPoint(new Vector3(worldPoint.X, worldPoint.Y, worldPoint.Z));

            return new Vector3Wrapper(vector3);
        }

        public IVector3 GetWorldPointFromScreenPoint(IVector3 sceenPoint)
        {
            var vector3 = Camera.main.ScreenToWorldPoint(new Vector3(sceenPoint.X, sceenPoint.Y, sceenPoint.Z)); //z coordinate is distance from camera, in game units

            return new Vector3Wrapper(vector3);
        }
    }
}
