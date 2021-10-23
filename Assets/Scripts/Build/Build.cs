using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Build : MonoBehaviour
{
    public static Build instance;

    public bool isBuild = false;

    private GameObject buildName; // 버튼 선택시 버튼 이름을 받는다.
    private GameObject buildingParent;
    private List<GameObject> buildings = new List<GameObject>();
    private BuildPosChecker[] buildPos;
    private bool isPosCheck = false;

    public GameObject buildPosList; // 건설 위치 목록
    public Button[] btnList;

    private void Awake()
    {
        instance = this;
        LoadBuilding();

        buildPos = buildPosList.transform.GetComponentsInChildren<BuildPosChecker>();

        for(int i = 0; i < buildings.Count; i++)
        {
            GameObject gameObject = buildings[i];
            btnList[i].onClick.AddListener(delegate () { GetName(gameObject); });
        }

    }

    private void Start()
    {
        LoadGameData();
    }

    private void Update() {
        if(!isPosCheck)
        {
            for (int i = 0; i < buildPos.Length; i++)
            buildPos[i].transform.gameObject.SetActive(false);
            isPosCheck = true;
        }
        else
        return;
    }

    public void GetName(GameObject buildingObject)
    {
        buildName = buildingObject;

        BuildUI.instance.SetBuildInfo(buildingObject);

        isBuild = false;
    }

    public void LoadGameData()
    {
        List<DataInfo.BuildingData> data = new List<DataInfo.BuildingData>();

        data = DataManager.instance.Load().buildingDatas;

        foreach(var temp in data)
        {
            string[] key = temp.key.Split('_');
            GameObject target = GameObject.Find(temp.buildingPosName); // Defence, Archer, Lv

            if(key.Length == 3)
                buildName = Resources.Load<GameObject>("prefabs/Building/" + key[0] + "_" + key[1]);            
            else
                buildName = Resources.Load<GameObject>("prefabs/Building/" + key[0] + "_" + key[1] + "_" + key[2]);
            BuildObject(target, true, temp.level);
        }
    }

    public void BuildObject(GameObject target, bool isLoad = false, int level = 1)
    {
        GameObject tartgetPos = target;
        Collider targetCol = target.GetComponent<MeshCollider>();

        buildingParent = GameObject.Find("Building_Parent");

        Quaternion temp = new Quaternion();
        GameObject building = Instantiate(buildName, tartgetPos.transform.position, temp, buildingParent.transform);

        switch(buildName.tag)
        {
            case "Resource":
            {
                if(buildName.name.Contains("Crystal"))
                    building.GetComponent<Crystal>().InitData(tartgetPos, isLoad, level);
                else
                    building.GetComponent<GameResource>().InitData(buildName.name + "_Lv", tartgetPos, isLoad, level);
            }
            break;
            case "Barrack":
            {
                building.GetComponent<Barrack>().InitData(buildName.name + "_Lv", tartgetPos, isLoad, level);
            }
            break;
            case "DefenceTower":
            {
                building.GetComponent<DefenceTower>().InitData(buildName.name + "_Lv", tartgetPos, isLoad, level);
            }
            break;
            case "ETC":
            {
                if(buildName.name.Contains("Castle"))
                    building.GetComponent<Castle>().InitData(tartgetPos, isLoad, level);
                
                if(buildName.name.Contains("Farm"))
                    building.GetComponent<Farm>().InitData(tartgetPos, isLoad, level);
            }
            break;
        }

        buildName = null;
        isBuild = false;

        if(!isLoad)
        {
            for (int i = 0; i < buildPos.Length; i++)
                buildPos[i].transform.gameObject.SetActive(false);
        }

        tartgetPos.GetComponent<BuildPosChecker>().CheckBuild(true);
    }

    public void BuildPosOff()
    {
        for (int i = 0; i < buildPos.Length; i++)
        {
            if(buildPos[i].onBuildMod)
                buildPos[i].transform.gameObject.SetActive(false);
        }
    }

    private void LoadBuilding()
    {
        string buildingRoute = "prefabs/Building/";

        buildings.Add(Resources.Load<GameObject>(buildingRoute + "ETC_Castle"));
        buildings.Add(Resources.Load<GameObject>(buildingRoute + "ETC_Farm"));
        buildings.Add(Resources.Load<GameObject>(buildingRoute + "Resource_Sawmill"));
        buildings.Add(Resources.Load<GameObject>(buildingRoute + "Resource_Quarry"));
        buildings.Add(Resources.Load<GameObject>(buildingRoute + "Resource_Crystal"));
        buildings.Add(Resources.Load<GameObject>(buildingRoute + "Barrack_Sword"));
        buildings.Add(Resources.Load<GameObject>(buildingRoute + "Barrack_Archer"));
        buildings.Add(Resources.Load<GameObject>(buildingRoute + "Barrack_Spear"));
        buildings.Add(Resources.Load<GameObject>(buildingRoute + "Barrack_Mage"));
        buildings.Add(Resources.Load<GameObject>(buildingRoute + "Defence_Archer"));
        buildings.Add(Resources.Load<GameObject>(buildingRoute + "Defence_Cannon"));
        buildings.Add(Resources.Load<GameObject>(buildingRoute + "Defence_HI_Archer"));
        buildings.Add(Resources.Load<GameObject>(buildingRoute + "Defence_HI_Cannon")); 
    }

    public void ResetBuildingData()
    {
        buildingParent = GameObject.Find("Building_Parent");

        int count = buildingParent.transform.childCount;

        for(int i = 0; i < count; i++)
        {
            GameObject building = buildingParent.transform.GetChild(i).gameObject;
            Destroy(building);
        }

        foreach(var temp in buildPos)
        {
            if(temp.onBuild)
                temp.onBuild = false;
        }
    }
}
