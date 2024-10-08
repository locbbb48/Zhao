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

    public Player player;

    [SerializeField] private Light2D light2D;
    public bool isLightOn = false;

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
            foreach (Map map in maps)
            {
                map.gameObject.SetActive(false);
            }
            maps[currentMapIndex].gameObject.SetActive(true);
        }
    }
    void Update()
    {
        LightManager();
        UIManager.Instance.pickupItemLeft.text = "Number of unpicked items: " + maps[currentMapIndex].PickUpItemLeft().ToString()
            + "and " + maps[currentMapIndex].InteractedItemLeft().ToString() + "secret in this Map";
    }

    public void LightManager()
    {
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    isLightOn = !isLightOn;
        //    light2D.enabled = isLightOn;
        //}
        if(isLightOn)
        {
            light2D.enabled = true;
        }
        else
        {
            light2D.enabled = false;
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
