using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] characters;
    [SerializeField] private Transform spawnPostion;

    private void Start()
    {
        SpawnCharacter();
    }

    private void SpawnCharacter()
    {
        if (PlayerPrefs.GetInt("char") == 0)
            Instantiate(characters[0], spawnPostion.position, Quaternion.identity);

        if (PlayerPrefs.GetInt("char") == 1)
            Instantiate(characters[1], spawnPostion.position, Quaternion.identity);
    }
}
