using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player;
    Vector3 offset;
    float normalDistance;

    [SerializeField] float dampPosition;

    float followSpd;
    [SerializeField] float normalSpd = 1;
    [SerializeField] float chaseSpd = 10;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.position;
        normalDistance = Vector3.Distance(player.position, transform.position);
    }
    private void LateUpdate()
    {
        if (Vector3.Distance(player.position, transform.position) > normalDistance + dampPosition)
            followSpd = chaseSpd;
        else
            followSpd = normalSpd;


        transform.position = Vector3.MoveTowards(transform.position, offset + player.position, followSpd);
    }
}
