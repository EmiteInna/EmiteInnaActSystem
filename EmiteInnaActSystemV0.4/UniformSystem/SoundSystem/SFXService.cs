using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    public class SFXService:MonoBehaviour
    {
        /// <summary>
        /// 初始化service，创建音效对象池，在对象池的initailize之后调用。
        /// </summary>
        public void CreateService()
        {
            GameObject prefab = ConfigureInstance.GetValue<GameObject>("Sound", "SFXPrefab");
            EPoolInstance.CreateObjectPool<GameObject>("SFX",100,50,true,prefab);
        }
        /// <summary>
        /// 播放SFX
        /// </summary>
        /// <param name="clip">片段名</param>
        /// <param name="position">播放位置</param>
        /// <param name="duration">持续时间</param>
        /// <param name="volume">音量</param>
        /// <param name="speed">播放速度</param>
        public void PlaySFX(AudioClip clip,Vector3 position,float duration=5f,float volume=1f,float speed = 1f)
        {
            GameObject sfx = EPoolInstance.Get<GameObject>("SFX", true,transform);
            sfx.transform.position = position;
            AudioSource src = sfx.GetComponent<AudioSource>();
            src.clip = clip;
            src.pitch = speed;
            src.volume = volume;
            src.Play();
            StartCoroutine(recycleSFX(src, duration));
            
        }
        IEnumerator recycleSFX(AudioSource src,float duration)
        {
            yield return new WaitForSecondsRealtime(duration);
            src.Stop();
            EPoolInstance.Push(src.gameObject, "SFX", true);
        }
    }
}