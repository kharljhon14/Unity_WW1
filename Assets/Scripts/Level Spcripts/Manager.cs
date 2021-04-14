using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWarOneTools
{
    public class Manager : GameManager
    {
        protected GameManager gameManager;
        protected override void Initialization()
        {
            base.Initialization();
            gameManager = FindObjectOfType<GameManager>();
        }
    }
}
