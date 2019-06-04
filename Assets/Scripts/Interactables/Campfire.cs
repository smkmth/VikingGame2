using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : Interactable {

    public override void UseInteractable(GameObject user)
    {
        user.GetComponent<PlayerInteraction>().SetCraftingMode(interactableType.Fire);
    }

        
}
