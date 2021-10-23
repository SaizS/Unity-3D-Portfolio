using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResource : ResourceBuilding
{
    private GameObject gameResource;

    public override void InitData(string value, GameObject targetPos, bool isLoad, int levelValue)
    {
        base.InitData(value, targetPos, isLoad, levelValue);

        ResourceManager.instance.AddResource(buildingData.type, buildingData.storageUp * level, false);
        StartCoroutine(IncreaceResource());
    }

    public override void Reinforce()
    {
        base.Reinforce();
    }

    private IEnumerator IncreaceResource()
    {
        while (true)
        {
            if (buildingData.maxStorage <= buildingData.curStorage)
            {
                yield return new WaitForSeconds(1.0f);
            }
            else
            {
                buildingData.curStorage += buildingData.output;
                yield return new WaitForSeconds(1.0f);
            }

        }
    }

    public void CollectResource()
    {
        float maxStorage = ResourceManager.instance.gameResources.gameResourceInfos[(int)buildingData.type].maxResource;
        float curStorage = ResourceManager.instance.gameResources.gameResourceInfos[(int)buildingData.type].curResource;

        if (maxStorage <= curStorage + buildingData.curStorage)
        {
            float gap = maxStorage - curStorage;
            buildingData.curStorage -= gap;
            ResourceManager.instance.AddResource(buildingData.type, gap);
            print(buildingData.curStorage);
        }
        else
        {
            ResourceManager.instance.AddResource(buildingData.type, buildingData.curStorage);
            buildingData.curStorage = 0;
        }
    }
}
