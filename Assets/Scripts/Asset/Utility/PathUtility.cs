using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PathUtility
{
    public static string GetAssetBundleOutPath()
    {

        string outPath = GetPlatformPath() + "/" + GetPlatformName();

        if (!Directory.Exists(outPath))
        {
            Directory.CreateDirectory(outPath);
        }

        return outPath;
    }

    private static string GetPlatformPath()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                return Application.streamingAssetsPath;

            case RuntimePlatform.Android:
                return Application.persistentDataPath;
            default:
                return null;
        }

    }

    private static string GetPlatformName()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                return "Windows";

            case RuntimePlatform.Android:
                return "Android";
            default:
                return null;
        }
    }

    private static string GetWWWPath()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                return "file:///" + GetPlatformPath();

            case RuntimePlatform.Android:
                return "jar:file:///" + GetPlatformPath();
            default:
                return null;
        }
    }






}
