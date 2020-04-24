using GameLogic.Utils;
using System.Collections;
using System.Collections.Generic;
using ThirdEyeSoftware.GameLogic;
using ThirdEyeSoftware.GameLogic.LogicHandlers;
using UnityEngine;

public class PlayerShipScript : MonoBehaviour, IComponent, IPlayerShipScript
{
    public ILogicHandler LogicHandler { get; set; }
    public IGameEngineInterface GameEngineInterface { get; set; }

    public int Health { get; set; }
    public float HeadingAngle { get; set; }
    public GameLogic.Utils.Vector3 ModelRotation { get; set; } //this is part of the class so that we don't instantiate new objects on each frame.

    //private Dictionary<Collider, bool> _previousCollisions = new Dictionary<Collider, bool>(); //we set the value to true if the player has collided with the key before

    public PlayerShipScript()
    {
        ModelRotation = new GameLogic.Utils.Vector3(0, 0, 0);
        Health = 1;
    }

    // Use this for initialization
    void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        //if(!_previousCollisions.ContainsKey(other) || _previousCollisions[other] == false)
        //{
            //_previousCollisions[other] = true;

            var gameObjWrapper = GameEngineInterface.FindGameObject(other.gameObject.name);
            LogicHandler.OnCollision(this, gameObjWrapper);

            //Debug.Log("OnTriggerEnter");
        //}
    }
}
