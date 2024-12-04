using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private bool enableLog;

    [HideInInspector] public LevelManager CurrentActiveLevelManager { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        LogManager.SetEnableLog(enableLog);
    }

    public void SetCurrentActiveLevelManager(LevelManager levelManager)
    {
        if (levelManager == null) return;
        CurrentActiveLevelManager = levelManager;
    }
}
