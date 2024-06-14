using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    public VfxPool pool;
    public VfxList thisVfx;
    public VfxType type;

    public void Return()
    {
        pool.ReturnVfx(this);
    }
}
