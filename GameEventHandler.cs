using System;
using System.Collections.Generic;
using ThirdEyeSoftware.GameLogic.LogicHandlers;
using UnityEngine;

namespace ThirdEyeSoftware.UnityProject
{
    public class GameEventHandler : MonoBehaviour
    {
        public void Start()
        {
            GameLogicHandler.Instance.OnStart();
        }

        public void OnApplicationFocus(bool focus)
        {
            GameLogicHandler.Instance.OnFocus(focus);
        }
    }
}