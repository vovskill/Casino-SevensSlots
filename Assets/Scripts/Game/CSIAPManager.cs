using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using System;

public class CSIAPManager : MonoBehaviour, IStoreListener {

    public Action<Product, CSIAPProduct> handleSuccessPurchase;
    public Action<Product, CSIAPProduct> handleFaliedPurchase;
    public List<CSIAPProduct> products;
    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;

    public static CSIAPManager instance = null;

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
            Loaded();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Loaded()
    {
        if (m_StoreController == null)
        {
            InitializePurchasing(products);
        }
    }

    public void InitializePurchasing(List<CSIAPProduct> products)
    {
        if (IsInitialized())
            return;

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        products.ForEach((i) => { builder.AddProduct(i.productId, i.type); });
        UnityPurchasing.Initialize(this, builder);
    }


    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public Product ProductForId(string productId)
    {
        if (!IsInitialized())
            return null;
        return m_StoreController.products.WithID(productId);
    }

    public string PriceForProductId(string productId)
    {
        Product product = ProductForId(productId);
        return product != null ? product.metadata.localizedPriceString : null;
    }

    public string TitleForProductId(string productId)
    {
        Product product = ProductForId(productId);
        return product != null ? product.metadata.localizedTitle : "";
    }

    public string DescriptionForProductId(string productId)
    {
        Product product = ProductForId(productId);
        return product != null ? product.metadata.localizedDescription : "";
    }

    public void Purchase(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("FAILED: Not purchasing product, either is not found or is not available for purchase");
            }
        }
    }

    public void RestorePurchases()
    {
        if (!IsInitialized())
            return;

        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) => {
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (handleSuccessPurchase != null)
        {
            handleSuccessPurchase(args.purchasedProduct, ProductDataForId(args.purchasedProduct.definition.id));
        }
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        if (handleFaliedPurchase != null)
        {
            handleFaliedPurchase(product, ProductDataForId(product.definition.id));
        }

        Debug.Log(string.Format("FAILED. Product: '{0}', PurchaseFailureReason: {1}",
            product.definition.storeSpecificId, failureReason));
    }

    public CSIAPProduct ProductDataForId(string productId)
    {
        CSIAPProduct product = null;
        for (int i = 0; i < products.Count && product == null; i++)
        {
            if (products[i].productId == productId)
                product = products[i];
        }
        return product;
    }
}
