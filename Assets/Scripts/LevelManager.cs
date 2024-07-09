using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform playerSpawnSpot;

    // Start is called before the first frame update
    void Start()
    {
        player.gameObject.SetActive(false);
        player.position = playerSpawnSpot.position;
        player.gameObject.SetActive(true);
    }

}
