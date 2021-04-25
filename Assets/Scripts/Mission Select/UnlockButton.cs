using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WorldWarOneTools
{
    public class UnlockButton : MonoBehaviour
    {
        [SerializeField] private int stageNumber;
        [SerializeField] private Button[] levels;
        private void Start()
        {
            //Debug.Log(PlayerPrefs.GetInt("Mission" + stageNumber));
            //if (PlayerPrefs.GetInt("Mission" + stageNumber) == 1)
            //    GetComponent<Button>().interactable = true;

            //else
            //    GetComponent<Button>().interactable = false;

            if(PlayerPrefs.GetInt("Mission") <= 0)
            {
                for (int i = 0; i < levels.Length; i++)
                {
                    levels[i].interactable = false;
                }
            }

            else
            {
                for (int i = 0; i < PlayerPrefs.GetInt("Mission"); i++)
                {
                    levels[i].interactable = true;
                }
            }  
        }
    }
}
