﻿using System.Collections;
using System.Collections.Generic;
using ThirdEyeSoftware.GameLogic;
using ThirdEyeSoftware.GameLogic.LogicHandlers;
using UnityEngine;

public class btnChooseLevelCancel : MonoBehaviour, IComponent {

    public ILogicHandler LogicHandler { get; set; }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        LogicHandler.OnClick(this.gameObject.name);
    }
}
