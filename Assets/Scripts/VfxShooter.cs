using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.DebugUI.Table;

public class VfxShooter : MonoBehaviour
{
    Player_Movement pm;
    VfxPool pool;

    [SerializeField] GameObject Vfx_GroundSlash;
    [SerializeField] float vfxSpd = 20;
    [SerializeField] float vfxLifetime = 20;

    bool toggle = false;

    // Start is called before the first frame update
    void Start()
    {
        pm = GetComponent<Player_Movement>();
        pool = VfxPool.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            toggle = !toggle;
            //Shoot(toggle ? VfxList.Lightning : VfxList.GroundSlash, transform.position - Vector3.up);
            Shoot(toggle ? VfxList.GroundSlash_Triple : VfxList.GroundSlash, transform.position - Vector3.up);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Shoot(VfxList.Fireball, transform.position);
        }
    }

    public void Shoot(VfxList vfx, Vector3 pos)
    {
        var vectorTarget = (pm.GetFace().position - transform.position);
        var rotation = Quaternion.LookRotation(vectorTarget);
        rotation.x = 0;
        rotation.z = 0;
        StartCoroutine(ShootingVfx(vfx, pos, rotation, vectorTarget));

    }

    IEnumerator ShootingVfx(VfxList vfx, Vector3 pos, Quaternion rot, Vector3 target)
    {
        float vfxTimer = 0;
        float vfxLerp = 0;
        var theVfx = pool.GetVfx(vfx, pos, rot);

        if (theVfx != null)
        {
            theVfx.TryGetComponent<VfxChildren>(out var child);

            switch (theVfx.GetComponent<PooledObject>().type)
            {
                case VfxType.UnlimitedRange:
                    child?.Setup();
                    theVfx.name = "THE ORIGINAL";
                    while (true)
                    {
                        vfxTimer += Time.deltaTime;
                        theVfx.GetComponent<Rigidbody>().velocity = target * vfxSpd;
                        if (vfxTimer > vfxLifetime)
                        {
                            break;
                        }
                        yield return null;
                    }
                    StartCoroutine(ReturnVfx(2, theVfx));
                    break;
                case VfxType.LimitedRange:
                    child?.Setup();
                    while (true)
                    {
                        vfxTimer += Time.deltaTime;
                        vfxLerp = vfxTimer / vfxLifetime;
                        theVfx.GetComponent<Rigidbody>().velocity = Vector3.Lerp(target * vfxSpd, Vector3.zero, vfxLerp);
                        if (vfxTimer > vfxLifetime)
                        {
                            break;
                        }
                        yield return null;
                    }
                    StartCoroutine(ReturnVfx(2, theVfx));
                    break;
                case VfxType.Other:
                    if (vfx == VfxList.Lightning)
                    {
                        theVfx.transform.position += target.normalized * 2;
                        theVfx.GetComponent<VfxLightning>().Setup(target, 10, 3, .5f);
                    }
                    break;
            }
        }
    }

    IEnumerator ReturnVfx(float delay, GameObject vfx)
    {
        yield return new WaitForSeconds(delay);
        vfx.GetComponent<PooledObject>().Return();
    }
}
