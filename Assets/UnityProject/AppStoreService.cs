using System;
using System.Collections.Generic;
using ThirdEyeSoftware.GameLogic;
using UnityEngine;
using UnityEngine.Purchasing;

namespace ThirdEyeSoftware.UnityProject
{


    public class AppStoreService : MonoBehaviour, IStoreListener, IAppStoreService
    {

        private IStoreController _unityStoreController;
        private IExtensionProvider _storeSpecificExtensionProvider;

        public Action OnInitializationFailedEventHandler { get; set; }
        public Action OnPurchaseFailedEventHandler { get; set; }
        public Action<string> OnPurchaseSucceededEventHandler { get; set; }
        public Action<string> LogToDebugOutput { get; set; }
        public Action<List<ProductInfo>> OnAppStoreInitialized { get; set; }

        //private static readonly AppStoreService _instance = new AppStoreService();

        public AppStoreService()
        {

        }

        //public static AppStoreService Instance
        //{
        //    get
        //    {
        //        return _instance;
        //    }
        //}

        private void _logToDebugOutput(string text)
        {
            if (LogToDebugOutput != null)
            {
                LogToDebugOutput(text);
            }
        }

        public bool IsInitialized
        {
            get
            {
                return (_unityStoreController != null && _storeSpecificExtensionProvider != null);
            }
        }

        public void Initialize()
        {
            //_logToDebugOutput("AppStoreService.Initialize()");
            if (IsInitialized)
            {
                //silent failure - this is a success case
                //_logToDebugOutput("AppStoreService.Initialize() - already initalized");
                return;
            }

            try
            {
                var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

                builder.AddProduct(Constants.ProductNames.BuyLivesSmall, ProductType.Consumable,
                    new IDs
                    {
                    { Constants.ProductNames.BuyLivesSmall, Constants.AppStoreNames.GooglePlay }
                    }
                );

                builder.AddProduct(Constants.ProductNames.BuyLivesMedium, ProductType.Consumable,
                    new IDs
                    {
                    { Constants.ProductNames.BuyLivesMedium, Constants.AppStoreNames.GooglePlay }
                    }
                );

                builder.AddProduct(Constants.ProductNames.BuyLivesLarge, ProductType.Consumable,
                    new IDs
                    {
                    { Constants.ProductNames.BuyLivesLarge, Constants.AppStoreNames.GooglePlay }
                    }
                );

                //this async method will get a response either in OnInitialized or OnInitializeFailed (callback methods)
                UnityPurchasing.Initialize(this, builder);

                //todo real game: use ProductType.Consumable for products such as upgrade points that can be spent on health, speed, damage, etc., or ammo (which must be consumed by firing).
                //builder.AddProduct(kProductIDConsumable, ProductType.Consumable);
            }
            catch (Exception ex)
            {
                _logToDebugOutput("caught exception during Initialize():");
                _logToDebugOutput($"message: {ex.Message}");
                _logToDebugOutput($"stack trace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// initiates an async call to the Unity store controller. There is 1 callback for the success case and 1 callback for the error case.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>true of we were able to initiate the purchase, otherwise false. Note that even if we were able to initiate the purchase, the error callback method could still be called.</returns>
        public bool BuyProductByID(string productId)
        {
            var retVal = false;

            try
            { 
                if (IsInitialized)
                {
                    Product product = _unityStoreController.products.WithID(productId);

                    if (product != null && product.availableToPurchase)
                    {
                        _unityStoreController.InitiatePurchase(product);
                        retVal = true;
                    }
                    else if(product == null)
                    {
                        _logToDebugOutput($"product id '{productId}' is null");
                        retVal = false;
                    }
                    else if(!product.availableToPurchase)
                    {
                        _logToDebugOutput($"product id '{productId}' not available to purchase");
                        retVal = false;
                    }
                }
                else
                {
                    //silent failure
                    //we'll reach here if the user has no internet connection, or if this class wasn't able to Initialize by the time the user clicked the 'buy' button.
                    //hopefully the user will just try again, and by that time the app store service will be initialized.
                    _logToDebugOutput("Still connecting to Google Play Store. Please check your internet connection or try again in a few seconds.");
                }
            }
            catch(Exception ex)
            {
                _logToDebugOutput($"caught exception in BuyProductById:");
                _logToDebugOutput($"product id: {productId}");
                _logToDebugOutput($"message: {ex.Message}");
                _logToDebugOutput($"stack trace: {ex.StackTrace}");
            }

            return retVal;
        }


        #region IStoreListner
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            try
            { 
                _unityStoreController = controller;
                _storeSpecificExtensionProvider = extensions;

                var productInfos = new List<ProductInfo>();
                foreach(var curProduct in controller.products.all)
                {
                    productInfos.Add(new ProductInfo
                    {
                        ProductId = curProduct.definition.id,
                        Price = curProduct.metadata.localizedPrice,
                        PriceString = curProduct.metadata.localizedPriceString,
                    });
                }

                OnAppStoreInitialized(productInfos);
            }
            catch(Exception ex)
            {
                _logToDebugOutput(ex.ToString());
            }
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            _logToDebugOutput("OnInitializeFailed()");
            _logToDebugOutput(string.Format("error: {0}", error.ToString()));

            if (OnInitializationFailedEventHandler != null)
            {
                OnInitializationFailedEventHandler();
            }
        }

        public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
        {
            //UserCancelled means the user tapped outside of the payment dialog, closing it
            //Unknown happens when the user hits the Home button while the purchase dialog is open
            //Existing Purchase Pending can happen when the user attempts a purchase offline, then gets an internet connection and tries again
            if(p != PurchaseFailureReason.UserCancelled && p != PurchaseFailureReason.Unknown) //UserCancelled and Unknown are success cases
            {
                _logToDebugOutput("Purchase Failed:");
                _logToDebugOutput("Reason: " + p.ToString());
            }
            
            //todo real game: save data key for this product id as FALSE
            if (OnPurchaseFailedEventHandler != null)
            {
                OnPurchaseFailedEventHandler();
            }
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
        {
            //_logToDebugOutput("ProcessPurchase()");
            //_logToDebugOutput("\t" + "e.purchasedProduct.availableToPurchase: " + e.purchasedProduct.availableToPurchase);
            //_logToDebugOutput("\t" + "e.purchasedProduct.definition.id: " + e.purchasedProduct.definition.id);
            //_logToDebugOutput("\t" + "e.purchasedProduct.hasReceipt: " + e.purchasedProduct.hasReceipt);
            //_logToDebugOutput("\t" + "e.purchasedProduct.metadata.localizedPrice: " + e.purchasedProduct.metadata.localizedPrice);
            //_logToDebugOutput("\t" + "e.purchasedProduct.receipt: " + e.purchasedProduct.receipt);
            //_logToDebugOutput("\t" + "e.purchasedProduct.transactionID: " + e.purchasedProduct.transactionID);


            //todo real game: how do I handle refunds? (e.g., suppose some guy purchases RemoveAds, then decides he'd rather keep his money and watch ads. Is there an event handler here that gets called? Some how I need to know so that I can save his data key as false and show him ads again!
            PurchaseProcessingResult retVal;

            if (OnPurchaseSucceededEventHandler != null && e.purchasedProduct != null && e.purchasedProduct.definition != null)
            {
                try
                {
                    OnPurchaseSucceededEventHandler(e.purchasedProduct.definition.id);
                    retVal = PurchaseProcessingResult.Complete;
                }
                catch(Exception ex)
                {
                    _logToDebugOutput("caught exception while calling OnPurchaseSucceededEventHandler");
                    _logToDebugOutput($"message: {ex.Message}");
                    _logToDebugOutput($"stack trace: {ex.StackTrace}");

                    //todo real game: log exception. do it in some way where the user can attach the log file in an email.
                    retVal = PurchaseProcessingResult.Pending;
                }
            }
            else
            {
                _logToDebugOutput("ProcessPurchase:");

                if (OnPurchaseSucceededEventHandler == null)
                    _logToDebugOutput("OnPurchaseSucceededEventHandler == null");
                if (e.purchasedProduct == null)
                    _logToDebugOutput("e.purchasedProduct == null");
                if (e.purchasedProduct.definition == null)
                    _logToDebugOutput("e.purchasedProduct.definition == null");

                retVal = PurchaseProcessingResult.Pending;
            }

            return retVal;
        }
        #endregion
    }
}
