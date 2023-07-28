using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// ��ɫ�����ļ��Ļ���������һЩ�������õ��Ķ�����
    /// </summary>
    [CreateAssetMenu(fileName = "NewCharacterConfigure", menuName = "EmiteInnaACT/CharacterConfigure", order = 3)]
    public class CharacterConfigureBase : ConfigureBase
    {
        [LabelText("��ݼ����������")]
        public KeyCode VerticalUp;
        [LabelText("��ݼ����������")]
        public KeyCode HorizontalRight;
        [LabelText("��ݼ����������")]
        public KeyCode VerticalDown;
        [LabelText("��ݼ����������")]
        public KeyCode HorizontalLeft;
        [LabelText("��ݼ�����Ծ")]
        public KeyCode Jump;
        [LabelText("����")]
        public KeyCode Sprint;
        [LabelText("��ݼ�������")]
        public KeyCode Attack;
        [LabelText("��ݼ�������")]
        public KeyCode Defend;
        [LabelText("��ݼ�������1")]
        public KeyCode SpellOne;
        [LabelText("��ݼ�������2")]
        public KeyCode SpellTwo;
        [LabelText("��ݼ�����ɱ��")]
        public KeyCode Ulti;
        [LabelText("��ݼ������ܽ���(��)")]
        public KeyCode SwapSpellLeft;
        [LabelText("��ݼ������ܽ���(��)")]
        public KeyCode SwapSpellRight;
        [LabelText("�������ֵ")]
        public float MaxHealth;
        [LabelText("��ǰ����ֵ")]
        public float CurrentHealth;
        [LabelText("�����ֵ")]
        public float MaxMental;
        [LabelText("��ǰ����ֵ")]
        public float CurrentMental;
        [LabelText("�ƶ��ٶ�")]
        public float MovementSpeed;
        [LabelText("��ת�ٶ�")]
        public float RotationSpeed;
        [LabelText("��Ծ�߶�")]
        public float JumpHeight;
        [LabelText("����Ծ�߶�")]
        public float LongJumpHeight;
        [LabelText("�����ٶȱ���")]
        public float SprintSpeedMultiplier;
        [LabelText("�������ٱ���")]
        public float AirSpeedMultiplier;
        [LabelText("��־")]
        public float Mentality;
        [LabelText("����")]
        public float Resilence;
        [LabelText("��̾���")]
        public float DashDistance;
        [LabelText("����ʱ��")]
        public float DefendDuration;
        [LabelText("��Դ����ֹ����")]
        public AnimationClip IdelAnimation;
        [LabelText("��Դ����·����")]
        public AnimationClip WalkAnimation;
        [LabelText("��Դ�����ܶ���")]
        public AnimationClip SprintAnimation;
        [LabelText("��Դ����Ծ��������")]
        public AnimationClip JumpRisingAnimation;
        [LabelText("��Դ����Ծ�½�����")]
        public AnimationClip JumpLandingAnimation;
        [LabelText("��Դ����½����")]
        public AnimationClip JumpLandedAnimation;
        [LabelText("��Դ�����˶���")]
        public AnimationClip HurtAnimation;
        [LabelText("��Դ��ѣ�ζ���")]
        public AnimationClip StunAnimation;
        [LabelText("��Դ����������")]
        public AnimationClip DefendAnimation;
        [LabelText("��Դ����̶���")]
        public AnimationClip DashAnimation;
        [LabelText("��Դ���Ų���")]
        public AudioClip Footstep;
        [LabelText("��Դ��������Ч")]
        public AudioClip Hurt;
        [LabelText("��Դ��������Ч")]
        public AudioClip HeavilyHurt;
        [LabelText("�����б�")]
        public List<SpellConfigureBase> spellList = new List<SpellConfigureBase>();
       
    }
}