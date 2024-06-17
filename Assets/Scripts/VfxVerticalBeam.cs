using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxVerticalBeam : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(WaitToReturn());
    }

    IEnumerator WaitToReturn()
    {
        yield return new WaitForSeconds(2);
        GetComponent<PooledObject>().Return();
    }
}
