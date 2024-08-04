using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABManifestLoader : System.IDisposable
{
    private static ABManifestLoader instance;
    public static ABManifestLoader _instance
    {
        get 
        { 
            if (instance == null)
            {
                instance = new ABManifestLoader();
            }
            return instance;
        }
    }

    private ABManifestLoader()
    {
        this.manifest = null;
        this.ab = null;
        isLoadComplete = false;

        this.manifestPath = PathUtility.GetWWWPath() + "/" + PathUtility.GetPlatformName();
    }

    private AssetBundleManifest manifest;
    private AssetBundle ab;

    private string manifestPath;
    private bool isLoadComplete;
    public bool _isLoadComplete { get { return isLoadComplete; } }

    public IEnumerator Load()
    {
        WWW www = new WWW(manifestPath);

        yield return www;

        if (www.error != null)
        {
            Common.Error(manifestPath + "loading error :" + www.error);
        }

        if (www.progress >= 1)
        {
            this.ab = www.assetBundle;
            this.manifest = ab.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
            this.isLoadComplete = true;
        }
    }

    public string[] GetAllDependencies(string bundleName)
    {
        return manifest.GetAllDependencies(bundleName);
    }

    public void UnLoadManifest()
    {
        ab.Unload(true);
    }

    public void Dispose()
    {

    }
}
