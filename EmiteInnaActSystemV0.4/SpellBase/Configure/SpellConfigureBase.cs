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
        [LabelText("����Ƭ��")]
        public AnimationClip clip;
        [LabelText("����ʱ��")]
        public float happenTime;
    }
    public struct SpellScriptEvent
    {
        [LabelText("�¼�����")]
        public string eventName;
        [LabelText("����ʱ��")]
        public float happenTime;
      
    }
    public struct SpellAudioEvent
    {
        [LabelText("��Ƶ")]
        public AudioClip clip;
        [LabelText("����λ��ƫ��")]
        public Vector3 offset;
        [LabelText("��������")]
        public float playVolume;
        [LabelText("�����ٶ�")]
        public float playPitch;
        [LabelText("����ʱ��")]
        public float happenTime;
    }

    public struct SpellAttackEvent
    {
        [LabelText("��״")]
        public AreaType areaType;
        [LabelText("�����ҳ����λ��ƫ����")]
        public Vector3 centerOffset;
        [LabelText("��С")]
        public Vector3 extends;
        [LabelText("�Ƕȣ�Ŀǰ������Բ�����ã��ֱ��ǽǶ�ƫ�ƺ�Բ�ĽǴ�С")]
        public Vector2 angle;
        [LabelText("��Ŀ�괥���¼���")]
        public string eventName;
        [LabelText("����ʱ��")]
        public float happenTime;
        [LabelText("�Ƿ���Debug")]
        public bool showDebugGizmos;
    }
    //TODO:��Ч����

    /// <summary>
    /// �������û��ࡣ
    /// </summary>
    [CreateAssetMenu(fileName = "NewSpellConfigure", menuName = "EmiteInnaACT/SpellConfigure", order = 4)]
    public class SpellConfigureBase : ConfigureBase
    {
        [LabelText("������(����ʹ�õ�����)")]
        public string Name;
        [LabelText("��������")]
        public string SpellName;
        [LabelText("���ܽ���")]
        public string SpellDescription;
        [LabelText("����CD")]
        public float SpellCoolDown;
        [LabelText("��������")]
        public float SpellManaCost;
        [LabelText("���ܳ���ʱ��")]
        public float duration;
        [LabelText("��������(ʱ���)")]
        public float timeMultiplier;
        [LabelText("���ܶ����¼�")]
        public List<SpellAnimationEvent> animationEvent=new List<SpellAnimationEvent>();
        [LabelText("�����߼��¼���")]
        public List<SpellScriptEvent> scriptEvents=new List<SpellScriptEvent>();
        [LabelText("������Ч�¼�")]
        public List<SpellAudioEvent> audioEvents=new List<SpellAudioEvent>();
        [LabelText("�����˺��¼�")]
        public List<SpellAttackEvent> attackEvents = new List<SpellAttackEvent>();
        [LabelText("���ܱ��ж��¼����б�")]
        public Dictionary<string, string> cancelEvents=new Dictionary<string, string>();
    }
}