using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABRelation
{
    public ABRelation(string bundleName, AssetLoading loading)
    {
        this.bundleName = bundleName;
        this.assetLoading = loading;
        this.isLoadComplete = false;

        list_dependenceBundles = new List<string>();
        list_referenceBundles = new List<string>();

        this.abLoader = new ABLoader(bundleName, loading, LoadComplete);
    }

    private ABLoader abLoader;

    private string bundleName;
    private bool isLoadComplete;
    public bool _isLoadComplete { get { return isLoadComplete; } }

    private AssetLoading assetLoading;
    public AssetLoading _assetLoading { get { return assetLoading; } }

    public IEnumerator Load()
    {
        yield return abLoader.LoadAssetBundle();
    }

    /// <summary>
    /// callbakc when load complete
    /// </summary>
    /// <param name="bundleName"></param>
    private void LoadComplete(string bundleName)
    {
        this.isLoadComplete = true;
    }

    #region Dependence
    private List<string> list_dependenceBundles;

    public void AddDependence(string bundleName)
    {
        if (string.IsNullOrEmpty(bundleName))
        {
            return;
        }

        if (list_dependenceBundles.Contains(bundleName))
        {
            return;
        }

        list_dependenceBundles.Add(bundleName);
    }

    public void RemoveDependence(string bundleName)
    {
        if (list_dependenceBundles.Contains(bundleName))
        {
            list_dependenceBundles.Remove(bundleName);
        }
    }

    public string[] GetAllDependence()
    {
        return list_dependenceBundles.ToArray();
    }

    #endregion

    #region Reference
    private List<string> list_referenceBundles;

    public void AddReference(string bundleName)
    {
        if (string.IsNullOrEmpty(bundleName))
        {
            return;
        }

        if (list_referenceBundles.Contains(bundleName))
        {
            return;
        }

        list_referenceBundles.Add(bundleName);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bundleName"></param>
    /// <returns> if return true,means this bundle has been disposed, the opposite is has't </returns>
    public bool RemoveReference(string bundleName)
    {
        if (list_referenceBundles.Contains(bundleName))
        {
            list_referenceBundles.Remove(bundleName);

            //if this bundle has not reference, dispose it
            if (list_referenceBundles.Count >= 0)
            {
                return false;
            }

            Dispose();
            return true;
        }

        return false;
    }

    public string[] GetAllReference()
    {
        return list_dependenceBundles.ToArray();
    }
    #endregion

    #region LoadAsset
    public T LoadAsset<T>(string assetName) where T : class
    {
        if (abLoader == null)
        {
            Common.Error("ABLoader is NULL!");
            return null;
        }

        return abLoader.LoadAsset<T>(assetName);
    }

    public Object[] LoadAllAssets()
    {
        if (abLoader == null)
        {
            Common.Error("ABLoader is NULL!");
            return null;
        }

        return abLoader.LoadAllAssets();
    }

    public Object[] LoadAssetWithSubAssets(string assetName)
    {
        if (abLoader == null)
        {
            Common.Error("ABLoader is NULL!");
            return null;
        }

        return abLoader.LoadAssetWithSubAssets(assetName);
    }
    #endregion

    #region UnloadAsset
    public void UnLoadAsset(Object asset)
    {
        if (abLoader == null)
        {
            Common.Error("ABLoader is NULL!");
            return;
        }

        abLoader.UnLoadAsset(asset);
    }

    public void Dispose()
    {
        if (abLoader == null)
        {
            return;
        }

        //false : unload assetbundle only
        //true : unload assetbundle and obj
        abLoader.Dispose();
        abLoader = null;
    }
    #endregion

    public void GetAllAssetNames()
    {
        abLoader.GetAllAssetNames();
    }

}
