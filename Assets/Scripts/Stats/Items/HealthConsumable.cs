using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWarOneTools
{
    [CreateAssetMenu(fileName = "ItemType", menuName = "WW1/Items/Consumable/HealthConsumable", order = 2)]
    public class HealthConsumable : Consumable
    {
        public int amount;

        public override void UseItem(GameObject player)
        {
            player.GetComponent<PlayerHealth>().GainCurrentHealth(amount);
            base.UseItem(player);
        }
    }

}

