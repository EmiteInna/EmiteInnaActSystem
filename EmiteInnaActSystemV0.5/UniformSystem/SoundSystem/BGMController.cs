using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

namespace EmiteInnaACT
{
    public class BGMController : MonoBehaviour
    {
        public GameObject bgmPlayer1;
        public GameObject bgmPlayer2;
        AudioSource src1;
        AudioSource src2;
        bool havePlayed = false;
        bool paused = false;
        float precentWeight;
        private Coroutine swap;
        float baseVolume;
        /// <summary>
        /// ≥ı ºªØ
        /// </summary>
        public void Create()
        {
            precentWeight = 1;
            paused = false;
            bgmPlayer1= ConfigureInstance.GetValue<GameObject>("Sound", "BGMPrefab");
            bgmPlayer2= ConfigureInstance.GetValue<GameObject>("Sound", "BGMPrefab");
            bgmPlayer1 = Instantiate(bgmPlayer1);
            bgmPlayer1.name = "BGMPlayer1";
            bgmPlayer1.transform.SetParent(transform);
            bgmPlayer2 = Instantiate(bgmPlayer2);
            bgmPlayer2.name = "BGMPlayer2";
            bgmPlayer2.transform.SetParent(transform);
            src1 = bgmPlayer1.GetComponent<AudioSource>();
            src2 = bgmPlayer2.GetComponent<AudioSource>();
            src1.loop = true;
            src2.loop = true;
            baseVolume = 1;
        }
        private void OnDestroy()
        {
            src1.Stop();
            src2.Stop();
            Destroy(bgmPlayer1);
            Destroy(bgmPlayer2);
        }
        /// <summary>
        /// ≤•∑≈±≥æ∞“Ù¿÷°£
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="transition"></param>
        /// <param name="speed"></param>
        public void PlayBGM(AudioClip clip, float transition = 0.2f, float speed = 1f)
        {
            if (havePlayed == false)
            {
                src2.clip = clip;
                src2.pitch = 1f;
                src2.Play();
            }
            else
            {
                float time = src2.time;
                src1.clip = src2.clip;
                src1.pitch = src2.pitch;
                src1.time = time;
                src2.clip = clip;
                src2.pitch = speed;
                src2.Play();
                if (swap != null) StopCoroutine(swap);
                swap = StartCoroutine(DoPlayBGM(transition));
            }
            havePlayed = true;

        }
        IEnumerator DoPlayBGM(float transition)
        {
            src1.Play();
            float speed = Time.fixedDeltaTime / transition;
            precentWeight = 1 - precentWeight;
            while (precentWeight < 1)
            {
                if (!paused)
                    precentWeight += speed;
//                Debug.Log("Updating" + precentWeight);
                src1.volume = (1 - precentWeight)*baseVolume;
                src2.volume = precentWeight*baseVolume;
                yield return new WaitForFixedUpdate();
            }
            src1.Stop();
        }
        /// <summary>
        /// …Ë÷√BGM“Ù¡ø
        /// </summary>
        /// <param name="volume"></param>
        public void SetBGMVolume(float volume)
        {
            baseVolume = volume;
            src2.volume = precentWeight * baseVolume;
            src1.volume = (1-precentWeight) * baseVolume;
        }
        /// <summary>
        /// ‘›Õ£≤•∑≈°£
        /// </summary>
        public void PauseBGM()
        {
            src1.Pause();
            src2.Pause();
            paused = true;
        }
        public void ResumeBGM()
        {
            src1.UnPause();
            src2.UnPause();
            paused = false;
        }
        // Update is called once per frame
        void Update()
        {
            if (havePlayed)
            {
                bgmPlayer1.transform.position = Camera.main.transform.position;
                bgmPlayer2.transform.position = Camera.main.transform.position;
            }
        }
    }
}