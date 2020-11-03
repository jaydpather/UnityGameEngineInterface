using System.Collections;
using System.Collections.Generic;
using ThirdEyeSoftware.GameLogic;
using ThirdEyeSoftware.GameLogic.LogicHandlers;
using UnityEngine;

public class btnHowToPlayOk : MonoBehaviour, IComponent {

    public ILogicHandler LogicHandler
    {
        get;
        set;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnClick()
    {
        LogicHandler.OnClick(this.gameObject.name);
    }
}
