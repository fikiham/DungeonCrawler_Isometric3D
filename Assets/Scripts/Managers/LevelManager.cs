using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private GameManager gameManagerRef;

    [SerializeField] Transform player;
    [SerializeField] Transform playerSpawnSpot;

    [SerializeField] NextStageInteractable nextStageInteractable;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerRef = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gameManagerRef.SetCurrentActiveLevelManager(this);

        player.gameObject.SetActive(false);
        player.position = playerSpawnSpot.position;
        player.gameObject.SetActive(true);

        nextStageInteractable.Initialize(this);
    }


    public void LoadNextStage()
    {
        //LogManager.LogMessage(this, "Loading next stage");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
