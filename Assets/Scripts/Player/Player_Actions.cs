using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player_Actions : MonoBehaviour
{
    Player_Interacts player_Interacts;
    VfxShooter vfxShooter;
    [SerializeField] TMP_Text promptText;


    private void Awake()
    {
        player_Interacts = GetComponent<Player_Interacts>();
        vfxShooter = GetComponent<VfxShooter>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        player_Interacts.CheckInteractables(out var item);
        promptText.text = (item != null && item.promptMessage != string.Empty) ? "Press F to " + item.promptMessage : string.Empty;

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (item != null)
            {
                item.Interact();

                if (item is WeaponsInteractable weapon)
                {
                    vfxShooter.DesiredVfx = weapon.WeaponEffect;
                }
            }
        }
    }
}
