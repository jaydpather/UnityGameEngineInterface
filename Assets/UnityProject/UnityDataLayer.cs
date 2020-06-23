using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThirdEyeSoftware.GameLogic;
using UnityEngine;

namespace ThirdEyeSoftware.UnityProject
{
    public class UnityDataLayer : IDataLayer
    {
        public bool GetBool(string key)
        {
            var intValue = PlayerPrefs.GetInt(key, 0);
            return Convert.ToBoolean(intValue);
        }

        public void SetBool(string key, bool value)
        {
            var intValue = Convert.ToInt32(value);
            PlayerPrefs.SetInt(key, intValue);
        }

        public int GetNumLivesRemaining()
        {
            return PlayerPrefs.GetInt(Constants.SavedDataKeys.NumLivesRemaining, Constants.DefaultDataKeyValues.NumLivesRemaining);
        }

        public void IncrementNumLivesRemaining(int numLivesToAdd)
        {
            //todo: try/finally? error handling? (what if we fail to save?)
            var numLivesRemaining = GetNumLivesRemaining();

            numLivesRemaining += numLivesToAdd;

            PlayerPrefs.SetInt(Constants.SavedDataKeys.NumLivesRemaining, numLivesRemaining);

            Save();
        }

        public void DecrementNumLivesRemaining(int numLivesToRemove)
        {
            //todo: try/finally? error handling? (what if we fail to save?)
            var numLivesRemaining = GetNumLivesRemaining();

            if(numLivesRemaining > 0)
                numLivesRemaining -= numLivesToRemove;

            PlayerPrefs.SetInt(Constants.SavedDataKeys.NumLivesRemaining, numLivesRemaining);

            Save();
        }

        public LevelInfo GetCurLevel()
        {            
            var level = PlayerPrefs.GetInt(Constants.SavedDataKeys.CurLevel, 1);
            var subLevel = PlayerPrefs.GetInt(Constants.SavedDataKeys.CurSubLevel, 1);

            var retVal = new LevelInfo(level, subLevel);

            return retVal;
        }

        public void SaveCurLevel(LevelInfo levelInfo)
        {
            PlayerPrefs.SetInt(Constants.SavedDataKeys.CurLevel, levelInfo.Level);
            PlayerPrefs.SetInt(Constants.SavedDataKeys.CurSubLevel, levelInfo.SubLevel);
        }

        public void Save()
        {
            PlayerPrefs.Save();
        }

        public void ClearAllAndSave()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        public bool GetIsEULAAccepted()
        {
            return GetBool(Constants.SavedDataKeys.IsEULAAccepted);
        }

        public void SetIsEULAAccepted(bool value)
        {
            SetBool(Constants.SavedDataKeys.IsEULAAccepted, value);
        }
    }
}
