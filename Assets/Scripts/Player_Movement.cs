using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player_Movement : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] Transform face;


    [SerializeField] float moveSpd = 10;


    Vector3 inputs;
    Vector3 movement;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        inputs.x = Input.GetAxisRaw("Horizontal");
        inputs.z = Input.GetAxisRaw("Vertical");
        inputs = inputs.normalized;

        if (inputs != Vector3.zero)
            face.localPosition = inputs;

        movement.x = inputs.x * Time.deltaTime * moveSpd;
        movement.z = inputs.z * Time.deltaTime * moveSpd;
        movement.y = rb.velocity.y;


    }

    private void FixedUpdate()
    {
        rb.velocity = movement;
    }

    public Transform GetFace()
    {
        return face;
    }
}
