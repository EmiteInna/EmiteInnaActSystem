using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    public enum AreaType
    {
        CUBE,ELLIPSE
    }
    public struct SpellAnimationEvent
    {
        [LabelText("动画片段")]
        public AnimationClip clip;
        [LabelText("发生时间")]
        public float happenTime;
    }
    public struct SpellScriptEvent
    {
        [LabelText("事件名称")]
        public string eventName;
        [LabelText("发生时间")]
        public float happenTime;
      
    }
    public struct SpellAudioEvent
    {
        [LabelText("音频")]
        public AudioClip clip;
        [LabelText("播放位置偏移")]
        public Vector3 offset;
        [LabelText("播放音量")]
        public float playVolume;
        [LabelText("播放速度")]
        public float playPitch;
        [LabelText("发生时间")]
        public float happenTime;
    }

    public struct SpellAttackEvent
    {
        [LabelText("形状")]
        public AreaType areaType;
        [LabelText("相对玩家朝向的位置偏移量")]
        public Vector3 centerOffset;
        [LabelText("大小")]
        public Vector3 extends;
        [LabelText("角度，目前仅在椭圆下有用，分别是角度偏移和圆心角大小")]
        public Vector2 angle;
        [LabelText("对目标触发事件名")]
        public string eventName;
        [LabelText("发生时间")]
        public float happenTime;
        [LabelText("是否开启Debug")]
        public bool showDebugGizmos;
    }
    //TODO:特效播放

    /// <summary>
    /// 技能配置基类。
    /// </summary>
    [CreateAssetMenu(fileName = "NewSpellConfigure", menuName = "EmiteInnaACT/SpellConfigure", order = 4)]
    public class SpellConfigureBase : ConfigureBase
    {
        [LabelText("技能名(调用使用的名称)")]
        public string Name;
        [LabelText("技能名称")]
        public string SpellName;
        [LabelText("技能介绍")]
        public string SpellDescription;
        [LabelText("技能CD")]
        public float SpellCoolDown;
        [LabelText("技能蓝耗")]
        public float SpellManaCost;
        [LabelText("技能持续时长")]
        public float duration;
        [LabelText("技能速率(时间比)")]
        public float timeMultiplier;
        [LabelText("技能动画事件")]
        public List<SpellAnimationEvent> animationEvent=new List<SpellAnimationEvent>();
        [LabelText("技能逻辑事件名")]
        public List<SpellScriptEvent> scriptEvents=new List<SpellScriptEvent>();
        [LabelText("技能音效事件")]
        public List<SpellAudioEvent> audioEvents=new List<SpellAudioEvent>();
        [LabelText("技能伤害事件")]
        public List<SpellAttackEvent> attackEvents = new List<SpellAttackEvent>();
        [LabelText("技能被中断事件名列表")]
        public Dictionary<string, string> cancelEvents=new Dictionary<string, string>();
    }
}