using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxShooter : MonoBehaviour
{
    Player_Movement pm;

    [SerializeField] GameObject Vfx_GroundSlash;
    [SerializeField] float slashSpd = 10;

    // Start is called before the first frame update
    void Start()
    {
        pm = GetComponent<Player_Movement>();
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
            var theSlash = Instantiate(Vfx_GroundSlash, transform.position - Vector3.up, rotation, transform);
            theSlash.GetComponent<Rigidbody>().velocity = vectorTarget * slashSpd;
            Destroy(theSlash, 10);
        }
    }
}
