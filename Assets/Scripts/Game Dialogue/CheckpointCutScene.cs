using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointCutScene : MonoBehaviour
{
    [SerializeField] private GameObject dialogueUI;
    public void StartCutScene()
    {
        if(dialogueUI != null)
            dialogueUI.SetActive(true);
    }
}
