using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace WorldWarOneTools
{
    public class PlayerHealth : Health
    {
        [SerializeField] protected float iFrameTime;
        [SerializeField] protected float verticalDamageForce;
        [SerializeField] protected float horizontalDamageForce;
        [SerializeField] protected float slowDownTimeAmount;
        [SerializeField] protected float slowDownSpeed;

        protected SpriteRenderer[] sprites;
        protected Rigidbody2D rb2d;
        protected Image deadScreenImg;
        protected Text deadScreenText;
        protected float originalTimeScale;

        [HideInInspector] public bool invulnerable;
        [HideInInspector] public bool left;
        [HideInInspector] public bool hit;

        protected override void Initialization()
        {
            base.Initialization();

            rb2d = GetComponent<Rigidbody2D>();
            sprites = GetComponentsInChildren<SpriteRenderer>();
            deadScreenImg = GameUIManager.deathScreen.GetComponent<Image>();
            deadScreenText = GameUIManager.deathScreen.GetComponentInChildren<Text>();
        }

        protected virtual void FixedUpdate()
        {
            HandleIFrames();
            HandleDamageMovement();
        }

        public virtual void HandleDamageMovement()
        {
            if (hit)
            {
                Time.timeScale = slowDownSpeed;
                rb2d.AddForce(Vector2.up * verticalDamageForce);

                if (!left)
                    rb2d.AddForce(Vector2.left * horizontalDamageForce);

                else
                    rb2d.AddForce(Vector2.right * horizontalDamageForce);

                Invoke("HitCancel", slowDownTimeAmount);
            }
        }

        protected virtual void HandleIFrames()
        {
            Color spriteColor = new Color();

            if (invulnerable)
            {
                foreach (SpriteRenderer sprite in sprites)
                {
                    spriteColor = sprite.color;
                    spriteColor.a = .5f;
                    sprite.color = spriteColor;
                }
            }

            else
            {
                foreach (SpriteRenderer sprite in sprites)
                {
                    spriteColor = sprite.color;
                    spriteColor.a = 1f;
                    sprite.color = spriteColor;
                }
            }
        }

        protected virtual void Cancel()
        {
            invulnerable = false;
        }

        protected virtual void HitCancel()
        {
            hit = false;
            Time.timeScale = originalTimeScale;
        }

        public virtual void GainCurrentHealth(int amount)
        {
            healthPoints += amount;

            if (healthPoints > maxHealthPoints)
                healthPoints = maxHealthPoints;
        }

        public override void DealDamage(int amount)
        {
            if (!character.isDead)
            {
                if (invulnerable)
                    return;

                AudioManager.instance.PlaySFX(1);
                base.DealDamage(amount);

                if (healthPoints <= 0)
                {
                    character.isDead = true;
                    healthPoints = 0;
                    player.GetComponent<Animator>().SetBool("Dying", true);
                    StartCoroutine(Dead());
                }

                originalTimeScale = Time.timeScale;
                hit = true;
                invulnerable = true;

                Invoke("Cancel", iFrameTime);
            }       
        }

        protected virtual IEnumerator Dead()
        {
            GameUIManager.deathScreen.SetActive(true);

            float timeStarted = Time.time;
            float timeSinceStarted = Time.time - timeStarted;
            float percentageComplete = timeSinceStarted / 2f;

            Color currentColor = deadScreenImg.color;
            Color currentTextColor = deadScreenText.color;
            Color spriteColor = new Color();

            foreach (SpriteRenderer sprite in sprites)
            {
                spriteColor = sprite.color;
            }

            while (true)
            {
                timeSinceStarted = Time.time - timeStarted;
                percentageComplete = timeSinceStarted / 2f;
                currentColor.a = Mathf.Lerp(0, 1, percentageComplete);
                deadScreenImg.color = currentColor;
                currentTextColor.a = Mathf.Lerp(0, 1, percentageComplete);
                deadScreenText.color = currentTextColor;

                foreach (SpriteRenderer sprite in sprites)
                {
                    spriteColor.a = Mathf.Lerp(0, 1, percentageComplete);
                    sprite.color = spriteColor;
                }

                if (percentageComplete >= 1)
                    break;

                yield return new WaitForEndOfFrame();
            }

            Invoke("LoadGame", 2);
        }

        public virtual void LoadGame()
        {
            levelManager.loadFromSave = true;
            PlayerPrefs.SetInt(" " + character.gameFile + "LoadFromSave", levelManager.loadFromSave ? 1 : 0);
            string scene = PlayerPrefs.GetString(" " + character.gameFile + "LoadGame");

            SceneManager.LoadScene(scene);
        }
    }

}
