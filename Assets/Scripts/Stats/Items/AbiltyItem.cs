using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWarOneTools
{
    [CreateAssetMenu(fileName = "ItemType", menuName = "WW1/Items/Consumable/Ability", order = 2)]
    public class AbiltyItem : Consumable
    {
        public override void UseItem(GameObject player)
        {
            base.UseItem(player);
            player.GetComponent<Abilities>().Invoke(itemName, 0);
        }
    }

}
