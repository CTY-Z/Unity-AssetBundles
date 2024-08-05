using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleSceneABMgr
{
    private Dictionary<string, ABRelation> dir_ABName_ABRelation;

    private string sceneName;

    public SingleSceneABMgr(string sceneName)
    {
        this.sceneName = sceneName;

        dir_ABName_ABRelation = new Dictionary<string, ABRelation>();
    }

    public IEnumerator Load(string bundleName)
    {
        while (!ABManifestLoader._instance._isLoadComplete)
        {
            yield return null;
        }

        if (dir_ABName_ABRelation.TryGetValue(bundleName, out ABRelation abRelation))
        {
            string[] bundlesDependence = ABManifestLoader._instance.GetAllDependencies(bundleName);

            foreach (string item in bundlesDependence)
            {
                abRelation.AddDependence(item);
                yield return LoadDependence(item, bundleName, abRelation._assetLoading);
            }

            //load assetbundle
            yield return abRelation.Load();
        }
        else
        {
            Common.Error(bundleName + "doesn't exist!");
        }
    }

    private IEnumerator LoadDependence(string bundleName, string referenceBundleName, AssetLoading al)
    {
        ABRelation abRelation;
        if (dir_ABName_ABRelation.TryGetValue(bundleName, out abRelation))
        {
            abRelation.AddReference(referenceBundleName);
        }
        else
        {
            abRelation = new ABRelation(bundleName, al);
            abRelation.AddReference(referenceBundleName);
            dir_ABName_ABRelation.Add(bundleName, abRelation);
        }

        yield return Load(bundleName);
    }

    #region Load Asset form assetbundle
    public T LoadAsset<T>(string bundleName, string assetName) where T : class
    {
        if (!dir_ABName_ABRelation.TryGetValue(bundleName, out ABRelation abRelaiton))
        {
            Common.Error(bundleName + " is NULL!");
            return null;
        }

        return abRelaiton.LoadAsset<T>(assetName);
    }

    public Object[] LoadAllAssets(string bundleName)
    {
        if (!dir_ABName_ABRelation.TryGetValue(bundleName, out ABRelation abRelaiton))
        {
            Common.Error(bundleName + " is NULL!");
            return null;
        }

        return abRelaiton.LoadAllAssets();
    }

    public Object[] LoadAssetWithSubAssets(string bundleName, string assetName)
    {
        if (!dir_ABName_ABRelation.TryGetValue(bundleName, out ABRelation abRelaiton))
        {
            Common.Error(bundleName + " is NULL!");
            return null;
        }

        return abRelaiton.LoadAssetWithSubAssets(assetName);
    }
    #endregion

    #region Unload Asset form assetbundle
    public void UnLoadAsset(string bundleName, Object asset)
    {
        if (!dir_ABName_ABRelation.TryGetValue(bundleName, out ABRelation abRelaiton))
        {
            Common.Error(bundleName + " is NULL!");
            return;
        }

        abRelaiton.UnLoadAsset(asset);
    }

    public void Dispose(string bundleName)
    {
        if (!dir_ABName_ABRelation.TryGetValue(bundleName, out ABRelation abRelaiton))
        {
            return;
        }

        //false : unload assetbundle only
        //true : unload assetbundle and obj
        abRelaiton.Dispose();
        abRelaiton = null;
    }
    #endregion

    public void GetAllAssetNames(string bundleName)
    {
        if (dir_ABName_ABRelation.TryGetValue(bundleName, out ABRelation abRelaiton))
        {
            abRelaiton.GetAllAssetNames();
        }

    }

}
