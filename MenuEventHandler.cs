using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThirdEyeSoftware.GameLogic.LogicHandlers;
using UnityEngine;

namespace ThirdEyeSoftware.UnityProject
{
    class MenuEventHandler : MonoBehaviour
    {
        public void Start()
        {
            MenuLogicHandler.Instance.OnStart();
        }
    }
}
