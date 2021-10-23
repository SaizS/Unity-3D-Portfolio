using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceTower : BaseBuilding
{
    private DefenceTowerData defenceTowerData;
    private Unit target;
    private Transform targetTr;
    private Animator animator;
    public Transform firePos;
    DefenceTowerData buildingData;

    public override void InitData(string value, GameObject targetPos, bool isLoad, int levelValue) //string을 키로 하여 유닛의 정보를 입력 
    {
        base.InitData(value, targetPos, isLoad, levelValue);

        animator = GetComponent<Animator>();
        animator.Play("idle");
        StartCoroutine(SetTarget());
    }

    public override void Reinforce()
    {
        base.Reinforce();
    }

    private IEnumerator SetTarget()
    { //포탑의 공격 대상 추적 코루틴. 가장 가까운 적을 대상으로 함.
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (target == null && targetTr == null || target.isDie)
            { //유닛의 목표 지정 알고리즘. 타겟이 없을 경우에만 작동
                Collider[] colls = Physics.OverlapSphere(transform.position, 30.0f, 1 << 10); //반경  안의 충돌체 검사
                float maxDis = 30.0f;
                foreach (Collider coll in colls)
                {
                    if (coll.GetType() != typeof(CapsuleCollider))
                        continue;
                    if (coll.tag != "Unit")
                        continue;
                    Unit temptarget = coll.GetComponent<Unit>();
                    if (temptarget.isDie || temptarget == null)
                        continue;
                    float dis = Vector3.Distance(transform.position, coll.GetComponent<Transform>().position);
                    if (dis < maxDis && dis != 0.0f)
                    {
                        maxDis = dis;
                        target = coll.GetComponent<Unit>();
                        targetTr = coll.GetComponent<Transform>();
                        continue;
                    }
                }
                if (target == null)
                    continue;
                if (target.isDie)
                { //주변에 적이 없는 경우 비전투 상황으로 변경
                    animator.Play("idle");
                    continue;
                }
            }
            if (target != null && targetTr != null && WaveManager.instance.waveTime)
            {
                if (Vector3.Distance(targetTr.position, transform.position) > 30.0f)
                { //사거리 밖으로 벗어나면 공격 대상을 재탐색
                    target = null;
                    targetTr = null;
                    continue;
                }
                for (int i = 0; i < defenceTowerData.attackCount; i++)
                {
                    animator.Play("shot");
                    switch (gameObject.name)
                    {
                        case "Defence_Archer(Clone)":
                            {
                                firePos.LookAt(targetTr);
                                firePos.Rotate(-2, 0, 0);
                                SoundManager.instance.PlayFx(FxClip.ARROW_SHOT, this.gameObject.transform.position, 0.6f);
                                ProjectileManager.instance.Fire(firePos, "Tower_Arrow", defenceTowerData.damage, 9, 3000);
                                yield return new WaitForSeconds(0.3f);
                            }
                            break;
                        case "Defence_HI_Archer(Clone)":
                            {
                                firePos.LookAt(targetTr);
                                firePos.Rotate(-2, 0, 0);
                                SoundManager.instance.PlayFx(FxClip.ARROW_SHOT, this.gameObject.transform.position, 0.6f);
                                ProjectileManager.instance.Fire(firePos, "Tower_Arrow", defenceTowerData.damage, 9, 3000);
                                yield return new WaitForSeconds(0.3f);
                            }
                            break;
                        case "Defence_Cannon(Clone)":
                            {
                                SoundManager.instance.PlayFx(FxClip.METEOR_HIT, this.gameObject.transform.position, 0.6f);
                                ParticleManager.instance.Play("FX_Burst_Cannon", new Vector3(transform.position.x, transform.position.y + 3.5f, transform.position.z), transform.rotation);
                                yield return new WaitForSeconds(1.0f);
                                ProjectileManager.instance.Drop(targetTr, 50.0f, "Tower_Cannon", defenceTowerData.damage, 9, 8000);
                                yield return new WaitForSeconds(0.5f);
                            }
                            break;
                        case "Defence_HI_Cannon(Clone)":
                            {
                                SoundManager.instance.PlayFx(FxClip.METEOR_HIT, this.gameObject.transform.position, 0.6f);
                                ParticleManager.instance.Play("FX_Burst_Cannon", new Vector3(transform.position.x, transform.position.y + 7.0f, transform.position.z), transform.rotation);
                                yield return new WaitForSeconds(1.0f);
                                ProjectileManager.instance.Drop(targetTr, 50.0f, "Tower_Cannon", defenceTowerData.damage, 9, 8000);
                                yield return new WaitForSeconds(0.5f);
                            }
                            break;
                        default:
                            break;
                    }
                }
                yield return new WaitForSeconds(defenceTowerData.attackSpeed);
            }
        }
    }

    public DefenceTowerData GetData()
    {
        return defenceTowerData;
    }

    public override void SetData()
    {
        buildingData = DataManager.instance.GetDefenceTowerData(Util.CombineString(key, level.ToString()));
        commonData = buildingData;
    }

    public override BuildingCommonData GetCommonData()
    {
        return DataManager.instance.GetDefenceTowerData(Util.CombineString(key, level.ToString()));
    }
}
