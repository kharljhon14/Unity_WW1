using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace WorldWarOneTools
{
    public class PlayerName : MonoBehaviour
    {
        private AuthManager authManager;
        private Camera camera;
        [SerializeField] private TextMeshProUGUI username;

        public bool multiplayer;

        bool facingLeft;

        private void Start()
        {
            camera = Camera.main;
            authManager = FindObjectOfType<AuthManager>();
            if(!multiplayer)
                username.text = authManager.user.DisplayName;
        }

        private void Update()
        {
            if (SimpleInput.GetAxisRaw("Horizontal") >= .5f)
                facingLeft = false;

            else if (SimpleInput.GetAxisRaw("Horizontal") <= -.5f)
                facingLeft = true;

            if(!multiplayer)
                FlipCanvas();
        }

        private void FlipCanvas()
        {
            if (facingLeft)
                transform.localScale = new Vector3(-1, 1, 1);

            else
                transform.localScale = Vector3.one;
        }
    }
}

