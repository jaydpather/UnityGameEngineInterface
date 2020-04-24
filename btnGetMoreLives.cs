using System.Collections;
using System.Collections.Generic;
using ThirdEyeSoftware.GameLogic;
using ThirdEyeSoftware.GameLogic.LogicHandlers;
using UnityEngine;

namespace ThirdEyeSoftware.UnityProject
{
    public class btnGetMoreLives : MonoBehaviour, IComponent
    {

        public ILogicHandler LogicHandler { get; set; }

        void Start()
        {

        }

        void Update()
        {

        }

        public void OnClick()
        {
            LogicHandler.OnClick(this.gameObject.name);
        }
    }
}