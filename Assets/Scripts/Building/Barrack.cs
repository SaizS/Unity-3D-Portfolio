using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrack : BaseBuilding
{
    private BarrackData buildingData;

    public override void InitData(string value, GameObject targetPos, bool isLoad, int levelValue)
    {
        base.InitData(value, targetPos, isLoad, levelValue);
        UnitManager.instance.AddUnit(new UnitKey(buildingData.type, level), buildingData.supply);
    }

    public override void Reinforce()
    {
        base.Reinforce();
        UnitManager.instance.AddUnit(new UnitKey(buildingData.type, level), buildingData.supply);
    }

    public BarrackData GetData()
    {
        return buildingData;
    }

    public override void SetData()
    {
        buildingData = DataManager.instance.GetBarrackData(Util.CombineString(key + level.ToString()));
        commonData = buildingData;
    }

    public override BuildingCommonData GetCommonData()
    {
        return DataManager.instance.GetBarrackData(Util.CombineString(key, level.ToString()));
    }
}
