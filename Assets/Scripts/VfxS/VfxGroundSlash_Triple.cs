using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using static UnityEngine.GraphicsBuffer;

public class VfxGroundSlash_Triple : VfxChildren
{
    public bool clone = false;

    private void OnEnable()
    {
        OnSpawn();
        StartCoroutine(WaitSetup());
    }

    private new void OnSpawn()
    {
        base.OnSpawn();
        clone = false;
    }

    IEnumerator WaitSetup()
    {
        while (!hasSetup)
        {
            yield return null;
        }

        if (clone)
            StartCoroutine(CloneLogic());
        else
            StartCoroutine(SpawningTwoMore());
    }

    IEnumerator SpawningTwoMore()
    {
        GetComponentInChildren<VisualEffect>().SetFloat("Size", 2);

        var rotation1 = transform.eulerAngles - new Vector3(0, -15, 0);
        var rotation2 = transform.eulerAngles - new Vector3(0, 15, 0);

        var slash1 = VfxPool.instance.GetVfx(VfxList.GroundSlash_Triple, transform.position, Quaternion.Euler(rotation1));
        slash1.GetComponentInChildren<VisualEffect>().SetFloat("Size", 1);
        slash1.GetComponent<VfxGroundSlash_Triple>().Setup(true);

        var slash2 = VfxPool.instance.GetVfx(VfxList.GroundSlash_Triple, transform.position, Quaternion.Euler(rotation2));
        slash2.GetComponentInChildren<VisualEffect>().SetFloat("Size", 1);
        slash2.GetComponent<VfxGroundSlash_Triple>().Setup(true);
        yield return null;
    }

    public void Setup(bool clone, float vfxLifetime = 2)
    {
        this.clone = clone;
        base.Setup();
    }
    IEnumerator CloneLogic()
    {
        float vfxTimer = 0;
        float vfxLerp = 0;
        while (true)
        {
            vfxTimer += Time.deltaTime;
            vfxLerp = vfxTimer;
            GetComponent<Rigidbody>().velocity = Vector3.Lerp(transform.forward * 20, Vector3.zero, vfxLerp);
            if (vfxTimer > 1)
            {
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(2);
        GetComponent<PooledObject>().Return();
    }
}
