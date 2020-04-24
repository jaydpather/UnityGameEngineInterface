using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using UnityEngine.Purchasing;
using ThirdEyeSoftware.UnityProject;
using ThirdEyeSoftware.GameLogic.LogicHandlers;
using ThirdEyeSoftware.GameLogic;
using ThirdEyeSoftware.UnityProject;
using System.Net;

namespace ThirdEyeSoftware.UnityProject
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
                if(_itransform != null)
                {
                    return _itransform;
                }

                var dicKey = SourceGameObjectName ?? _gameObject.name; //if this is a cloned game object, then _dicGameObjNamesToUnityTypeNames will only contain the source (parent)  object's name
                var componentName = _dicGameObjNamesToUnityTypeNames[dicKey];

                var component = _gameObject.GetComponent(componentName);
                if(_transformWrapper == null)
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


            if(_dicComponentCache.ContainsKey(typeof(T)))
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

            foreach(var curCollider in colliders)
            {
                if(curCollider.bounds.size.x > xMax)
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

            foreach(var curMaterial in meshRenderer.materials)
            {
                curMaterial.mainTexture.wrapMode = TextureWrapMode.Repeat;
            }
            
        }
    }

    internal class AudioSourceWrapper : IAudioSource
    {
        private AudioSource _audioSource;

        public AudioSourceWrapper(AudioSource audioSource)
        {
            _audioSource = audioSource;
        }
        
        public void Play()
        {
            _audioSource.Play();
        }

        public void PlayOneShot()
        {
            _audioSource.PlayOneShot(_audioSource.clip, 1f);
        }

        public void UnPause()
        {
            _audioSource.UnPause();
        }

        public void Pause()
        {
            _audioSource.Pause();
        }

        public bool IsPlaying
        {
            get
            {
                return _audioSource.isPlaying;
            }
        }

        public string Name
        {
            get
            {
                return _audioSource.name;
            }
        }

        /// <summary>
        /// returns Length in seconds
        /// </summary>
        public float Length
        {
            get
            {
                return _audioSource.clip.length;
            }
            
        }

        public float Time
        {
            set
            {
                _audioSource.time = value;
            }
        }
    }

    internal class TimeWrapper : ITime
    {
        public float DeltaTime
        {
            get
            {
                return Time.deltaTime;
            }
        }
    }

    public class TransformWrapper : ITransform
    {
        private Transform _transform;
        private Vector3Wrapper _position = new Vector3Wrapper(new Vector3(0, 0, 0));
        private Vector3 _eulerAngles = new Vector3(0, 0, 0);
        private Vector3 _rotationAngles = new Vector3(0, 0, 0);

        public Transform Transform
        {
            get
            {
                return _transform;
            }

            set
            {
                _transform = value;
            }
        }

        public TransformWrapper(Transform transform)
        {
            _transform = transform;
        }

        public float EulerAngleX
        {
            set
            {
                _eulerAngles.x = value;
                _eulerAngles.y = _transform.eulerAngles.y;
                _eulerAngles.z = _transform.eulerAngles.z;

                _transform.eulerAngles = _eulerAngles;
            }
        }

        public void Translate(float x, float y, float z)
        {
            _transform.Translate(x, y, z, Space.World);
        }

        public void Scale(float x, float y, float z)
        {
            _transform.localScale = new Vector3(x, y, z);
        }

        public void Rotate(IVector3 rotations)
        {
            _rotationAngles.x = rotations.X;
            _rotationAngles.y = rotations.Y;
            _rotationAngles.z = rotations.Z;

            _transform.Rotate(_rotationAngles, Space.World); //todo: memory allocation
        }

        public IVector3 Position
        {
            get
            {
                _position.X = _transform.position.x;
                _position.Y = _transform.position.y;
                _position.Z = _transform.position.z;

                return _position;
            }

            set
            {
                _transform.position = new Vector3(value.X, value.Y, value.Z);
            }
        }
    }

    public class ScreenWrapper : IScreen
    {
        public int Width
        {
            get
            {
                return Screen.width;
            }
        }

        public int Height
        {
            get
            {
                return Screen.height;
            }
        }
    }

    public class Vector2Wrapper : IVector2
    {
        private Vector2 _vector2;

        public Vector2Wrapper(Vector2 vector2)
        {
            _vector2 = vector2;
        }

        public float X
        {
            get
            {
                return _vector2.x;
            }

            set
            {
                _vector2.x = value;
            }
        }

        public float Y
        {
            get
            {
                return _vector2.y;
            }

            set
            {
                _vector2.y = value;
            }
        }
    }

    public class Vector3Wrapper : IVector3
    {
        private Vector3 _vector3;

        public Vector3Wrapper(Vector3 vector3)
        {
            _vector3 = vector3;
        }

        public float X
        {
            get
            {
                return _vector3.x;
            }

            set
            {
                _vector3.x = value;
            }
        }

        public float Y
        {
            get
            {
                return _vector3.y;
            }

            set
            {
                _vector3.y = value;
            }
        }

        public float Z
        {
            get
            {
                return _vector3.z;
            }

            set
            {
                _vector3.z = value;
            }
        }
    }

    public class TouchWrapper : ITouch
    {
        private Touch _touch;
        private Vector2Wrapper _position;

        public TouchWrapper(Touch touch)
        {
            _touch = touch;
            _position = new Vector2Wrapper(touch.position);
        }

        public IVector2 Position
        {
            get
            {
                return _position;
            }
        }
    }

    public class InputWrapper : IInput
    {
        public int TouchCount
        {
            get
            {
                return Input.touchCount;
            }
        }

        public float GetAxis(string axisName)
        {
            return Input.GetAxis(axisName);
        }

        public bool GetButtonUp(string buttonName)
        {
            return Input.GetButtonUp(buttonName);
        }

        public ITouch GetTouch(int index)
        {
            var touch = Input.GetTouch(index);
            return new TouchWrapper(touch);
        }
    }

    public class AdvertisementWrapper : IAdvertisement
    {
        private Action _onAdCompleted;

        public void Initialize()
        {
            //todo real game: conditional compile for iOS vs Android. (google play store Id being used currently)
            //todo real game: config setting for test mode VS real ads
            //Advertisement.Initialize("2629783", true); //commented out b/c we don't use ads anymore
        }

        public bool IsReady()
        {
            return true;
            //does not compile after updating Android SDK: (but unreferenced):
            //return Advertisement.IsReady();
        }

        public void Show(Action onAdCompleted)
        {
            //does not compile after updating Android SDK: (but unreferenced):
            //_onAdCompleted = onAdCompleted;
            //Advertisement.Show(new ShowOptions() { resultCallback = OnAdCompleted });
        }

        //private void OnAdCompleted(ShowResult result)
        //{
        //    _onAdCompleted();
        //}

    }

    public class TextWrapper : IText
    {
        Text _text;

        public TextWrapper(Text text)
        {
            _text = text;
        }

        public string Text
        {
            get
            {
                return _text.text;
            }

            set
            {
                _text.text = value;
                
            }
        }

        public void SetColor(float r, float g, float b)
        {
            _text.color = new Color(r, g, b);
        }
    }

    public class AsyncOperationWrapper : IAsyncOperation
    {
        private AsyncOperation _asyncOperation;

        public AsyncOperationWrapper(AsyncOperation asyncOperation)
        {
            _asyncOperation = asyncOperation;
        }

        public bool AllowSceneActivation
        {
            get
            {
                return _asyncOperation.allowSceneActivation;
            }

            set
            {
                _asyncOperation.allowSceneActivation = value;
            }
        }

        public float Progress
        {
            get
            {
                return _asyncOperation.progress;
            }
        }
    }

    public class ScreenUtils : IScreenUtils
    {
        public IVector3 GetScreenPointFromWorldPoint(IVector3 worldPoint)
        {
            var vector3 = Camera.main.WorldToScreenPoint(new Vector3(worldPoint.X, worldPoint.Y, worldPoint.Z));

            return new Vector3Wrapper(vector3);
        }

        public IVector3 GetWorldPointFromScreenPoint(IVector3 sceenPoint)
        {
            var vector3 = Camera.main.ScreenToWorldPoint(new Vector3(sceenPoint.X, sceenPoint.Y, sceenPoint.Z)); //z coordinate is distance from camera, in game units

            return new Vector3Wrapper(vector3);
        }
    }

    public class UnityGameEngineInterface : MonoBehaviour, IGameEngineInterface
    {
        private static bool _isGooglePlayGamesInitialized = false;

        public static Dictionary<Type, Type> DicUnityExposedTypes = new Dictionary<Type, Type>(); 

        public void CopyToClipBoard(string value)
        {
            GUIUtility.systemCopyBuffer = value;
        }

        public string AppVersion
        {
            get
            {
                return Application.version;
            }
        }

        public int VSyncCount
        {
            set
            {
                QualitySettings.vSyncCount = value;
            }
        }

        private static AdvertisementWrapper _adWrapper = new AdvertisementWrapper();
        private static IAdvertisement _iAdvertisement; //used for UT's ONLY
        public IAdvertisement Advertisement
        {
            get
            {
                if (_iAdvertisement != null)
                    return _iAdvertisement;

                return _adWrapper;
            }
            set
            {
                _iAdvertisement = value;
            }
        }

        private static TimeWrapper _timeWrapper = new TimeWrapper();
        private static ITime itime; //this is used only for unit testing
        public ITime Time
        {
            get
            {
                if(itime != null)
                {
                    return itime;
                }
                return _timeWrapper;
            }

            set
            {
                itime = value;
            }
        }

        private static ScreenWrapper _screenWrapper = new ScreenWrapper();
        private static IScreen _iScreen; //used for UTs only
        public IScreen Screen
        {
            get
            {
                if (_iScreen != null)
                    return _iScreen;

                return _screenWrapper;
            }

            set //used for UTs only
            {
                _iScreen = value;
            }
        }

        private static InputWrapper _inputWrapper = new InputWrapper();
        public IInput Input
        {
            get
            {
                return _inputWrapper;
            }
        }

        private IScreenUtils _iScreenUtils;
        public IScreenUtils ScreenUtils
        {
            get
            {
                if(_iScreenUtils == null)
                {
                    _iScreenUtils = new ScreenUtils();
                }

                return _iScreenUtils;
            }

            set
            {
                _iScreenUtils = value;
            }
        }

        public float TimeScale
        {
            get
            {
                return UnityEngine.Time.timeScale;
            }
            set
            {
                UnityEngine.Time.timeScale = value;
            }
        }

        private Text _txtDebugOutput;
        private Light _mainLight;


        private static Dictionary<string, GameObjectWrapper> _dicGameObjCache = new Dictionary<string, GameObjectWrapper>();
        public IAppStoreService AppStoreService { get; set; } 
        public IGameController GameController { get; set; }

        static UnityGameEngineInterface()
        {
            DicUnityExposedTypes.Add(typeof(ISphereBehavior), typeof(SphereBehavior));
            DicUnityExposedTypes.Add(typeof(IPlayerShipScript), typeof(PlayerShipScript));
            DicUnityExposedTypes.Add(typeof(IAudioSource), typeof(AudioSource));
            DicUnityExposedTypes.Add(typeof(IText), typeof(Text));
        }

        public UnityGameEngineInterface()
        {

        }

        void Start()
        {
            Application.logMessageReceived += Application_logMessageReceived;

            AppStoreService = new UnityProject.AppStoreService();
            GameController = ThirdEyeSoftware.GameLogic.GameController.Instance;

            AppStoreService.OnInitializationFailedEventHandler = OnAppStoreInitFailed;
            AppStoreService.Initialize(); 
            UnityEngine.Screen.fullScreen = false; //todo real game: set this through wrapper Screen class, called from Logic Project   

            GameController.OnStart(this, new UnityDataLayer(), StoreLogicService.Instance); //todo: don't instantiate a new data layer here b/c this method gets called many times
            //var goTxtDebugOutput = GameObject.Find("txtDebugOutput");
            //_txtDebugOutput = goTxtDebugOutput.GetComponent<Text>();

            //InitGooglePlayServices();
        }

        public void MinimizeApp()
        {
            var curActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");

            curActivity.Call<bool>("moveTaskToBack", true);
        }

        public void SetNebulaColor(float r, float g, float b, float a)
        {
            var nebula = GameObject.Find("Nebula1");

            var propBlock = new MaterialPropertyBlock();
            var nebulaRenderer = nebula.GetComponent<Renderer>();
            nebulaRenderer.GetPropertyBlock(propBlock);

            propBlock.SetColor("_TintColor", new Color(r, g, b, a));

            nebulaRenderer.SetPropertyBlock(propBlock);
        }

        //private void LogToDebugOutput(string msg)
        //{
        //    _txtDebugOutput.text += Environment.NewLine;
        //    _txtDebugOutput.text += msg; 
        //}

        private void InitGooglePlayServices()
        {
            //todo: make this compatible with iOS too
            if(!_isGooglePlayGamesInitialized)
            {
                var config = new GooglePlayGames.BasicApi.PlayGamesClientConfiguration.Builder().Build();
                GooglePlayGames.PlayGamesPlatform.InitializeInstance(config);
                GooglePlayGames.PlayGamesPlatform.Activate();
                Social.localUser.Authenticate(GooglePlayGames_OnAuthenticationComplete); //todo: handle success and failure cases
            }
        }

        private void GooglePlayGames_OnAuthenticationComplete(bool success, string msg)
        {
            _isGooglePlayGamesInitialized = true;

            //LogToDebugOutput($"reached callbck. success == {success}, msg == \"{msg}\"");
            //if (success)
            //{
                
            //}
        }

        public void SubmitScoreToLeaderboard(int score, Action<bool> callback)
        {
            Social.ReportScore(score, "CgkI6pm2reYfEAIQAQ", callback);
            Social.ShowLeaderboardUI();
        }

        public void SubmitScoreToLeaderboard(int score)
        {
            Social.ReportScore(score, "CgkI6pm2reYfEAIQAQ", SubmitScore_Complete);
            Social.ShowLeaderboardUI();
        }

        private void SubmitScore_Complete(bool success)
        {
            //LogToDebugOutput("SubmitScore_Complete - UnityGameEngineInterface callback");
            //LogToDebugOutput($"success == {success}");
        }

        private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
        {
            //if(type == LogType.Exception)
            //{
            //    var uniqueId = Guid.NewGuid().ToString();
            //    uniqueId = uniqueId.Substring(0, uniqueId.IndexOf("-"));
            //    var subject = $"support issue {uniqueId}";
            //    var explanation = "Asteroid Field Navigation has crashed. Please help us improve the game by sending us this bug report.";

                //    var bodyRaw = $@"{explanation}

                //CONDITION:{condition}

                //STACK TRACE:{stackTrace}";

                //    var body = WWW.EscapeURL(bodyRaw).Replace("+", "%20");
                //    var url = $@"mailto:{Constants.EmailAddresses.SupportEmail}?subject={subject}&body={body}";

                //    url = url.Replace("\r", string.Empty);
                //    url = url.Replace("\n", "<br/>");
                //                / Application.OpenURL(url);

                //    ExitApp();
                //}
        }

        public void OpenURL(string url)
        {
            Application.OpenURL(url);
        }

        public void ExitApp()
        {
            /**************************************************************************/
            /*              CURRENTLY JUST MINIMIZING INSTEAD OF EXITING              */
            /*                 why?
             *                   * b/c after you exit and restart, purchases do not work      */
            /*                   * Unity's suggested workaround is to call Process.Kill(), but this 
             *                     doesn't really exit the app
             *                   * it also seems to throw an exception, so you don't know what state you're in
            /********************************************************/


            /*
              why do we have to call Process.Kill() before Application.Quit()?
              b/c the app does not completely exit if you only call Application.Quit()
              as a result, the user cannot make purchases after exiting and restarting
              this is a known Unity issue, and the suggested workaround is to call Process.Kill()
            */

            GC.Collect(); //collecting GC before we call Kill() to reduce the chance of memory leaks
            MinimizeApp(); //killing the app doesn't make it disappear from the screen
            System.Diagnostics.Process.GetCurrentProcess().Kill(); //Unity recommeds calling Kill() as a workaround to a purchsing bug. (see bug desc above in /**/ comments).
            Application.Quit(); //seems like this doesn't run after we call Kill(), but leaving it in here anyway, just in case it cleans up a resource lock or something.
        }

        public void SetupLighting()
        {
            //ambient lighting is now set up in the Unity editor
            //RenderSettings.ambientLight = new Color(1, 1, 1);
            //RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat; //todo real game: TriLight also looks good. Google these and see what the difference is.
        }

        public IGameObject Clone(IGameObject gameObject, string nameSuffix)
        {
            GameObjectWrapper retVal = null;

            if(gameObject is GameObjectWrapper)
            {
                var gameObjectWrapper = gameObject as GameObjectWrapper;
                var gameObjectUnity = gameObjectWrapper.GameObject;

                var clone = Instantiate(gameObjectUnity);

                retVal = new GameObjectWrapper(clone);
                retVal.SourceGameObjectName = gameObjectUnity.name;
                retVal.Name += nameSuffix;

                _dicGameObjCache[retVal.Name] = retVal;
            }

            return retVal;
        }

        public void TranslateCamera(float x, float y, float z)
        {
            Camera.main.transform.Translate(new Vector3(x, y, z), Space.World); //todo: memory allocation
        }

        private void OnAppStoreInitFailed()
        {
        }

        public void LogToConsole(string msg)
        {
            Debug.Log(msg);
        }

        // Update is called once per frame
        void Update()
        {
            GameController.OnUpdate();
        }

        public void MakePurchase()
        {

        }

        public void RestorePurchase()
        {

        }

        public void IsInitialized()
        {

        }

        public void ClearGameObjectCache()
        {
            if(_dicGameObjCache != null)
            {
                _dicGameObjCache.Clear();
            }
        }

        public void LoadScene(string sceneName)
        {
            var scene = SceneManager.GetSceneByName(sceneName);
            if (!scene.isLoaded) //todo: will this ever be true? can we remove this if statement?
            {
                _dicGameObjCache.Clear(); //we need to clear the cache b/c when you go back to the previous scene, those cached objects have been destroyed by Unity
                SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            }
        }
        
        /// <summary>
        /// returns null if the scene is already loaded
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        public IAsyncOperation LoadSceneAsync(string sceneName)
        {
            var retVal = (IAsyncOperation)null;
            var scene = SceneManager.GetSceneByName(sceneName);

            _dicGameObjCache.Clear();
            var asyncOp = SceneManager.LoadSceneAsync(sceneName);
            retVal = new AsyncOperationWrapper(asyncOp);
            
            return retVal;
        }

        public IGameObject FindGameObject(string name)
        {
            if(!_dicGameObjCache.ContainsKey(name))
            {
                var gameObject = GameObject.Find(name);
                var gameObjectWrapper = new GameObjectWrapper(gameObject);

                _dicGameObjCache[name] = gameObjectWrapper;
            }
            
            return _dicGameObjCache[name];
        }

        public T[] FindObjectsOfType<T>()
        {
            var unityType = DicUnityExposedTypes[typeof(T)];
            var unityObjs =  GameObject.FindObjectsOfType(unityType);

            var wrapperType = GameObjectWrapper._dicUnityTypesToWrapperTypes[unityType];
            var retVal = Array.CreateInstance(wrapperType, unityObjs.Length);

            for(var i = 0; i<unityObjs.Length; i++)
            {
                var wrapperObject = Activator.CreateInstance(wrapperType, new object[] { unityObjs[i] });
                retVal.SetValue(wrapperObject, i);
            }

            return (T[])retVal.Cast<T>();
        }

        public void OpenShareDialog(string msg)
        {
            //yield return new WaitForEndOfFrame();

            //THIS SHOWS HOW TO TAKE A SCREENSHOT:
            //Texture2D ss = new Texture2D(UnityEngine.Screen.width, UnityEngine.Screen.height, TextureFormat.RGB24, false);
            //ss.ReadPixels(new Rect(0, 0, UnityEngine.Screen.width, UnityEngine.Screen.height), 0, 0);
            //ss.Apply();
            //string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
            //File.WriteAllBytes(filePath, ss.EncodeToPNG());
            // To avoid memory leaks
            //Destroy(ss);

            new NativeShare().SetTitle("Asteroid Field Navigation").SetSubject("Check out this cool game!").SetText(msg).Share();

            // Share on WhatsApp only, if installed (Android only)
            //if( NativeShare.TargetExists( "com.whatsapp" ) )
            //	new NativeShare().AddFile( filePath ).SetText( "Hello world!" ).SetTarget( "com.whatsapp" ).Share();
        }

        public void SetMainLightColor(float r, float g, float b)
        {
            var gameObjectMainLight = GameObject.Find("SunSpotLight");
            _mainLight = gameObjectMainLight.GetComponent<Light>();

            _mainLight.color = new Color(r, g, b);
        }
    }

}