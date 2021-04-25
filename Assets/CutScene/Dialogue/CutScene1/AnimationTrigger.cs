using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWarOneTools
{
    public class AnimationTrigger : MonoBehaviour
    {
        [SerializeField] private BetterDialogue betterDialogue;
        private Animator anim;
        [SerializeField] private int triggerCount;

        private void Start()
        {
            anim = GetComponent<Animator>();
        }

        private void Update()
        {
            if(betterDialogue.index == triggerCount)
            {
                anim.SetBool("Move", true);
            }
        }
    }
}
