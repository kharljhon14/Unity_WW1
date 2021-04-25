using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWarOneTools 
{
    public class MenuSlot : MonoBehaviour
    {
        public int slotNumber;
        protected MainMenuManager mainMenu;

        private void Start()
        {
            mainMenu = FindObjectOfType<MainMenuManager>();
        }

        public virtual void NewGameSlot()
        {
            mainMenu.NewGame(slotNumber);
        }

        public virtual void LoadGameSlot()
        {
            mainMenu.LoadGame(slotNumber);
        }

    }
}

