using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : ETCBuilding
{
    public override void InitData(GameObject targetPos, bool isLoad, int levelValue)
    {
        key = "ETC_Farm_Lv";
        base.InitData(targetPos, isLoad, levelValue);
    }
}
