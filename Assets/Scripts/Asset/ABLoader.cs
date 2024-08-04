using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABLoader : System.IDisposable
{
    public ABLoader(string bundleName, AssetLoading loading, AssetLoadComplete loadComplete)
    {
        this.bundleName = bundleName;
        this.bundlePath = PathUtility.GetWWWPath() + "/" + bundleName;
        this.progress = 0f;
        this.assetLoading = loading;
        this.assetLoadComplete = loadComplete;

        this.www = null;
        this.assetLoader = null;
    }

    private AssetLoader assetLoader;

    private WWW www;

    private string bundleName;
    private string bundlePath;
    private float progress;

    private AssetLoading assetLoading;
    private AssetLoadComplete assetLoadComplete;

    public IEnumerator LoadAssetBundle()
    {
        www = new WWW(bundlePath);

        while(!www.isDone)
        {
            this.progress = www.progress;

            //callback every frame
            if (assetLoading != null)
            {
                assetLoading(bundleName, progress);
            }

            yield return www;
        }
        this.progress = www.progress;

        if (progress >= 1f)
        {
            //allreadly loaded

            assetLoader = new AssetLoader(www.assetBundle);

            if (assetLoadComplete != null)
            {
                assetLoadComplete(bundleName);
            }
        }

    }

    #region LoadAsset
    public T LoadAsset<T>(string assetName) where T : class
    {
        if (assetLoader == null)
        {
            Common.Error("AssetLoader is NULL!");
            return null;
        }

        return assetLoader.LoadAsset<T>(assetName);
    }

    public Object[] LoadAllAssets()
    {
        if (assetLoader == null)
        {
            Common.Error("AssetLoader is NULL!");
            return null;
        }

        return assetLoader.LoadAllAssets();
    }

    public Object[] LoadAssetWithSubAssets(string assetName)
    {
        if (assetLoader == null)
        {
            Common.Error("AssetLoader is NULL!");
            return null; 
        }

        return assetLoader.LoadAssetWithSubAssets(assetName);
    }
    #endregion

    #region UnloadAsset
    public void UnLoadAsset(Object asset)
    {
        if (assetLoader == null)
        {
            Common.Error("AssetLoader is NULL!");
            return;
        }

        assetLoader.UnLoadAsset(asset);
    }

    public void Dispose()
    {
        if (assetLoader == null)
        {
            return;
        }

        //false : unload assetbundle only
        //true : unload assetbundle and obj
        assetLoader.Dispose();
        assetLoader = null;
    }
    #endregion

    public void GetAllAssetNames()
    {
        assetLoader.GetAllAssetNames();
    }

}
