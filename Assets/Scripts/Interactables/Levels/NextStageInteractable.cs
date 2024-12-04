using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStageInteractable : Interactable
{
    private LevelManager levelManagerRef;

    public void Initialize(LevelManager levelManager)
    {
        levelManagerRef = levelManager;
    }

    public override void Interact()
    {
        base.Interact();
        levelManagerRef.LoadNextStage();
    }
}
