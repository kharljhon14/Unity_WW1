using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour
{
    public void Male()
    {
        PlayerPrefs.SetInt("char", 0);
        SceneManager.LoadScene(1);
    }

    public void Female()
    {
        PlayerPrefs.SetInt("char", 1);
        SceneManager.LoadScene(1);
    }
}
