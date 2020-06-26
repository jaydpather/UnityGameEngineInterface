using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.UnityProject.UnityTypeWrappers
{
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
}
