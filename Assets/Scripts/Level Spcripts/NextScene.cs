using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWarOneTools
{
    public class NextScene : Manager
    {
        [SerializeField] protected SceneReference nextScene;
        [SerializeField] protected int locationReference;
        [SerializeField] private GameObject buttonUI;

        protected override void Initialization()
        {
            base.Initialization();
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                buttonUI.SetActive(true);
                if(SimpleInput.GetButton("Fire2"))
                    levelManager.NextScene(nextScene, locationReference);
            }
        }

        protected virtual void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                buttonUI.SetActive(true);
                if (SimpleInput.GetButton("Fire2"))
                    levelManager.NextScene(nextScene, locationReference);
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            buttonUI.SetActive(false);
        }
    }

}
