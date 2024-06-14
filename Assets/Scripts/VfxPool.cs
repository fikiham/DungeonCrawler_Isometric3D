using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum VfxList
{
    GroundSlash
}

public class VfxPool : MonoBehaviour
{
    public static VfxPool instance;

    [Serializable]
    class vfxPrefab
    {
        public VfxList name;
        public GameObject prefab;
    }
    [SerializeField] List<vfxPrefab> pool;
    [HideInInspector] public Dictionary<VfxList, GameObject> Pool = new();

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
            var theVfx = Instantiate(vfx.prefab, transform);
            theVfx.SetActive(false);
            Pool.Add(vfx.name, theVfx);
        }

    }

    public GameObject GetVfx(VfxList key, Vector3 pos, Quaternion rot)
    {
        GameObject theVfx = Pool[key];
        if (theVfx.activeInHierarchy)
            return null;
        theVfx.transform.SetPositionAndRotation(pos, rot);
        theVfx.SetActive(true);
        return theVfx;
    }

    public void ReturnVfx(GameObject vfx)
    {
        vfx.SetActive(false);
    }
}
