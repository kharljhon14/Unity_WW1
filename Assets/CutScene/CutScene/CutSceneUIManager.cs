using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneUIManager : MonoBehaviour
{

    public static CutSceneUIManager instance;

    public GameObject CutScene1;
    public GameObject CutScene2;
    public GameObject CutScene3;
    public GameObject CutScene4;

    public GameObject Button1;
    public GameObject Button2;
    public GameObject Button3;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void CutScene2UI()
    {
        CutScene3.SetActive(true);
        CutScene2.SetActive(false);
    }
    public void CutScene1UI()
    {
        CutScene2.SetActive(true);
        CutScene1.SetActive(false);
        Button1.SetActive(false);
        Button2.SetActive(true);
    }
   
    public void CutScene3UI()
    {
        Button3.SetActive(true);
        Button1.SetActive(false);
        CutScene3.SetActive(true);
    }

    public void CutScene4UI()
    {
        Button3.SetActive(true);
        Button2.SetActive(false);
        CutScene4.SetActive(true);
    }

}
