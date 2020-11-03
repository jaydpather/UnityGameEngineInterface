using System.Collections;
using System.Collections.Generic;
using ThirdEyeSoftware.UnityProject;
using ThirdEyeSoftware.GameLogic;
using ThirdEyeSoftware.GameLogic.LogicHandlers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ThirdEyeSoftware.UnityProject
{
    public class SphereBehavior : MonoBehaviour, IComponent, ISphereBehavior
    {
        public float Speed { get; set; }
        public float Health { get; set; }
        public ILogicHandler LogicHandler { get; set; }
        public IGameEngineInterface GameEngineInterface { get; set; }

        public SphereBehavior()
        {
        }

        private void Awake()
        {
            //this event is called before start
        }

        // Use this for initialization
        void Start()
        {
            //Debug.Log("Hello");
            Speed = 5f;
            Health = 10f;
        }

        private void OnCollisionEnter(Collision collision)
        {
            //Debug.Log("OnCollisionEnter");
        }

        private void OnCollisionStay(Collision collision)
        {
            //Debug.Log("OnCollisionStay");
        }

        private void OnCollisionExit(Collision collision)
        {
            //Debug.Log("OnCollisionExit");
        }

        private void OnTriggerEnter(Collider other)
        {
            //todo 2nd game: remove this class. it should be dead code
            var gameObjWrapper = GameEngineInterface.FindGameObject(other.gameObject.name);

            LogicHandler.OnCollision(this, gameObjWrapper);
            //Debug.Log("OnTriggerEnter");
        }

        private void OnTriggerStay(Collider other)
        {
        }

        private void OnTriggerExit(Collider other)
        {
            //Debug.Log("OnTriggerExit");
        }

        private void FixedUpdate()
        {
            //tutorial code says to do physics updates here. (e.g., calling rigidBody.AddForce())
        }

        private void LateUpdate()
        {
            //tutorial code says this event fires after the position of all objects has been updated.
            //(e.g., you could use this to make the camera track a target object)
            //(calls Camera.main.transform.LookAt(target.transform))
        }
    }
}