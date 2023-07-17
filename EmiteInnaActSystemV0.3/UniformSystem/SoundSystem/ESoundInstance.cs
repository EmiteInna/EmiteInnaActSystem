using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

namespace EmiteInnaACT
{
    public static class ESoundInstance
    {
        public const string InstanceName = "EmiteInnaSoundInstance";
        static BGMController bgmController;
        static SFXService sfxService;
        static Transform root;
        /// <summary>
        /// 初始化Initialize，一定要在对象池初始化之后进行喵。
        /// </summary>
        public static void Initialize()
        {
            root = new GameObject(InstanceName).transform;
            root.SetParent(GameObject.Find("EmiteInnaACTCore").transform);
            bgmController = new GameObject(InstanceName+"BGMController").AddComponent<BGMController>();
            bgmController.transform.parent = root;
            bgmController.Create();
            sfxService = new GameObject(InstanceName + "SFXController").AddComponent<SFXService>();
            sfxService.transform.parent = root;
            sfxService.CreateService();
        }
        /// <summary>
        /// 播放BGM
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="transition"></param>
        /// <param name="speed"></param>
        public static void PlayBGM(AudioClip clip, float transition = 0.2f, float speed = 1f)
        {
            bgmController.PlayBGM(clip, transition, speed);
        }
        /// <summary>
        /// 设置BGM音量
        /// </summary>
        /// <param name="volume"></param>
        public static void SetBGMVolume(float volume)
        {
            bgmController.SetBGMVolume(volume);
        }
        /// <summary>
        /// 暂停bgm播放
        /// </summary>
        public static void PauseBGM()
        {
            bgmController.PauseBGM();
        }
        /// <summary>
        /// 继续播放BGM
        /// </summary>
        public static void ResumeBGM()
        {
            bgmController?.ResumeBGM();
        }
        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="position"></param>
        /// <param name="duration"></param>
        /// <param name="volume"></param>
        /// <param name="speed"></param>
        public static void PlaySFX(AudioClip clip, Vector3 position, float duration = 5f, float volume = 1f, float speed = 1f)
        {
            sfxService.PlaySFX(clip, position, duration, volume, speed);
        }
    }
}
