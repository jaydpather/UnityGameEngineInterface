using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.UnityProject.UnityTypeWrappers
{
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
}
