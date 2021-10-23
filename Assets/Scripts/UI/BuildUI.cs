using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct DefenseTower_Desc
{
    public Text damage;
    public Text attackSpeed;
    public GameObject desc;
}

[System.Serializable]
public struct Resource_Desc
{
    public Text maxResourceAmount;
    public Text secResourceAmount;
    public GameObject desc;
}

[System.Serializable]
public struct Barrack_Desc
{
    public RawImage previewImage;
    public Text name;
    public Text hp;
    public Text attack;
    public Text speed;
    public Text supply;
    public GameObject desc;
}

[System.Serializable]
public struct Castle_Desc
{
    public Text gold;
    public Text tree;
    public Text stone;
    public Text supply;
    public GameObject desc;
}

[System.Serializable]
public struct Farm_Desc
{
    public Text supply;
    public GameObject desc;
}


[System.Serializable]
public struct Sub_Desc
{
    public Text title;
    public Text[] resourcesText;
    public Text btnText;
    public GameObject desc;
}

public class BuildUI : MonoBehaviour
{
    public static BuildUI instance;

    public bool isCheckBuildResource = true;

    public GameObject buildUIPanel;
    public GameObject reinforceBtnObj;
    public GameObject buildBtn;
    public GameObject sellBtnObj;
    public GameObject warningUI;

    public Text title;

    public List<RenderTexture> unitImages = new List<RenderTexture>();

    public DefenseTower_Desc defenseTower_Desc;
    public Resource_Desc resource_Desc;
    public Barrack_Desc barrack_Desc;
    public Castle_Desc castle_Desc;
    public Farm_Desc farm_Desc;
    public Sub_Desc sub_Desc;

    private GameObject selectBuilding;

    private int level;
    private int maxLevel;

    private void Awake()
    {
        instance = this;
        buildUIPanel.SetActive(false);

        unitImages.Add(Resources.Load<RenderTexture>("RenderTexture/Sword"));
        unitImages.Add(Resources.Load<RenderTexture>("RenderTexture/Spear"));
        unitImages.Add(Resources.Load<RenderTexture>("RenderTexture/Archer"));
        unitImages.Add(Resources.Load<RenderTexture>("RenderTexture/Mage"));

        //sub_Desc.resourcesText
    }

    public void SetBuildInfo(GameObject buildingObject)
    {
        InitSetting(true);

        switch (buildingObject.tag)
        {
            case "Resource":
                {
                    ResourceBuildingData data = DataManager.instance.GetResourceData(buildingObject.name + "_Lv1");

                    if (data == null)
                        Debug.Log("data null");

                    title.text = data.name;
                    title.color = data.fontColor;

                    resource_Desc.maxResourceAmount.text = data.maxStorage.ToString();
                    resource_Desc.secResourceAmount.text = data.output.ToString();

                    for (int i = (int)GameResourceType.GOLD; i <= (int)GameResourceType.SUPPLY; i++)
                        sub_Desc.resourcesText[i].text = data.resourceCost[i].ToString();

                    resource_Desc.desc.SetActive(true);

                    if (CheckCost(data))
                        isCheckBuildResource = false;
                }
                break;
            case "Barrack":
                {
                    BarrackData data = DataManager.instance.GetBarrackData(buildingObject.name + "_Lv1");

                    title.text = data.name;
                    title.color = data.fontColor;

                    if (data.name.Contains("검병"))
                    {
                        barrack_Desc.name.text = "검병";
                        barrack_Desc.previewImage.texture = unitImages[0];
                    }
                    else if (data.name.Contains("창병"))
                    {
                        barrack_Desc.name.text = "창병";
                        barrack_Desc.previewImage.texture = unitImages[1];
                    }
                    else if (data.name.Contains("궁병"))
                    {
                        barrack_Desc.name.text = "궁병";
                        barrack_Desc.previewImage.texture = unitImages[2];
                    }
                    else if (data.name.Contains("마법병"))
                    {
                        barrack_Desc.name.text = "마법병";
                        barrack_Desc.previewImage.texture = unitImages[3];
                    }

                    barrack_Desc.hp.text = "체력 : " + data.hp;
                    barrack_Desc.attack.text = "공격력 : " + data.attack;
                    barrack_Desc.speed.text = "이동속도 : " + data.speed;
                    barrack_Desc.supply.text = "유닛 생산수 : " + data.supply;

                    for (int i = (int)GameResourceType.GOLD; i <= (int)GameResourceType.SUPPLY; i++)
                        sub_Desc.resourcesText[i].text = data.resourceCost[i].ToString();

                    barrack_Desc.desc.SetActive(true);

                    if (CheckCost(data))
                        isCheckBuildResource = false;

                }
                break;
            case "DefenceTower":
                {
                    DefenceTowerData data = DataManager.instance.GetDefenceTowerData(buildingObject.name + "_Lv1");

                    title.text = data.name;
                    title.color = data.fontColor;

                    defenseTower_Desc.damage.text = "공격력 : " + data.damage.ToString();
                    defenseTower_Desc.attackSpeed.text = "공격 속도 : " + data.attackSpeed.ToString();
                    defenseTower_Desc.desc.SetActive(false);

                    for (int i = (int)GameResourceType.GOLD; i <= (int)GameResourceType.SUPPLY; i++)
                        sub_Desc.resourcesText[i].text = data.resourceCost[i].ToString();

                    defenseTower_Desc.desc.SetActive(true);

                    if (CheckCost(data))
                        isCheckBuildResource = false;
                }
                break;
            case "ETC":
                {
                    ETCBuildingData data = DataManager.instance.GetETCData(buildingObject.name + "_Lv1");

                    title.text = data.name;
                    title.color = data.fontColor;

                    if (buildingObject.name.Contains("Castle"))
                    {
                        castle_Desc.gold.text = data.firstValueIncreace.ToString();
                        castle_Desc.tree.text = data.firstValueIncreace.ToString();
                        castle_Desc.stone.text = data.firstValueIncreace.ToString();
                        castle_Desc.supply.text = data.secondValueIncreace.ToString();
                        castle_Desc.desc.SetActive(true);
                    }

                    if (buildingObject.name.Contains("Farm"))
                    {
                        farm_Desc.supply.text = data.secondValueIncreace.ToString();
                        farm_Desc.desc.SetActive(true);
                    }

                    sub_Desc.title.text = "건설비용";

                    for (int i = (int)GameResourceType.GOLD; i <= (int)GameResourceType.SUPPLY; i++)
                        sub_Desc.resourcesText[i].text = data.resourceCost[i].ToString();

                    if (CheckCost(data))
                        isCheckBuildResource = false;
                }
                break;
        }

        sub_Desc.title.text = "건설비용";
        buildUIPanel.SetActive(true);
    }

    public bool CheckCost(BuildingCommonData data)
    {
        if (data.resourceCost[(int)GameResourceType.GOLD] > ResourceManager.instance.gameResources.gameResourceInfos[(int)GameResourceType.GOLD].curResource ||
            data.resourceCost[(int)GameResourceType.TREE] > ResourceManager.instance.gameResources.gameResourceInfos[(int)GameResourceType.TREE].curResource ||
            data.resourceCost[(int)GameResourceType.STONE] > ResourceManager.instance.gameResources.gameResourceInfos[(int)GameResourceType.STONE].curResource ||
            ResourceManager.instance.gameResources.gameResourceInfos[(int)GameResourceType.SUPPLY].curResource + data.resourceCost[(int)GameResourceType.SUPPLY] > ResourceManager.instance.gameResources.gameResourceInfos[(int)GameResourceType.SUPPLY].maxResource)
            return true;

        return false;
    }

    //public bool CheckCost(BarrackData data)
    //{
    //    if (data.resourceCost[(int)GameResourceType.GOLD] > ResourceManager.instance.gameResources.gameResourceInfos[(int)GameResourceType.GOLD].curResource ||
    //        data.resourceCost[(int)GameResourceType.TREE] > ResourceManager.instance.gameResources.gameResourceInfos[(int)GameResourceType.TREE].curResource ||
    //        data.resourceCost[(int)GameResourceType.STONE] > ResourceManager.instance.gameResources.gameResourceInfos[(int)GameResourceType.STONE].curResource ||
    //        ResourceManager.instance.gameResources.gameResourceInfos[(int)GameResourceType.SUPPLY].curResource + data.resourceCost[(int)GameResourceType.SUPPLY] > ResourceManager.instance.gameResources.gameResourceInfos[(int)GameResourceType.SUPPLY].maxResource)
    //        return true;
    //    return false;
    //}

    //public bool CheckCost(DefenceTowerData data)
    //{
    //    if (data.resourceCost[(int)GameResourceType.GOLD] > ResourceManager.instance.gameResources.gameResourceInfos[(int)GameResourceType.GOLD].curResource ||
    //        data.resourceCost[(int)GameResourceType.TREE] > ResourceManager.instance.gameResources.gameResourceInfos[(int)GameResourceType.TREE].curResource ||
    //        data.resourceCost[(int)GameResourceType.STONE] > ResourceManager.instance.gameResources.gameResourceInfos[(int)GameResourceType.STONE].curResource ||
    //        ResourceManager.instance.gameResources.gameResourceInfos[(int)GameResourceType.SUPPLY].curResource + data.resourceCost[(int)GameResourceType.SUPPLY] > ResourceManager.instance.gameResources.gameResourceInfos[(int)GameResourceType.SUPPLY].maxResource)
    //        return true;
    //    return false;
    //}

    //public bool CheckCost(ETCBuildingData data)
    //{
    //    if (data.resourceCost[(int)GameResourceType.GOLD] > ResourceManager.instance.gameResources.gameResourceInfos[(int)GameResourceType.GOLD].curResource ||
    //        data.resourceCost[(int)GameResourceType.TREE] > ResourceManager.instance.gameResources.gameResourceInfos[(int)GameResourceType.TREE].curResource ||
    //        data.resourceCost[(int)GameResourceType.STONE] > ResourceManager.instance.gameResources.gameResourceInfos[(int)GameResourceType.STONE].curResource ||
    //        ResourceManager.instance.gameResources.gameResourceInfos[(int)GameResourceType.SUPPLY].curResource + data.resourceCost[(int)GameResourceType.SUPPLY] > ResourceManager.instance.gameResources.gameResourceInfos[(int)GameResourceType.SUPPLY].maxResource)
    //        return true;
    //    return false;
    //}

    public void OffUI(bool isSell = false)
    {
        if (!isCheckBuildResource && !isSell)
        {
            OnWarningUI();
            return;
        }

        if (buildUIPanel.activeSelf)
            buildUIPanel.SetActive(false);

        if (resource_Desc.desc.activeSelf)
            resource_Desc.desc.SetActive(false);

        if (defenseTower_Desc.desc.activeSelf)
            defenseTower_Desc.desc.SetActive(false);

        if (castle_Desc.desc.activeSelf)
            castle_Desc.desc.SetActive(false);

        if (farm_Desc.desc.activeSelf)
            farm_Desc.desc.SetActive(false);
    }

    public void OnWarningUI()
    {
        warningUI.SetActive(true);
    }

    //강화 UI 설정
    public void SetBuildingReinforceInfo(GameObject buildingObject)
    {
        InitSetting(false);

        float curGold = ResourceManager.instance.gameResources.gameResourceInfos[(int)GameResourceType.GOLD].curResource;
        float curTree = ResourceManager.instance.gameResources.gameResourceInfos[(int)GameResourceType.TREE].curResource;
        float curStone = ResourceManager.instance.gameResources.gameResourceInfos[(int)GameResourceType.STONE].curResource;
        float curSupply = ResourceManager.instance.gameResources.gameResourceInfos[(int)GameResourceType.SUPPLY].curResource;
        float maxSupply = ResourceManager.instance.gameResources.gameResourceInfos[(int)GameResourceType.SUPPLY].maxResource;

        Button reinforceBtn = reinforceBtnObj.GetComponent<Button>();
        Button sellBtn = sellBtnObj.GetComponent<Button>();



        //if (buildingObject.tag == "Barrack" && WaveManager.instance.waveTime)
        //{
        //    reinforceBtn.interactable = false;
        //    sellBtn.interactable = false;
        //}
        //else if (buildingObject.tag == "ETC")
        //{
        //    ETCBuilding etcBuilding = buildingObject.GetComponent<ETCBuilding>();
        //    ETCBuildingData buildingData = etcBuilding.GetData();

        //    if (curSupply > maxSupply - buildingData.secondValue)
        //        sellBtn.interactable = false;
        //    else
        //        sellBtn.interactable = true;
        //}
        //else
        //{
        //    reinforceBtn.interactable = true;
        //    sellBtn.interactable = true;
        //}

        selectBuilding = buildingObject;

        switch (buildingObject.tag)
        {
            case "Resource":
                {
                    reinforceBtn.interactable = true;
                    sellBtn.interactable = true;

                    ResourceBuildingData data;

                    string key = "";

                    ResourceBuilding resourceBuilding = buildingObject.GetComponentInChildren<ResourceBuilding>();

                    key = resourceBuilding.GetKey();
                    level = resourceBuilding.GetLevel();
                    maxLevel = resourceBuilding.GetMaxLevel();
                    data = resourceBuilding.GetData();

                    ResourceBuildingData nextData = DataManager.instance.GetResourceData(key + (level + 1));

                    title.text = data.name;
                    title.color = data.fontColor;

                    resource_Desc.maxResourceAmount.text = data.maxStorage.ToString();
                    resource_Desc.secResourceAmount.text = data.output.ToString();

                    for (int i = (int)GameResourceType.GOLD; i <= (int)GameResourceType.SUPPLY; i++)
                        sub_Desc.resourcesText[i].text = data.resourceCost[i].ToString();

                    resource_Desc.desc.SetActive(true);

                    if (CheckCost(nextData))
                        isCheckBuildResource = false;
                }
                break;
            case "Barrack":
                {
                    if (WaveManager.instance.waveTime)
                    {
                        reinforceBtn.interactable = false;
                        sellBtn.interactable = false;
                    }

                    Barrack barrack = buildingObject.GetComponent<Barrack>();
                    BarrackData data = barrack.GetData();

                    level = barrack.GetLevel();
                    maxLevel = barrack.GetMaxLevel();

                    BarrackData nextData = DataManager.instance.GetBarrackData(barrack.GetKey() + (level + 1));

                    title.text = data.name;
                    title.color = data.fontColor;

                    if (data.name.Contains("검병"))
                    {
                        barrack_Desc.name.text = "검병";
                        barrack_Desc.previewImage.texture = unitImages[0];
                    }
                    else if (data.name.Contains("창병"))
                    {
                        barrack_Desc.name.text = "창병";
                        barrack_Desc.previewImage.texture = unitImages[1];
                    }
                    else if (data.name.Contains("궁병"))
                    {
                        barrack_Desc.name.text = "궁병";
                        barrack_Desc.previewImage.texture = unitImages[2];
                    }
                    else if (data.name.Contains("마법병"))
                    {
                        barrack_Desc.name.text = "마법병";
                        barrack_Desc.previewImage.texture = unitImages[3];
                    }

                    barrack_Desc.hp.text = "체력 : " + data.hp;
                    barrack_Desc.attack.text = "공격력 : " + data.attack;
                    barrack_Desc.speed.text = "이동속도 : " + data.speed;
                    barrack_Desc.supply.text = "유닛 생산수 : " + data.supply;

                    for (int i = (int)GameResourceType.GOLD; i <= (int)GameResourceType.SUPPLY; i++)
                        sub_Desc.resourcesText[i].text = data.resourceCost[i].ToString();

                    barrack_Desc.desc.SetActive(true);

                    if (CheckCost(nextData))
                        isCheckBuildResource = false;
                }
                break;
            case "DefenceTower":
                {
                    reinforceBtn.interactable = true;
                    sellBtn.interactable = true;

                    DefenceTower defenceTower = buildingObject.GetComponent<DefenceTower>();
                    DefenceTowerData data = defenceTower.GetData();

                    level = defenceTower.GetLevel();
                    maxLevel = defenceTower.GetMaxLevel();

                    DefenceTowerData nextData = DataManager.instance.GetDefenceTowerData(defenceTower.GetKey() + (level + 1));

                    title.text = data.name;
                    title.color = data.fontColor;

                    defenseTower_Desc.damage.text = "공격력 : " + data.damage.ToString();
                    defenseTower_Desc.attackSpeed.text = "공격 속도 : " + data.attackSpeed.ToString();
                    defenseTower_Desc.desc.SetActive(false);

                    for (int i = (int)GameResourceType.GOLD; i <= (int)GameResourceType.SUPPLY; i++)
                        sub_Desc.resourcesText[i].text = data.resourceCost[i].ToString();

                    defenseTower_Desc.desc.SetActive(true);

                    if (CheckCost(nextData))
                        isCheckBuildResource = false;
                }
                break;
            case "ETC":
                {
                    ETCBuilding etcBuilding = buildingObject.GetComponent<ETCBuilding>();
                    ETCBuildingData buildingData = etcBuilding.GetData();

                    if (curSupply > maxSupply - buildingData.secondValue)
                        sellBtn.interactable = false;
                    else
                        sellBtn.interactable = true;

                    ETCBuildingData data;

                    level = etcBuilding.GetLevel();
                    maxLevel = etcBuilding.GetMaxLevel();
                    data = etcBuilding.GetData();

                    ETCBuildingData nextData = DataManager.instance.GetETCData(etcBuilding.GetKey() + (level + 1));

                    title.text = data.name;
                    title.color = data.fontColor;

                    for (int i = (int)GameResourceType.GOLD; i <= (int)GameResourceType.SUPPLY; i++)
                        sub_Desc.resourcesText[i].text = data.resourceCost[i].ToString();

                    if (CheckCost(nextData))
                        isCheckBuildResource = false;

                    if (buildingObject.name.Contains("Castle"))
                    {
                        castle_Desc.gold.text = data.firstValue.ToString();
                        castle_Desc.tree.text = data.firstValue.ToString();
                        castle_Desc.stone.text = data.firstValue.ToString();
                        castle_Desc.supply.text = data.secondValue.ToString();

                        castle_Desc.desc.SetActive(true);
                    }

                    if (buildingObject.name.Contains("Farm"))
                    {
                        farm_Desc.supply.text = data.secondValue.ToString();
                        farm_Desc.desc.SetActive(true);                        
                    }

                    sub_Desc.title.text = "건설비용";
                }
                break;
        }

        if (level == maxLevel)
        {
            reinforceBtnObj.SetActive(false);
            sub_Desc.desc.SetActive(false);
            Vector3 savePos = sellBtnObj.GetComponent<Transform>().localPosition;
            savePos.x = 0.0f;
            sellBtnObj.GetComponent<Transform>().localPosition = savePos;
        }
        else
        {
            reinforceBtnObj.SetActive(true);
            sub_Desc.desc.SetActive(true);
            Vector3 savePos = sellBtnObj.GetComponent<Transform>().localPosition;
            savePos.x = 28.0f;
            sellBtnObj.GetComponent<Transform>().localPosition = savePos;
        }

        sub_Desc.title.text = "강화 비용";
        buildUIPanel.SetActive(true);
    }

    public void SellBuilding()
    {
        if (!selectBuilding)
            return;

        switch (selectBuilding.tag)
        {
            case "Resource":
                {
                    ResourceBuildingData lv1Data;
                    ResourceBuildingData data;
                    float sellGold = 0.0f;
                    float sellTree = 0.0f;
                    float sellStone = 0.0f;

                    ResourceBuilding resourceBuilding = selectBuilding.GetComponent<ResourceBuilding>();
                    lv1Data = DataManager.instance.GetResourceData(resourceBuilding.GetKey() + 1);
                    data = resourceBuilding.GetData();

                    if (selectBuilding.name.Contains("Crystal"))
                    {
                        //Crystal crystal = selectBuilding.GetComponent<Crystal>();
                        //lv1Data = DataManager.instance.GetResourceData(crystal.GetKey() + 1);
                        //data = crystal.GetData();

                        SkillManager.instance.AddMP(-(data.storageUp * resourceBuilding.GetLevel()), false);
                        //crystal.buildingPosOn();

                        //sellGold += lv1Data.resourceCost[(int)GameResourceType.GOLD] * 0.5f;
                        //sellTree += lv1Data.resourceCost[(int)GameResourceType.TREE] * 0.5f;
                        //sellStone += lv1Data.resourceCost[(int)GameResourceType.STONE] * 0.5f;

                        //if (crystal.GetLevel() != 1)
                        //{
                        //    sellGold += data.resourceCost[(int)GameResourceType.GOLD] * 0.5f;
                        //    sellTree += data.resourceCost[(int)GameResourceType.TREE] * 0.5f;
                        //    sellStone += data.resourceCost[(int)GameResourceType.STONE] * 0.5f;
                        //}

                        //GameManager.instance.save -= crystal.SaveData;
                    }
                    else
                    {
                        //GameResource resource = selectBuilding.GetComponent<GameResource>();
                        //lv1Data = DataManager.instance.GetResourceData(resource.GetKey() + 1);
                        //data = resource.GetData();

                        if (selectBuilding.tag.Contains("Sawmill"))
                            ResourceManager.instance.AddResource(GameResourceType.TREE, -(data.storageUp * resourceBuilding.GetLevel()), false);

                        if (selectBuilding.tag.Contains("Quarry"))
                            ResourceManager.instance.AddResource(GameResourceType.STONE, -(data.storageUp * resourceBuilding.GetLevel()), false);

                        //resource.buildingPosOn();

                        //sellGold += lv1Data.resourceCost[(int)GameResourceType.GOLD] * 0.5f;
                        //sellTree += lv1Data.resourceCost[(int)GameResourceType.TREE] * 0.5f;
                        //sellStone += lv1Data.resourceCost[(int)GameResourceType.STONE] * 0.5f;

                        //if (resource.GetLevel() != 1)
                        //{
                        //    sellGold += data.resourceCost[(int)GameResourceType.GOLD] * 0.5f;
                        //    sellTree += data.resourceCost[(int)GameResourceType.TREE] * 0.5f;
                        //    sellStone += data.resourceCost[(int)GameResourceType.STONE] * 0.5f;
                        //}

                        //GameManager.instance.save -= resource.SaveData;
                    }

                    resourceBuilding.buildingPosOn();

                    sellGold += lv1Data.resourceCost[(int)GameResourceType.GOLD] * 0.5f;
                    sellTree += lv1Data.resourceCost[(int)GameResourceType.TREE] * 0.5f;
                    sellStone += lv1Data.resourceCost[(int)GameResourceType.STONE] * 0.5f;

                    if (resourceBuilding.GetLevel() != 1)
                    {
                        sellGold += data.resourceCost[(int)GameResourceType.GOLD] * 0.5f;
                        sellTree += data.resourceCost[(int)GameResourceType.TREE] * 0.5f;
                        sellStone += data.resourceCost[(int)GameResourceType.STONE] * 0.5f;
                    }

                    GameManager.instance.save -= resourceBuilding.SaveData;

                    ResourceManager.instance.AddResource(GameResourceType.SUPPLY, -(data.supply));

                    //판매가 획득
                    ResourceManager.instance.AddResource(GameResourceType.GOLD, sellGold);
                    ResourceManager.instance.AddResource(GameResourceType.TREE, sellTree);
                    ResourceManager.instance.AddResource(GameResourceType.STONE, sellStone);

                    Destroy(selectBuilding);
                }
                break;
            case "Barrack":
                {
                    Barrack barrack = selectBuilding.GetComponent<Barrack>();
                    BarrackData lv1Data = DataManager.instance.GetBarrackData(barrack.GetKey() + 1);
                    BarrackData data;

                    float sellGold = lv1Data.resourceCost[(int)GameResourceType.GOLD] * 0.5f;
                    float sellTree = lv1Data.resourceCost[(int)GameResourceType.TREE] * 0.5f;
                    float sellStone = lv1Data.resourceCost[(int)GameResourceType.STONE] * 0.5f;

                    if (barrack.GetLevel() != 1)
                    {
                        data = barrack.GetData();
                        sellGold += data.resourceCost[(int)GameResourceType.GOLD] * 0.5f;
                        sellTree += data.resourceCost[(int)GameResourceType.TREE] * 0.5f;
                        sellStone += data.resourceCost[(int)GameResourceType.STONE] * 0.5f;
                    }
                    else
                    {
                        data = lv1Data;
                    }

                    GameManager.instance.save -= barrack.SaveData;

                    UnitManager.instance.RemoveUnit(new UnitKey(data.type, barrack.GetLevel()), data.supply);

                    ResourceManager.instance.AddResource(GameResourceType.GOLD, sellGold);
                    ResourceManager.instance.AddResource(GameResourceType.TREE, sellTree);
                    ResourceManager.instance.AddResource(GameResourceType.STONE, sellStone);
                    ResourceManager.instance.AddResource(GameResourceType.SUPPLY, -(data.supply));

                    barrack.buildingPosOn();
                    Destroy(selectBuilding);
                }
                break;
            case "DefenceTower":
                {
                    DefenceTower tower = selectBuilding.GetComponent<DefenceTower>();
                    DefenceTowerData lv1Data = DataManager.instance.GetDefenceTowerData(tower.GetKey() + 1);
                    DefenceTowerData data;

                    float sellGold = lv1Data.resourceCost[(int)GameResourceType.GOLD] * 0.5f;
                    float sellTree = lv1Data.resourceCost[(int)GameResourceType.TREE] * 0.5f;
                    float sellStone = lv1Data.resourceCost[(int)GameResourceType.STONE] * 0.5f;

                    if (tower.GetLevel() != 1)
                    {
                        data = tower.GetData();
                        sellGold += data.resourceCost[(int)GameResourceType.GOLD] * 0.5f;
                        sellTree += data.resourceCost[(int)GameResourceType.TREE] * 0.5f;
                        sellStone += data.resourceCost[(int)GameResourceType.STONE] * 0.5f;
                    }
                    else
                    {
                        data = lv1Data;
                    }

                    GameManager.instance.save -= tower.SaveData;

                    ResourceManager.instance.AddResource(GameResourceType.GOLD, sellGold);
                    ResourceManager.instance.AddResource(GameResourceType.TREE, sellTree);
                    ResourceManager.instance.AddResource(GameResourceType.STONE, sellStone);
                    ResourceManager.instance.AddResource(GameResourceType.SUPPLY, -(data.supply));

                    tower.buildingPosOn();

                    Destroy(selectBuilding);
                }
                break;
            case "ETC":
                {
                    ETCBuilding etcBuilding = selectBuilding.GetComponent<ETCBuilding>();

                    ETCBuildingData lv1Data = DataManager.instance.GetETCData(etcBuilding.GetKey() + 1);
                    ETCBuildingData data;

                    float sellGold = lv1Data.resourceCost[(int)GameResourceType.GOLD] * 0.5f;
                    float sellTree = lv1Data.resourceCost[(int)GameResourceType.TREE] * 0.5f;
                    float sellStone = lv1Data.resourceCost[(int)GameResourceType.STONE] * 0.5f;

                    etcBuilding.buildingPosOn();

                    if (etcBuilding.GetLevel() != 1)
                    {
                        data = etcBuilding.GetData();
                        sellGold += data.resourceCost[(int)GameResourceType.GOLD];
                        sellTree += data.resourceCost[(int)GameResourceType.TREE];
                        sellStone += data.resourceCost[(int)GameResourceType.STONE];
                    }
                    else
                    {
                        data = lv1Data;
                    }

                    GameManager.instance.save -= etcBuilding.SaveData;

                    ResourceManager.instance.AddResource(GameResourceType.GOLD, sellGold);
                    ResourceManager.instance.AddResource(GameResourceType.TREE, sellTree);
                    ResourceManager.instance.AddResource(GameResourceType.STONE, sellStone);
                    ResourceManager.instance.AddResource(GameResourceType.SUPPLY, -(data.secondValue), false);

                    //if (selectBuilding.name.Contains("Farm"))
                    //{
                    //    Farm farm = selectBuilding.GetComponent<Farm>();
                    //    ETCBuildingData lv1Data = DataManager.instance.GetETCData(farm.GetKey() + 1);
                    //    ETCBuildingData data;

                    //    float sellGold = lv1Data.resourceCost[(int)GameResourceType.GOLD] * 0.5f;
                    //    float sellTree = lv1Data.resourceCost[(int)GameResourceType.TREE] * 0.5f;
                    //    float sellStone = lv1Data.resourceCost[(int)GameResourceType.STONE] * 0.5f;

                    //    farm.buildingPosOn();

                    //    if (farm.GetLevel() != 1)
                    //    {
                    //        data = farm.GetData();
                    //        sellGold += data.resourceCost[(int)GameResourceType.GOLD];
                    //        sellTree += data.resourceCost[(int)GameResourceType.TREE];
                    //        sellStone += data.resourceCost[(int)GameResourceType.STONE];
                    //    }
                    //    else
                    //    {
                    //        data = lv1Data;
                    //    }

                    //    GameManager.instance.save -= farm.SaveData;

                    //    ResourceManager.instance.AddResource(GameResourceType.GOLD, sellGold);
                    //    ResourceManager.instance.AddResource(GameResourceType.TREE, sellTree);
                    //    ResourceManager.instance.AddResource(GameResourceType.STONE, sellStone);

                    //    ResourceManager.instance.AddResource(GameResourceType.SUPPLY, -(data.secondValue), false);

                    //}

                    //if (selectBuilding.name.Contains("Castle"))
                    //{
                    //    Castle castle = selectBuilding.GetComponent<Castle>();
                    //    ETCBuildingData lv1Data = DataManager.instance.GetETCData(castle.GetKey() + 1);
                    //    ETCBuildingData data;

                    //    float sellGold = lv1Data.resourceCost[(int)GameResourceType.GOLD] * 0.5f;
                    //    float sellTree = lv1Data.resourceCost[(int)GameResourceType.TREE] * 0.5f;
                    //    float sellStone = lv1Data.resourceCost[(int)GameResourceType.STONE] * 0.5f;

                    //    castle.buildingPosOn();

                    //    if (castle.GetLevel() != 1)
                    //    {
                    //        data = castle.GetData();
                    //        sellGold += data.resourceCost[(int)GameResourceType.GOLD];
                    //        sellTree += data.resourceCost[(int)GameResourceType.TREE];
                    //        sellStone += data.resourceCost[(int)GameResourceType.STONE];
                    //    }
                    //    else
                    //    {
                    //        data = lv1Data;
                    //    }

                    //    GameManager.instance.save -= castle.SaveData;

                    //    ResourceManager.instance.AddResource(GameResourceType.GOLD, sellGold);
                    //    ResourceManager.instance.AddResource(GameResourceType.TREE, sellTree);
                    //    ResourceManager.instance.AddResource(GameResourceType.STONE, sellStone);

                    //    ResourceManager.instance.AddResource(GameResourceType.GOLD, -data.firstValue, false);
                    //    ResourceManager.instance.AddResource(GameResourceType.TREE, -data.firstValue, false);
                    //    ResourceManager.instance.AddResource(GameResourceType.STONE, -data.firstValue, false);
                    //    ResourceManager.instance.AddResource(GameResourceType.SUPPLY, -(data.secondValue), false);
                    //}
                    Destroy(selectBuilding);
                }
                break;
        }

        OffUI(true);
    }

    public void ReinforceBuilding()
    {
        selectBuilding.GetComponent<BaseBuilding>().Reinforce();

        //switch (selectBuilding.tag)
        //{
        //    case "Resource":
        //        {
        //            selectBuilding.GetComponent<ResourceBuilding>().Reinforce()
        //            if (selectBuilding.name.Contains("Crystal"))
        //                selectBuilding.GetComponent<Crystal>().Reinforce();
        //            else
        //                selectBuilding.GetComponent<GameResource>().Reinforce();
        //        }
        //        break;
        //    case "Barrack":
        //        {
        //            selectBuilding.GetComponent<Barrack>().Reinforce();
        //        }
        //        break;
        //    case "DefenceTower":
        //        {
        //            selectBuilding.GetComponent<DefenceTower>().Reinforce();
        //        }
        //        break;
        //    case "ETC":
        //        {
        //            if (selectBuilding.name.Contains("Castle"))
        //                selectBuilding.GetComponent<Castle>().Reinforce();

        //            if (selectBuilding.name.Contains("Farm"))
        //                selectBuilding.GetComponent<Farm>().Reinforce();
        //        }
        //        break;
        //}
    }

    private void InitSetting(bool isBuild = false)
    {
        isCheckBuildResource = true;

        if (resource_Desc.desc.activeSelf)
            resource_Desc.desc.SetActive(false);

        if (castle_Desc.desc.activeSelf)
            castle_Desc.desc.SetActive(false);

        if (barrack_Desc.desc.activeSelf)
            barrack_Desc.desc.SetActive(false);

        if (farm_Desc.desc.activeSelf)
            farm_Desc.desc.SetActive(false);

        if (isBuild)
        {
            if (reinforceBtnObj.activeSelf)
                reinforceBtnObj.SetActive(false);

            if (sellBtnObj.activeSelf)
                sellBtnObj.SetActive(false);

            if (!buildBtn.activeSelf)
                buildBtn.SetActive(true);

            if (!sub_Desc.desc.activeSelf)
                sub_Desc.desc.SetActive(true);
        }
        else
        {
            if (!reinforceBtnObj.activeSelf)
                reinforceBtnObj.SetActive(true);

            if (!sellBtnObj.activeSelf)
                sellBtnObj.SetActive(true);

            if (buildBtn.activeSelf)
                buildBtn.SetActive(false);
        }
    }
}
