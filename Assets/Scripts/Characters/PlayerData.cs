using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class PlayerData
{
    public int coins;
    public int premium;

    public bool isTutorialDone;

    public Dictionary<Consumable.ConsumableType, int> consumables = new Dictionary<Consumable.ConsumableType, int>();   // Inventory of owned consumables and quantity.
    //Player win a rank ever 300m (e.g. a player having reached 1200m at least once will be rank 4)
    public int rank = 0;
}
