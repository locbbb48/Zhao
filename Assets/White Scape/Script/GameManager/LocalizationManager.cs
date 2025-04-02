/*	  - Codeby Bui Thanh Loc -
	contact : builoc08042004@gmail.com
*/


using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager instance { get; private set; }
    private Dictionary<string, string> localizedText;
    public bool isReady = false;
    public string missingTextString = "Localized text not found";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadLocalizedText(string languageCode)
    {
        StartCoroutine(LoadLocalizedTextCoroutine(languageCode));
    }

    private IEnumerator LoadLocalizedTextCoroutine(string languageCode)
    {
        localizedText = new Dictionary<string, string>();
        string fileName = languageCode + ".json";
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        using (UnityWebRequest request = UnityWebRequest.Get(filePath))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string dataAsJson = request.downloadHandler.text;
                LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

                foreach (LocalizationItem item in loadedData.items)
                {
                    localizedText.Add(item.key, item.value);
                }

                Debug.Log("Loaded localization: " + localizedText.Count + " entries");
                isReady = true;
            }
            else
            {
                Debug.LogError("Cannot find localization file: " + filePath + " | Error: " + request.error);
            }
        }
    }

    public string GetLocalizedValue(string key)
    {
        if (localizedText != null && localizedText.ContainsKey(key))
        {
            return localizedText[key];
        }
        return missingTextString;
    }
}

[System.Serializable]
public class LocalizationData
{
    public LocalizationItem[] items;
}

[System.Serializable]
public class LocalizationItem
{
    public string key;
    public string value;
}


//using System.Collections.Generic;
//using UnityEngine;
//using System.IO;

//public class LocalizationManager : MonoBehaviour
//{
//    public static LocalizationManager instance { get; private set; }
//    private Dictionary<string, string> localizedText;
//    public bool isReady = false;
//    public string missingTextString = "Localized text not found";

//    void Awake()
//    {
//        if (instance == null)
//        {
//            instance = this;
//            DontDestroyOnLoad(gameObject);
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    public void LoadLocalizedText(string languageCode)
//    {
//        localizedText = new Dictionary<string, string>();
//        string fileName = languageCode + ".json";
//        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

//        if (File.Exists(filePath))
//        {
//            string dataAsJson = File.ReadAllText(filePath);
//            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);
//            foreach (LocalizationItem item in loadedData.items)
//            {
//                localizedText.Add(item.key, item.value);
//            }
//            Debug.Log("Loaded localization: " + localizedText.Count + " entries");
//        }
//        else
//        {
//            Debug.LogError("Cannot find localization file: " + filePath);
//        }
//        isReady = true;
//    }

//    public string GetLocalizedValue(string key)
//    {
//        if (localizedText != null && localizedText.ContainsKey(key))
//        {
//            return localizedText[key];
//        }
//        return missingTextString;
//    }
//}

//[System.Serializable]
//public class LocalizationData
//{
//    public LocalizationItem[] items;
//}

//[System.Serializable]
//public class LocalizationItem
//{
//    public string key;
//    public string value;
//}
