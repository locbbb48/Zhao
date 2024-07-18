/*	  - Codeby Bui Thanh Loc -
	contact : builoc08042004@gmail.com
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Light2D light2D;
    [SerializeField] private bool isLightOn = false;

    [SerializeField] private List<Map> maps = new List<Map>();
    [SerializeField] private int currentMapIndex = 0;

    public bool isHaveDagger = false;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }



        if (maps.Count > 0)
        {
            maps[currentMapIndex].gameObject.SetActive(true);
        }
    }
    void Update()
    {
        LightManager();
    }

    public void LightManager()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            isLightOn = !isLightOn;
            light2D.enabled = isLightOn;
        }
    }

    public void LoadNextMap()
    {
        if (currentMapIndex < maps.Count - 1)
        {
            maps[currentMapIndex].gameObject.SetActive(false);
            currentMapIndex++;
            maps[currentMapIndex].gameObject.SetActive(true);
            UIManager.Instance.ShowNoti("You are in new challenge");
        }
        else
        {
            UIManager.Instance.ShowNoti("Waiting for Update new Map...");
        }
    }
}
