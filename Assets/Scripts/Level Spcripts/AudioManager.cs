using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWarOneTools
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        public AudioSource[] soundEffects;

        public AudioSource bgm, levelEndMusic, bossMusic;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            PlayBG();
        }

        public void PlaySFX(int soundToPlay)
        {
            soundEffects[soundToPlay].Stop();

            soundEffects[soundToPlay].pitch = Random.Range(.9f, 1.1f);

            soundEffects[soundToPlay].Play();
        }

        private void PlayBG()
        {
            bgm.Play();
        }

        public void PlayLevelVictory()
        {
            bgm.Stop();
            levelEndMusic.Play();
        }

        public void PlayBossMusic()
        {
            bgm.Stop();
            bossMusic.Play();
        }

        public void StopBossMusic()
        {
            bossMusic.Stop();
            bgm.Play();
        }
    }
}