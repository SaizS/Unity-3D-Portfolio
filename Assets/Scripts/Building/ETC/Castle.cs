using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : ETCBuilding
{
    public override void InitData(GameObject targetPos, bool isLoad, int levelValue) 
    {
        key = "ETC_Castle_Lv";
        base.InitData(targetPos, isLoad, levelValue);
    }
}
