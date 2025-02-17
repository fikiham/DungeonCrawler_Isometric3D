using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxChildren : MonoBehaviour
{
    public bool hasSetup = false;

    public virtual void OnSpawn()
    {
        hasSetup = false;
    }

    public virtual void Setup()
    {
        hasSetup = true;
    }
}
