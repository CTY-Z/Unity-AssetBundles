using System;
using UnityEditor;
using System.IO;
using UnityEngine;

public class AssetBundlesEditor
{
    #region Auto Mark

    [MenuItem("AssetBundle/Set AssetBundle Labels")]
    public static void SetAssetBundlesLabels()
    {
        AssetDatabase.RemoveUnusedAssetBundleNames();

        String assetDirectory = Application.dataPath + "/ResRoot";
        DirectoryInfo info = new DirectoryInfo(assetDirectory);
        DirectoryInfo[] resRootInfos = info.GetDirectories();

        foreach (DirectoryInfo temp in resRootInfos)
        {
            string resDir = assetDirectory + "/" + temp.Name;
            DirectoryInfo resDirInfo = new DirectoryInfo(resDir);

            if (resDirInfo == null)
            {
                Common.Error(resDir + "Doesn't exists!");
                return;
            }
            else
            {
                int index = resDir.LastIndexOf("/");
                string resName = resDir.Substring(index + 1);
                OnResFileSystemInfo(resDirInfo, resName);
            }
        }

        string[] bundlesNames =  AssetDatabase.GetAllAssetBundleNames();

        foreach (string name in bundlesNames)
        {
            Common.Log(name);
        }

        Common.Log("Set AssetBundles Labels Sucesssfuly!");


    }

    private static void OnResFileSystemInfo(FileSystemInfo fsInfo, string sceneName)
    {
        if (!fsInfo.Exists)
        {
            Common.Error(fsInfo.FullName + "Doesn't exists!");
            return;
        }

        DirectoryInfo dirInfo = fsInfo as DirectoryInfo;
        FileSystemInfo[] fsInfos = dirInfo.GetFileSystemInfos();
        foreach (var temp in fsInfos)
        {
            FileInfo fileInfo = temp as FileInfo;
            if (fileInfo == null)
            {
                OnResFileSystemInfo(temp, sceneName);
            }
            else
            {
                //change resource file's assetbundle labels
                SetLabels(fileInfo, sceneName);
            }
        }

    }

    private static void SetLabels(FileInfo fileInfo, string sceneName)
    {
        if (fileInfo.Extension == ".meta")
        {
            return;
        }

        string bundelName = GetBundleName(fileInfo, sceneName);

        int index = fileInfo.FullName.IndexOf("Assets");
        string assetPath = fileInfo.FullName.Substring(index);

        AssetImporter importer = AssetImporter.GetAtPath(assetPath);
        importer.assetBundleName = bundelName.ToLower();
        if (fileInfo.Extension == ".unity")
        {
            importer.assetBundleVariant = "u3d";
        }
        else
        {
            importer.assetBundleVariant = "assetbundle";
        }
    }

    private static string  GetBundleName(FileInfo fileInfo, string sceneName)
    {
        string windowsPath = fileInfo.FullName;
        string unityPath = windowsPath.Replace(@"\", "/");

        int index = unityPath.IndexOf(sceneName) + sceneName.Length;
        string bundlePath = unityPath.Substring(index + 1);

        if (bundlePath.Contains("/"))
        {
            string[] temp = bundlePath.Split('/');
            return sceneName + "/" + temp[0];
        }

        return sceneName;
    }

    #endregion

    #region Build AssetBundles

    [MenuItem("AssetBundle/Build AssetBundles")]
    public static void BuildAllAssetBundles()
    {
        string outpath = PathUtility.GetAssetBundleOutPath();

        BuildPipeline.BuildAssetBundles(outpath, 0, BuildTarget.StandaloneWindows64);
    }

    #endregion

    #region delete

    [MenuItem("AssetBundle/Delete All")]
    public static void DeleteAllAssetBundles()
    {
        string outpath = PathUtility.GetAssetBundleOutPath();

        Directory.Delete(outpath, true);
        File.Delete(outpath + ".meta");
    }

    #endregion
}
