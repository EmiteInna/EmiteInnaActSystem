using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace EmiteInnaACT
{
    /// <summary>
    /// 一个结构体，为了应付连续播放相同动画的情况。
    /// </summary>
    public struct AnimationClipStruct
    {
        public AnimationClipPlayable cp0;
        public AnimationClipPlayable cp1;
        public int now;
    }
    /// <summary>
    /// 动画控制器，注意这个控件是挂在显示层上而不是根部。
    /// 很简单的控制器，只能负责两个动画的切换和过渡。
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimationController:MonoBehaviour
    {
        public Dictionary<int,AnimationClipStruct> clipDict = new Dictionary<int, AnimationClipStruct>();
        public CharacterControllerBase controller;
        public Animator animator;
        PlayableGraph graph;
        AnimationClipPlayable clip1;
        AnimationClipPlayable clip2;
        AnimationMixerPlayable rootmixer;
        bool isFirstPlay = true;
        float precentWeight;
        Coroutine swap;
        /// <summary>
        /// 调用Controller的相应函数
        /// </summary>
        /// <param name="str"></param>
        public void OnApplyCharacterEvent(string str)
        {
            controller.OnApplyCharacterEvent(str);
        }
        /// <summary>
        /// 动画机的初始化
        /// </summary>
        public void Initialize(CharacterControllerBase controller)
        {
            controller.animationController = this;
            this.controller = controller;
            animator = GetComponent<Animator>();
            graph = PlayableGraph.Create("Player");
            graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
            rootmixer = AnimationMixerPlayable.Create(graph, 2);
            var playableOutput = AnimationPlayableOutput.Create(graph, "输出", animator);
            playableOutput.SetSourcePlayable(rootmixer);
        }
        /// <summary>
        /// 释放
        /// </summary>
        public void Destroy()
        {
            clipDict.Clear();
            clipDict = null;
            graph.Stop();
            graph.Destroy();
        }
        /// <summary>
        /// 获取ClipPlayble，没有则创建一个。
        /// </summary>
        /// <param name="clip"></param>
        /// <returns></returns>
        public AnimationClipPlayable GetClipPlayable(AnimationClip clip)
        {
            if(clipDict.TryGetValue(clip.GetInstanceID(),out AnimationClipStruct c))
            {
                //如果是同类就取copy
                if (c.now==0)
                {
                //    Debug.Log("我是0");
                    c.now = 1;
                    clipDict[clip.GetInstanceID()] = c;
                    return c.cp1;
                }
            //    Debug.Log("我是1");
                c.now = 0;
                clipDict[clip.GetInstanceID()] = c;
                return c.cp0;
            }
            else
            {
                AnimationClipPlayable cp = AnimationClipPlayable.Create(graph, clip);
                AnimationClipPlayable cp1 = AnimationClipPlayable.Create(graph, clip);
                AnimationClipStruct s = new AnimationClipStruct();
                s.cp0 = cp;
                s.cp1 = cp1;
                s.now = 0;
                clipDict.Add(clip.GetInstanceID(), s);
                return cp;
            }
        }
        /// <summary>
        /// 滚动播放，主播放的永远是2
        /// </summary>
        /// <param name="clip">播放的clip</param>
        /// <param name="transition">过渡时间</param>
        public void PlayAnimation(AnimationClip clip,float transition=0f,float speed=1f)
        {
     //       if(clip1.GetAnimationClip()!=null&&clip2.GetAnimationClip()!=null)
            if (isFirstPlay)
            {
                precentWeight = 1;
                clip2 = GetClipPlayable(clip);
                graph.Connect(clip2, 0, rootmixer, 0);
                rootmixer.SetInputWeight(0, 1f);
            }
            else
            {
                
                clip1 = clip2;
                clip2 = GetClipPlayable(clip);
                clip2.SetSpeed(speed);
                graph.Disconnect(rootmixer, 0);
                graph.Disconnect(rootmixer, 1);
                
                graph.Connect(clip1, 0, rootmixer, 0);
                graph.Connect(clip2, 0, rootmixer, 1);
                if (swap != null) controller.StopCoroutine(swap);
                swap = controller.StartCoroutine(DoPlayAnimation(transition));
            }
            isFirstPlay = false;
            if (graph.IsPlaying() == false)
            {
                graph.Play();
            }
        }
        IEnumerator DoPlayAnimation(float transition)
        {
            float speed = Time.fixedDeltaTime / transition;
            precentWeight = 1 - precentWeight;
            while (precentWeight < 1)
            {
                precentWeight += speed;
                rootmixer.SetInputWeight(0, 1 - precentWeight);
                rootmixer.SetInputWeight(1, precentWeight);
                yield return new WaitForFixedUpdate();
            }
            rootmixer.SetInputWeight(0, 0);
            rootmixer.SetInputWeight(1, 1);
        }
    }
}