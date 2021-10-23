using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBuilding : MonoBehaviour
{
    protected readonly int maxLevel = 5;
    protected int level = 1;

    protected string key;    
    protected GameObject buildingPos;

    public BuildingCommonData commonData;

    public virtual void InitData(string value, GameObject targetPos, bool isLoad, int levelValue)
    {
        key = value;
        InitData(targetPos, isLoad, levelValue);
    }
    public virtual void InitData(GameObject targetPos, bool isLoad, int levelValue)
    {
        level = levelValue;
        buildingPos = targetPos;
        SetData();

        if (!isLoad)
        {
            for (int i = (int)GameResourceType.GOLD; i <= (int)GameResourceType.SUPPLY; i++)
            {
                if (i != (int)GameResourceType.SUPPLY)
                    ResourceManager.instance.AddResource((GameResourceType)i, -commonData.resourceCost[i]);
                else
                    ResourceManager.instance.AddResource((GameResourceType)i, commonData.resourceCost[i]);
            }
        }

        GameManager.instance.save += SaveData;
    }

   

    public virtual void Reinforce()
    {
        BuildUI.instance.OffUI();

        if (!BuildUI.instance.isCheckBuildResource)
            return;

        level++;
        SetData();

        for (int i = (int)GameResourceType.GOLD; i <= (int)GameResourceType.SUPPLY; i++)
        {
            if (i != (int)GameResourceType.SUPPLY)
                ResourceManager.instance.AddResource((GameResourceType)i, -commonData.resourceCost[i]);
            else
                ResourceManager.instance.AddResource((GameResourceType)i, commonData.resourceCost[i]);
        }
    }

    public abstract void SetData();
    public abstract BuildingCommonData GetCommonData();

    public string GetKey()
    {
        return key;
    }

    public int GetLevel()
    {
        return level;
    }

    public int GetMaxLevel()
    {
        return maxLevel;
    }

    public void buildingPosOn()
    {
        buildingPos.GetComponent<BuildPosChecker>().CheckBuild(false);
    }

    public void SaveData()
    {
        GameManager.instance.SaveBuildingData(key, buildingPos.name, level);
    }

    public void OnDestroy()
    {
        GameManager.instance.save -= SaveData;
    }
}

public abstract class ETCBuilding : BaseBuilding
{
    protected ETCBuildingData buildingData;

    public override void InitData(GameObject targetPos, bool isLoad, int levelValue)
    {
        base.InitData(targetPos, isLoad, levelValue);
        ResourceManager.instance.AddResource(GameResourceType.SUPPLY, buildingData.secondValue, false);
    }

    public override BuildingCommonData GetCommonData()
    {
        return DataManager.instance.GetETCData(Util.CombineString(key, level.ToString()));
    }

    public override void SetData()
    {
        buildingData = DataManager.instance.GetETCData(Util.CombineString(key, level.ToString()));
        commonData = buildingData;
    }

    public ETCBuildingData GetData()
    {
        return buildingData;
    }

    public override void Reinforce()
    {
        base.Reinforce();
        ResourceManager.instance.AddResource(GameResourceType.SUPPLY, buildingData.secondValueIncreace, false);
    }
}

public abstract class ResourceBuilding : BaseBuilding
{
    protected ResourceBuildingData buildingData;

    public ResourceBuildingData GetData()
    {
        return buildingData;
    }

    public override void SetData()
    {
        buildingData = DataManager.instance.GetResourceData(Util.CombineString(key, level.ToString()));
        commonData = buildingData;
    }

    public override BuildingCommonData GetCommonData()
    {
        return DataManager.instance.GetResourceData(Util.CombineString(key, level.ToString()));
    }

}