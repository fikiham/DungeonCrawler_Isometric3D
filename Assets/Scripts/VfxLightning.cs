using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class VfxLightning : MonoBehaviour
{
    public Vector3 Direction;
    public float Range;
    public int Count;
    public float FinishTime;
    public int CurrentCount;

    private void OnEnable()
    {
        StartCoroutine(RecursiveLightning());
    }

    IEnumerator RecursiveLightning()
    {
        // Wait for setup code to work
        yield return new WaitForSeconds(.1f);
        yield return new WaitForSeconds(FinishTime / Count);
        if (CurrentCount > 1)
        {
            var targetPos = transform.position + (Direction.normalized * Range / Count);
            CurrentCount--;
            var theVfx = VfxPool.instance.GetVfx(VfxList.Lightning, targetPos, Quaternion.identity);
            theVfx.GetComponent<VfxLightning>().SetupLightning(Direction, Range, Count, FinishTime, CurrentCount);
        }

        yield return new WaitForSeconds(1);
        GetComponent<PooledObject>().Return();
    }

    public void SetupLightning(Vector3 dir, float range, int count, float finishTime, int current = 0)
    {
        Direction = dir;
        Range = range;
        Count = count;
        FinishTime = finishTime;
        CurrentCount = current == 0 ? count : current;
    }
}
