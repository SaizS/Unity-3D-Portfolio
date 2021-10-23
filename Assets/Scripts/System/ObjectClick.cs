using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectClick : MonoBehaviour
{
    private GameObject target = null;
    private Transform buildType;
    private ResourceManager resourceManager;
    private Tower_Resource resourceCheck;
    private Build buildSystem;
    private string[] buildName;
    private Vector3 posDown;
    private Vector3 posUp;

    void Start()
    {
        resourceManager = GetComponent<ResourceManager>();
        buildSystem = GetComponent<Build>();
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.touchCount != 0)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                return;
            }
        }

        if (Input.GetMouseButtonDown(0))
            posDown = Camera.main.transform.position;
        if (Input.GetMouseButtonUp(0))
        {
            posUp = Camera.main.transform.position;

            float temp = Vector3.Distance(posDown, posUp);
            if(temp < 0.1f)
            target = GetClick();

            if (target == null)
            {
                return;
            }
            else if (target.name.Substring(0, 8) == "BuildPos")
            {
                buildSystem.BuildObject(target);
                target = null;
            }
            else if (Build.instance.isBuild)
                return;
            else if (target.tag == "Resource")
            {
                if (!GameManager.instance.isTown)
                    return;

                if (!target.name.Contains("Crystal"))
                {
                    float curStorage = target.GetComponent<GameResource>().GetData().curStorage;

                    if (curStorage <= target.GetComponent<GameResource>().GetData().maxStorage * 0.3f)
                    {
                        BuildUI.instance.SetBuildingReinforceInfo(target);
                    }

                    target.GetComponent<GameResource>().CollectResource();

                    if (curStorage == target.GetComponent<GameResource>().GetData().curStorage)
                    {
                        BuildUI.instance.SetBuildingReinforceInfo(target);
                    }
                }
                else
                    BuildUI.instance.SetBuildingReinforceInfo(target);
            }
            else if (target.tag == "DefenceTower")
            {
                if (GameManager.instance.isTown)
                    return;
                else
                    BuildUI.instance.SetBuildingReinforceInfo(target);
            }
            else
            {
                if (!GameManager.instance.isTown)
                    return;

                if (target.tag != "Barrack" && target.tag != "ETC")
                    return;

                BuildUI.instance.SetBuildingReinforceInfo(target);
            }
        }

    }
    public void TargetCancel()
    {
        target = null;
    }
    private GameObject GetClick() // 클릭된 대상 받는 함수
    {
        RaycastHit hit;

        Vector3 camera = Camera.main.transform.position;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction, out hit))
        {
            target = hit.transform.gameObject;
        }
        else
        {
            target = null;
        }
        return target;
    }

    private void ResourceMove()
    {
        resourceCheck = target.GetComponent<Tower_Resource>();
        resourceManager.AddResource(resourceCheck.maker, resourceCheck.curStorage);
        resourceCheck.curStorage = 0;
    }
}
