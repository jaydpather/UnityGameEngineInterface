using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThirdEyeSoftware.GameLogic.LogicHandlers;
using ThirdEyeSoftware.GameLogic;

namespace ThirdEyeSoftware.UnityProject
{
    public class btnQuitGame : MonoBehaviour, IComponent
    {
        public ILogicHandler LogicHandler { get; set; }

        void OnClick()
        {
            LogicHandler.OnClick(this.gameObject.name);
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}