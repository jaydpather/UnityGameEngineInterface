using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.UnityProject.UnityTypeWrappers
{
    public class GameObjectWrapper : IGameObject
    {
        private GameObject _gameObject;
        private TransformWrapper _transformWrapper;
        private static Dictionary<string, string> _dicGameObjNamesToUnityTypeNames = new Dictionary<string, string>();

        public static Dictionary<Type, Type> _dicUnityTypesToWrapperTypes = new Dictionary<Type, Type>();

        private Dictionary<Type, object> _dicComponentCache = new Dictionary<Type, object>();

        public string Name
        {
            get
            {
                return _gameObject.name;
            }

            set
            {
                _gameObject.name = value;
            }
        }

        public string SourceGameObjectName { get; set; }

        public GameObject GameObject
        {
            get
            {
                return _gameObject;
            }
        }

        static GameObjectWrapper()
        {
            //todo 2nd game: make it so you don't need to add a new dictionary entry each time you add a new game object
            _dicGameObjNamesToUnityTypeNames.Add("btnStartGame", "btnStartGame");
            _dicGameObjNamesToUnityTypeNames.Add("btnHowToPlay", "btnHowToPlay");
            _dicGameObjNamesToUnityTypeNames.Add("btnShare", "btnShare");
            _dicGameObjNamesToUnityTypeNames.Add("btnGetMoreLives", "btnGetMoreLives");
            _dicGameObjNamesToUnityTypeNames.Add("btnExitGame", "btnExitGame");
            _dicGameObjNamesToUnityTypeNames.Add("btnPrivacyPolicy", "btnPrivacyPolicy");
            _dicGameObjNamesToUnityTypeNames.Add("btnEULA", "btnEULA");

            _dicGameObjNamesToUnityTypeNames.Add("btnHowToPlayOk", "btnHowToPlayOk");

            _dicGameObjNamesToUnityTypeNames.Add("btnBuyLivesSmall", "btnBuyLivesSmall");
            _dicGameObjNamesToUnityTypeNames.Add("btnBuyLivesMedium", "btnBuyLivesMedium");
            _dicGameObjNamesToUnityTypeNames.Add("btnBuyLivesLarge", "btnBuyLivesLarge");

            _dicGameObjNamesToUnityTypeNames.Add("btnStorePurchaseSucceededOk", "btnStorePurchaseSucceededOk");
            _dicGameObjNamesToUnityTypeNames.Add("btnStorePurchaseFailedOk", "btnStorePurchaseFailedOk");
            _dicGameObjNamesToUnityTypeNames.Add("btnResumeGame", "btnResumeGame");
            _dicGameObjNamesToUnityTypeNames.Add("btnQuitGame", "btnQuitGame");
            _dicGameObjNamesToUnityTypeNames.Add("btnHome", "btnHome");
            _dicGameObjNamesToUnityTypeNames.Add("btnGameOverMainMenu", "btnGameOverMainMenu");
            _dicGameObjNamesToUnityTypeNames.Add("btnGameOverGetMoreLives", "btnGameOverGetMoreLives");
            _dicGameObjNamesToUnityTypeNames.Add("btnGameLoseMainMenu", "btnGameLoseMainMenu");
            _dicGameObjNamesToUnityTypeNames.Add("btnGameLoseTryAgain", "btnGameLoseTryAgain");
            _dicGameObjNamesToUnityTypeNames.Add("btnGameWinOk", "btnGameWinOk");
            _dicGameObjNamesToUnityTypeNames.Add("btnGameWinNextLevel", "btnGameWinNextLevel");
            _dicGameObjNamesToUnityTypeNames.Add("btnGetMoreLivesCancel", "btnGetMoreLivesCancel");
            _dicGameObjNamesToUnityTypeNames.Add("Sphere", "SphereBehavior");
            _dicGameObjNamesToUnityTypeNames.Add("PlayerShip", "PlayerShipScript");
            //_dicGameObjNamesToUnityTypeNames.Add("PlayerShield", "PlayerShieldScript");
            _dicGameObjNamesToUnityTypeNames.Add("Asteroid", "AsteroidScript");
            _dicGameObjNamesToUnityTypeNames.Add("SunSpotLight", "SunSpotLightScript");
            _dicGameObjNamesToUnityTypeNames.Add("ShipSpotLight", "ShipSpotLightScript");
            _dicGameObjNamesToUnityTypeNames.Add("ShipBrighteningSpotLight", "ShipBrighteningSpotLightScript");
            _dicGameObjNamesToUnityTypeNames.Add("Explosion", "ExplosionScript");
            _dicGameObjNamesToUnityTypeNames.Add("btnOutOfLivesOk", "btnOutOfLivesOk");
            _dicGameObjNamesToUnityTypeNames.Add("btnChooseLevelStart", "btnChooseLevelStart");
            _dicGameObjNamesToUnityTypeNames.Add("btnChooseLevelCancel", "btnChooseLevelCancel");
            _dicGameObjNamesToUnityTypeNames.Add("btnNextLevel", "btnNextLevel");
            _dicGameObjNamesToUnityTypeNames.Add("btnPrevLevel", "btnPrevLevel");
            _dicGameObjNamesToUnityTypeNames.Add("btnPause", "btnPause");
            _dicGameObjNamesToUnityTypeNames.Add("Quad", "QuadScript");
            _dicGameObjNamesToUnityTypeNames.Add("MainCamera", "MainCameraScript");
            _dicGameObjNamesToUnityTypeNames.Add("btnEULAAccept", "btnEULAAccept");
            _dicGameObjNamesToUnityTypeNames.Add("btnEULACancel", "btnEULACancel");
            _dicGameObjNamesToUnityTypeNames.Add("btnEULAPrompt", "btnEULAPrompt");
            _dicGameObjNamesToUnityTypeNames.Add("btnPrivacyPolicyPrompt", "btnPrivacyPolicyPrompt");

            _dicGameObjNamesToUnityTypeNames.Add("txtBuyLivesSmallSavePct", "txtBuyLivesSmallSavePct");
            _dicGameObjNamesToUnityTypeNames.Add("txtBuyLivesMediumSavePct", "txtBuyLivesMediumSavePct");
            _dicGameObjNamesToUnityTypeNames.Add("txtBuyLivesLargeSavePct", "txtBuyLivesLargeSavePct");

            _dicUnityTypesToWrapperTypes.Add(typeof(AudioSource), typeof(AudioSourceWrapper));
            _dicUnityTypesToWrapperTypes.Add(typeof(Text), typeof(TextWrapper));
        }

        public GameObjectWrapper(GameObject gameObject)
        {
            _gameObject = gameObject;
        }

        public ILogicHandler LogicHandler
        {
            get
            {
                var typeName = _dicGameObjNamesToUnityTypeNames[_gameObject.name];
                var component = (IComponent)_gameObject.GetComponent(typeName);
                return component.LogicHandler;
            }

            set
            {
                var typeName = _dicGameObjNamesToUnityTypeNames[_gameObject.name];
                var component = (IComponent)_gameObject.GetComponent(typeName);
                component.LogicHandler = value;
            }
        }

        private ITransform _itransform; //this is used only for unit testing
        public ITransform Transform
        {
            get
            {
                if (_itransform != null)
                {
                    return _itransform;
                }

                var dicKey = SourceGameObjectName ?? _gameObject.name; //if this is a cloned game object, then _dicGameObjNamesToUnityTypeNames will only contain the source (parent)  object's name
                var componentName = _dicGameObjNamesToUnityTypeNames[dicKey];

                var component = _gameObject.GetComponent(componentName);
                if (_transformWrapper == null)
                {
                    _transformWrapper = new TransformWrapper(component.transform);
                }
                else
                {
                    _transformWrapper.Transform = component.transform;
                }

                return _transformWrapper;
            }

            set
            {
                _itransform = value;
            }
        }

        public T GetComponent<T>() where T : class
        {
            object retVal = null;


            if (_dicComponentCache.ContainsKey(typeof(T)))
            {
                retVal = _dicComponentCache[typeof(T)];
            }
            else
            {
                var unityType = UnityGameEngineInterface.DicUnityExposedTypes[typeof(T)];

                Component component = null;
                component = _gameObject.GetComponent(unityType.Name);

                if (_dicUnityTypesToWrapperTypes.ContainsKey(unityType))
                {
                    var wrapperType = _dicUnityTypesToWrapperTypes[unityType];

                    var wrapperObj = Activator.CreateInstance(wrapperType, new object[] { component });
                    retVal = wrapperObj;
                }
                else
                {
                    retVal = component;
                }

                _dicComponentCache.Add(typeof(T), retVal);
            }

            return (T)retVal;
        }

        public IVector3 GetSize()
        {
            var colliders = _gameObject.GetComponents<Collider>();

            float xMax = 0;
            float yMax = 0;
            float zMax = 0;

            foreach (var curCollider in colliders)
            {
                if (curCollider.bounds.size.x > xMax)
                {
                    xMax = curCollider.bounds.size.x;
                }

                if (curCollider.bounds.size.y > yMax)
                {
                    yMax = curCollider.bounds.size.y;
                }

                if (curCollider.bounds.size.z > zMax)
                {
                    zMax = curCollider.bounds.size.z;
                }
            }

            return new Vector3Wrapper(new Vector3(xMax, yMax, zMax));
        }

        public void SetMaterialColor(float r, float g, float b)
        {
            _gameObject.GetComponent<Renderer>().material.color = new Color(r, g, b);
        }

        public void SetMaterialColor(float r, float g, float b, float a)
        {
            _gameObject.GetComponent<Renderer>().material.color = new Color(r, g, b, a);
        }

        public void SetActive(bool isActive)
        {
            _gameObject.SetActive(isActive);
        }

        public void EnableTextureWrapping()
        {
            var meshRenderer = _gameObject.GetComponent<MeshRenderer>();
            //meshRenderer.material.mainTexture.wrapMode = TextureWrapMode.Repeat;

            foreach (var curMaterial in meshRenderer.materials)
            {
                curMaterial.mainTexture.wrapMode = TextureWrapMode.Repeat;
            }

        }
    }
}
