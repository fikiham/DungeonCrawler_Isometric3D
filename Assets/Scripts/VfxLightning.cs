using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxLightning : MonoBehaviour
{
    public Vector3 Direction;
    public float Range;
    public int Count;
    public float FinishTime;

    private void OnEnable()
    {
        StartCoroutine(RecursiveLightning());
    }

    IEnumerator RecursiveLightning()
    {
        yield return new WaitForSeconds(.1f);
        yield return new WaitForSeconds(FinishTime / Count);
        if (Count != 1)
        {
            var targetPos = transform.position + (Direction.normalized * Range / Count);
            Count--;
            var theVfx = VfxPool.instance.GetVfx(VfxList.Lightning, targetPos, Quaternion.identity);
            theVfx.GetComponent<VfxLightning>().SetupLightning(Direction, Range, Count, FinishTime);
        }

        yield return new WaitForSeconds(1);
        Debug.Log("RETURNED");
        GetComponent<PooledObject>().Return();
    }

    public void SetupLightning(Vector2 dir, float range, int count, float finishTime)
    {
        Direction = dir;
        Range = range;
        Count = count;
        FinishTime = finishTime;
    }
}
