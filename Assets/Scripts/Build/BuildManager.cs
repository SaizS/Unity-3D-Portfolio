using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    public GameObject buildList; 
    public GameObject TowerList; 
    public GameObject buildPosList;

    private BuildPosChecker[] buildPos;
    
    private void Start()
    {
        buildPos = buildPosList.transform.GetComponentsInChildren<BuildPosChecker>();

        buildList.SetActive(false);
        TowerList.SetActive(false);
    }

    public void OnBuildBtn()
    {
        if (buildList.gameObject.activeSelf || TowerList.gameObject.activeSelf)
        {
            buildList.SetActive(false);
            TowerList.SetActive(false);

            for (int i = 0; i < buildPos.Length; i++)
                buildPos[i].transform.gameObject.SetActive(false);
        }
        else
        {
            if(GameManager.instance.isTown)
                buildList.SetActive(true);
            else
                TowerList.SetActive(true);
        }

        Build.instance.isBuild = false;
    }
    
    public void SelectBuild()
    {
        if(!BuildUI.instance.isCheckBuildResource)
        {
            Debug.Log("자원 부족");
            BuildUI.instance.OnWarningUI();
            return;
        }

        Build.instance.isBuild = true;

        for (int i = 0; i < buildPos.Length; i++)
        {
            if (buildPos[i].onBuild)
                continue;
            else
            {
                if(i > 39 && !GameManager.instance.isTown)
                    buildPos[i].BuildMod();
                if(i < 40 && GameManager.instance.isTown)
                   buildPos[i].BuildMod();

            }
        }
    }
}
