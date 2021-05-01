using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WorldWarOneTools
{
    public class HealthBar : Manager
    {
        protected Slider slider;
        protected PlayerHealth playerHealth;
        [SerializeField] private Image hp;

        protected override void Initialization()
        {
            base.Initialization();
            slider = GetComponent<Slider>();
            playerHealth = player.GetComponent<PlayerHealth>();

            slider.maxValue = playerHealth.maxHealthPoints;
            slider.value = PlayerPrefs.GetInt(" " + character.gameFile + "CurrentHealth");
        }

        private void Update()
        {
            if (slider.value >= 100)
                hp.color = Color.green;

            if (slider.value < 60)
                hp.color = Color.yellow;

            if (slider.value < 30)
                hp.color = Color.red;
        }

        private void LateUpdate()
        {
            slider.value = playerHealth.healthPoints;
        }
    }
}

