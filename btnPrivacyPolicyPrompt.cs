using System.Collections;
using System.Collections.Generic;
using ThirdEyeSoftware.GameLogic;
using ThirdEyeSoftware.GameLogic.LogicHandlers;
using UnityEngine;

public class btnPrivacyPolicyPrompt : MonoBehaviour, IComponent
{
    public ILogicHandler LogicHandler { get; set; }

    void OnClick()
    {
        LogicHandler.OnClick(this.gameObject.name);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
