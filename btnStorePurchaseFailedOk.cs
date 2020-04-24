using System.Collections;
using System.Collections.Generic;
using ThirdEyeSoftware.GameLogic;
using ThirdEyeSoftware.GameLogic.LogicHandlers;
using UnityEngine;

namespace ThirdEyeSoftware.UnityProject
{
    public class btnStorePurchaseFailedOk : MonoBehaviour, IComponent
    {

        public ILogicHandler LogicHandler { get; set; }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnClick()
        {
            LogicHandler.OnClick(this.gameObject.name);
        }
    }
}