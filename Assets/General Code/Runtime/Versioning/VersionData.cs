using System.IO;
using UnityEngine;

public static class VersionData
{
    // Name of the created file, does not have to be readable, it will be thrown into resources and should never appear in editor
    // Name is like that to avoid conflict
    private const string FileName = "86EBCEC5-071C-45EB-965B-C34D38D16C2C";

    // Placeholders for data
    private const string NoVersionPlaceholder = "__.__.__";
    private const string NoHashPlaceholder = "8f621b309dcdd386214b3926f77ae912eea801cd";

    // Directory for 
    private readonly static string directory = Path.Combine(Application.dataPath, "Resources");
    private readonly static string path = Path.Combine(directory, $"{FileName}.txt");


    // Holds data, have to be used because of serialization
    [System.Serializable]
    class VersionDataHolder
    {
        public string Version = "";
        public string Hash = "";
    }


    // ----
    private static VersionDataHolder data;
    private static bool CanReturnData => Application.isPlaying && data != null;

    // Returns current data or placeholder if there is none (for edit mode)
    public static string Version => CanReturnData ? data.Version : NoVersionPlaceholder;
    public static string Hash => CanReturnData ? data.Hash : NoHashPlaceholder;

    // Loads data or creates it when game starts
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    private static void DeserializeOnAppStat()
    {
#if !UNITY_EDITOR
        LoadFromFile();
#else
        PopulateFromGit();
#endif
    }

    public static void LoadFromFile()
    {
        string json = Resources.Load<TextAsset>(FileName).text;
        data = JsonUtility.FromJson<VersionDataHolder>(json);
    }

    // Editor code, won't be compiled and should not be
    // It uses git that is not present in build
#if UNITY_EDITOR
    public static void StoreInFile()
    {
        string json = JsonUtility.ToJson(data);

        // Writes json to path
        if (!Directory.Exists(directory)) {
            Directory.CreateDirectory(directory);
        }
        File.WriteAllText(path, json);
    }

    public static void ClearFiles()
    {
        if (File.Exists(path)) {
            File.Delete(path);
            File.Delete($"{path}.meta");
        }
    }

    public static void PopulateFromGit()
    {
        try
        {
            data = new VersionDataHolder()
            {
                Version = Build.Git.BuildVersion,
                Hash = Build.Git.Hash
            };
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"{e} thrown, no version data will be added");
            data = new VersionDataHolder()
            {
                Version = NoVersionPlaceholder,
                Hash = "version data was not found"
            };
        }
    }
#endif
}


