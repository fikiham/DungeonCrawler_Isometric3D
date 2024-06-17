using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class VfxLightning : VfxChildren
{
    public Vector3 Direction;
    public float Range;
    public int Count;
    public float FinishTime;
    public int CurrentCount;

    private void OnEnable()
    {
        base.OnSpawn();
        StartCoroutine(RecursiveLightning());
    }

    IEnumerator RecursiveLightning()
    {
        // Wait for setup code
        while (!hasSetup)
        {
            yield return null;
        }

        yield return new WaitForSeconds(FinishTime / Count);
        if (CurrentCount > 1)
        {
            var targetPos = transform.position + (Direction.normalized * Range / Count);
            CurrentCount--;
            var theVfx = VfxPool.instance.GetVfx(VfxList.Lightning, targetPos, Quaternion.identity);
            theVfx.GetComponent<VfxLightning>().Setup(Direction, Range, Count, FinishTime, CurrentCount);
        }

        yield return new WaitForSeconds(1);
        GetComponent<PooledObject>().Return();
    }

    public void Setup(Vector3 dir, float range, int count, float finishTime, int current = 0)
    {
        Direction = dir;
        Range = range;
        Count = count;
        FinishTime = finishTime;
        CurrentCount = current == 0 ? count : current;
        base.Setup();
    }
}
