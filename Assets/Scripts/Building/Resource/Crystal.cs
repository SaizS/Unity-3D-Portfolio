using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : ResourceBuilding
{
    private SkillManager skillManager;

    public override void InitData(GameObject targetPos, bool isLoad, int levelValue)
    {
        key = "Resource_Crystal_Lv";
        base.InitData(targetPos, isLoad, levelValue);

        skillManager = GameObject.Find("Skills").GetComponent<SkillManager>();
        skillManager.mp.secPoint = buildingData.output;
        SkillManager.instance.AddMP(buildingData.storageUp, false);

        StartCoroutine(skillManager.IncreaseMP());
    }

    public override void Reinforce()
    {
        base.Reinforce();

        SkillManager.instance.AddMP(buildingData.storageUp, false);
    }
}
