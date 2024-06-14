using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum VfxList
{
    GroundSlash,
    Fireball,
    FireExplosion
}

[Serializable]
public enum VfxType
{
    UnlimitedRange,
    LimitedRange,
    Melee,
    TrailThenExplosion
}

public class VfxPool : MonoBehaviour
{
    public static VfxPool instance;

    [Serializable]
    class vfxPrefab
    {
        public VfxList name;
        public VfxType type;
        public int count;
        public GameObject prefab;
    }
    [SerializeField] List<vfxPrefab> pool;
    [HideInInspector] public Dictionary<VfxList, Queue<GameObject>> Pool = new();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        foreach (var vfx in pool)
        {
            Queue<GameObject> queue = new();
            for (int i = 0; i < vfx.count; i++)
            {
                var theVfx = Instantiate(vfx.prefab, transform);
                PooledObject obj = theVfx.AddComponent<PooledObject>();
                obj.pool = this;
                obj.thisVfx = vfx.name;
                obj.type = vfx.type;
                theVfx.SetActive(false);
                queue.Enqueue(theVfx);
            }
            Pool.Add(vfx.name, queue);
        }

    }

    public GameObject GetVfx(VfxList key, Vector3 pos, Quaternion rot)
    {
        if (!Pool[key].TryDequeue(out GameObject theVfx))
            return null;
        theVfx.transform.SetPositionAndRotation(pos, rot);
        theVfx.SetActive(true);
        return theVfx;
    }

    public void ReturnVfx(PooledObject vfx)
    {
        vfx.gameObject.SetActive(false);
        Pool[vfx.thisVfx].Enqueue(vfx.gameObject);
    }
}
