using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxShooter : MonoBehaviour
{
    Player_Movement playerMovement;
    VfxPool pool;

    [SerializeField] GameObject debugGameObject;
    [SerializeField] float zDist;


    [SerializeField] float vfxSpd = 20;
    [SerializeField] float vfxLifetime = 20;

    bool toggle = false;

    public VfxList DesiredVfx = VfxList.Fireball;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<Player_Movement>();
        pool = VfxPool.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            toggle = !toggle;
            //Shoot(toggle ? VfxList.Lightning : VfxList.GroundSlash, transform.position - Vector3.up);


            Shoot(DesiredVfx, transform.position - Vector3.up);
        }

        //if (Input.GetKeyDown(KeyCode.Mouse1))
        //{
        //    toggle = !toggle;
        //    Shoot(toggle ? VfxList.Arrow : VfxList.Fireball, transform.position);
        //}
    }

    public void Shoot(VfxList vfx, Vector3 pos)
    {
        var vectorTarget = (playerMovement.GetFace().position - transform.position);

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = zDist;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.y = transform.position.y;
        debugGameObject.transform.position = mousePos;
        vectorTarget = mousePos;

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
                    theVfx.GetComponent<Rigidbody>().velocity = target * vfxSpd;
                    while (true)
                    {
                        vfxTimer += Time.deltaTime;
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
                        vfxLerp *= vfxLerp;
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
