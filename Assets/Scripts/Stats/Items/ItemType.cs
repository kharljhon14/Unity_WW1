using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWarOneTools
{
    [CreateAssetMenu(fileName = "ItemType", menuName = "WW1/Items/NullItems", order = 2)]
    public class ItemType : ScriptableObject
    {
        public virtual void UseItem(GameObject player)
        {

        }
    }

}
