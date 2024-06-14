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

    // Start is called before the first frame update
    void Start()
    {
        pm = GetComponent<Player_Movement>();
        pool = VfxPool.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var vectorTarget = (pm.GetFace().position - transform.position);
            var rotation = Quaternion.LookRotation(vectorTarget);
            rotation.x = 0;
            rotation.z = 0;
            //StartCoroutine(ShootVfx(VfxList.GroundSlash, transform.position - Vector3.up, rotation, vectorTarget));
            StartCoroutine(ShootVfx(VfxList.Fireball, transform.position - Vector3.up, rotation, vectorTarget));
        }
    }

    IEnumerator ShootVfx(VfxList vfx, Vector3 pos, Quaternion rot, Vector3 target)
    {
        float vfxTimer = 0;
        float vfxLerp = 0;
        var theVfx = pool.GetVfx(vfx, pos, rot);
        if (theVfx != null)
        {
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
            StartCoroutine(ReturnVfx(1, theVfx));
        }
    }

    IEnumerator ReturnVfx(float delay, GameObject vfx)
    {
        yield return new WaitForSeconds(delay);
        vfx.GetComponent<PooledObject>().Return();
    }
}
