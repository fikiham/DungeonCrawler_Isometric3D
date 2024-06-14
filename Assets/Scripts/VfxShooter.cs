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
    [SerializeField] float slashSpd = 10;

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
            ShootVfx(VfxList.GroundSlash, transform.position - Vector3.up, rotation, vectorTarget);
        }
    }

    void ShootVfx(VfxList vfx, Vector3 pos, Quaternion rot, Vector3 target)
    {
        var theVfx = pool.GetVfx(vfx, pos, rot);
        if (theVfx != null)
        {
            theVfx.GetComponent<Rigidbody>().velocity = target * slashSpd;
            StartCoroutine(ReturnVfx(5, theVfx));
        }
    }

    IEnumerator ReturnVfx(float delay, GameObject vfx)
    {
        yield return new WaitForSeconds(delay);
        pool.ReturnVfx(vfx);
    }
}
