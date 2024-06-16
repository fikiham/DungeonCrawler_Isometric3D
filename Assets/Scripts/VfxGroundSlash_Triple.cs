using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using static UnityEngine.GraphicsBuffer;

public class VfxGroundSlash_Triple : MonoBehaviour
{
    public bool clone = false;
    bool hasSetup = false;
    private void OnEnable()
    {
        if (clone)
            StartCoroutine(CloneLogic());
        else
            StartCoroutine(SpawningTwoMore());
    }

    IEnumerator SpawningTwoMore()
    {
        var rotation1 = transform.eulerAngles - new Vector3(0,-45,0);
        var rotation2 = transform.eulerAngles - new Vector3(0,45,0);

        var slash1 = VfxPool.instance.GetVfx(VfxList.GroundSlash, transform.position, Quaternion.Euler(rotation1));
        slash1.AddComponent<VfxGroundSlash_Triple>();
        slash1.GetComponent<VisualEffect>().SetFloat("Size", 1);

        var slash2 = VfxPool.instance.GetVfx(VfxList.GroundSlash, transform.position, Quaternion.Euler(rotation2));
        slash2.AddComponent<VfxGroundSlash_Triple>();
        slash2.GetComponent<VisualEffect>().SetFloat("Size", 1);
        yield return null;
    }

    public void SetupClone(bool clone, float vfxLifetime)
    {

        hasSetup = true;
    }
    IEnumerator CloneLogic()
    {
        while (!hasSetup)
        {
            yield return null;
        }

        float vfxTimer = 0;
        float vfxLerp = 0;
        while (true)
        {
            vfxTimer += Time.deltaTime;
            vfxLerp = vfxTimer / 2;
            GetComponent<Rigidbody>().velocity = Vector3.Lerp(transform.forward * 10, Vector3.zero, vfxLerp);
            if (vfxTimer > 2)
            {
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(2);
        GetComponent<PooledObject>().Return();
    }
}
