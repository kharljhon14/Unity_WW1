using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWarOneTools
{
    public class Abilities : Character
    {
        protected Character character;

        protected override void Initialization()
        {
            base.Initialization();

            //Get Character Reference
            character = GetComponent<Character>();
        }
    }
}

