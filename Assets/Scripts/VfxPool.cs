using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Search;
using UnityEngine;

[Serializable]
public enum VfxList
{
    GroundSlash,
    GroundSlash_Triple,
    Fireball,
    FireExplosion,
    Lightning
}

[Serializable]
public enum VfxType
{
    UnlimitedRange,
    LimitedRange,
    Melee,
    Other
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
        var canDeq = Pool[key].TryDequeue(out GameObject theVfx);
        if (!canDeq)
        {
            vfxPrefab thePrefab = pool.First(x => x.name == key);
            theVfx = Instantiate(thePrefab.prefab, transform);
            PooledObject obj = theVfx.AddComponent<PooledObject>();
            obj.pool = this;
            obj.thisVfx = thePrefab.name;
            obj.type = thePrefab.type;
        }
        theVfx.transform.SetPositionAndRotation(pos, rot);
        theVfx.SetActive(true);
        return theVfx;
    }

    public void ReturnVfx(PooledObject vfx)
    {
        // Destroy if pool is full, otherwise just queue it back
        //if (Pool[vfx.thisVfx].Count >= pool.First(x => x.name == vfx.thisVfx).count)
        //{
        //    Destroy(vfx.gameObject);
        //    return;
        //}

        vfx.gameObject.SetActive(false);
        Pool[vfx.thisVfx].Enqueue(vfx.gameObject);

    }
}
