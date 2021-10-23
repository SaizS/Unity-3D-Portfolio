using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [HideInInspector]
    public bool isTown = true;

    public GameObject setting;
    public GameObject canvasUI;

    public Sprite[] changeSceneBtnSprite;
    public Image changeSceneBtnImage;

    private List<Button> bgmBtn = new List<Button>();
    private List<Button> sfxBtn = new List<Button>();

    public DataInfo.GameData gameData;

    public delegate void SaveData();
    public SaveData save;

    private void Awake()
    {
        instance = this;
        DataManager.instance.Initialize();
        gameData = DataManager.instance.Load();        
    }

    public void ClickSetting(bool isValue)
    {
        setting.SetActive(isValue);
    }

    public void ChangeScene()
    {
        if (isTown)
        {
            changeSceneBtnImage.sprite = changeSceneBtnSprite[0];
            if(GameObject.Find("BuildList") != null)
                GameObject.Find("BuildList").SetActive(false);
        }
        else
        {
            changeSceneBtnImage.sprite = changeSceneBtnSprite[1];
            if (GameObject.Find("TowerList") != null)
                GameObject.Find("TowerList").SetActive(false);

        }

        isTown = !isTown;

        BuildUI.instance.OffUI(true);

        CameraControl.instance.MediateCam();
    }

    public void SaveGameData()
    {
        gameData.buildingDatas.Clear();
        gameData.curResource.Clear();

        for (int i = (int)GameResourceType.GOLD; i<= (int)GameResourceType.SUPPLY; i++)
            gameData.curResource.Add(ResourceManager.instance.gameResources.gameResourceInfos[i].curResource);

        if(save != null)
            save();

        gameData.waveLevel = WaveManager.instance.waveCount;
        DataManager.instance.Save(gameData);
    }

    public void SaveBuildingData(string key, string buildingPosName, int level)
    {
        DataInfo.BuildingData buildingData = new DataInfo.BuildingData();
        buildingData.key = key;
        buildingData.buildingPosName = buildingPosName;
        buildingData.level = level;

        gameData.buildingDatas.Add(buildingData);
    }

    public void RemoveBuildingData(string buidlingPosName)
    {
        for(int i = 0; i < gameData.buildingDatas.Count; i++)
        {
            if(gameData.buildingDatas[i].buildingPosName == buidlingPosName)
                gameData.buildingDatas.Remove(gameData.buildingDatas[i]);
        }
    }
}
