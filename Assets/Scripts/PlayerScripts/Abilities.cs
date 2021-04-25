using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWarOneTools
{
    public class Abilities : Character
    {
        protected Character character;

        [HideInInspector] public bool weaponAbility;

        protected override void Initialization()
        {
            base.Initialization();

            //Get Character Reference
            character = GetComponent<Character>();
            weaponAbility = PlayerPrefs.GetInt(" " + character.gameFile + "WeaponAbility") == 1 ? true : false;

            TurnOnAbilities();
        }

        public virtual void WeaponAbility()
        {
            weaponAbility = true;
            weapon.enabled = true;

            PlayerPrefs.SetInt(" " + character.gameFile + "WeaponAbility", weaponAbility ? 1 : 0);
        }

        public virtual void TurnOnAbilities()
        {
            if (weaponAbility)
            {
                weapon.enabled = true;
            }
        }
    }
}

