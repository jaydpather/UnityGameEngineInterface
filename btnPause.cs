using System.Collections;
using System.Collections.Generic;
using ThirdEyeSoftware.GameLogic;
using ThirdEyeSoftware.GameLogic.LogicHandlers;
using UnityEngine;

public class btnPause : MonoBehaviour, IComponent
{
    public ILogicHandler LogicHandler
    {
        get; set;
    }

    // Start is called before the first frame update
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
