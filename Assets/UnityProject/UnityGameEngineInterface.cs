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
using Assets.UnityProject.UnityTypeWrappers;

namespace ThirdEyeSoftware.UnityProject
{
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
            Social.ReportScore(score, "", callback);
            Social.ShowLeaderboardUI();
        }

        public void SubmitScoreToLeaderboard(int score)
        {
            Social.ReportScore(score, "", SubmitScore_Complete);
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