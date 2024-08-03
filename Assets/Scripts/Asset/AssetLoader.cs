using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetLoader : System.IDisposable
{
    public AssetLoader(AssetBundle ab)
    {
        this.AB = ab;
    }

    /// <summary>
    /// current AssetBundle
    /// </summary>
    private AssetBundle ab;

    public AssetBundle AB
    {
        set { this.ab = value; }
    }

    #region Load
    public T LoadAsset<T>(string assetName) where T : class
    {
        if (ab == null)
        {
            Common.Error(ab.name + " is NULL, can't get" + assetName);
            return default(T);
        }
        else if(!ab.Contains(assetName))
        {
            Common.Error(ab.name + " isn't contains" + assetName);
            return default(T);
        }

        return ab.LoadAsset(assetName) as T; 
    }

    public Object[] LoadAllAssets()
    {
        if (ab == null)
        {
            Common.Error(ab.name + " is NULL, can't get resource!");
            return null;
        }

        return ab.LoadAllAssets();
    }

    public Object[] LoadAssetWithSubAssets(string assetName)
    {
        if (ab == null)
        {
            Common.Error(ab.name + " is NULL, can't get" + assetName);
            return null;
        }
        else if (!ab.Contains(assetName))
        {
            Common.Error(ab.name + " isn't contains" + assetName);
            return null;
        }

        return ab.LoadAssetWithSubAssets(assetName);
    }
    #endregion

    #region Unload
    public void UnLoadAsset(Object asset)
    {
        Resources.UnloadAsset(asset);
    }

    public void Dispose()
    {
        if (ab == null)
        {
            return;
        }

        //false : unload assetbundle only
        //true : unload assetbundle and obj
        ab.Unload(false);
    }
    #endregion

    public void GetAllAssetNames()
    {
        string[] names = ab.GetAllAssetNames();

        foreach (string item in names)
        {
            Common.Log(item);
        }
    }
}
