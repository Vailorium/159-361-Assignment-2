using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Interactable
{
    public void interact(PlayerController pC = null, GameObject obj = null);
}
