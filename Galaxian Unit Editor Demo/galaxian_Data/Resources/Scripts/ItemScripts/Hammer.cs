using UnityEngine;
using System.Collections;

public class Hammer : Item
{
    void Start()
    {
        Name = "Hammer";
        activeSlot = SlotName.Center;
        durability = 3;
    }

}
