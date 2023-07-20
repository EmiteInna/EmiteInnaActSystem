using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// 角色配置文件的基础，包含一些经常会用到的东西。
    /// </summary>
    [CreateAssetMenu(fileName = "NewCharacterConfigure", menuName = "EmiteInnaACT/CharacterConfigure", order = 3)]
    public class CharacterConfigureBase : ConfigureBase
    {
        public KeyCode VerticalUp;
        public KeyCode HorizontalRight;
        public KeyCode VerticalDown;
        public KeyCode HorizontalLeft;
        public KeyCode Jump;
        public KeyCode Sprint;
        public KeyCode Attack;
        public KeyCode Defend;
        public float MaxHealth;
        public float CurrentHealth;
        public float MovementSpeed;
        public float RotationSpeed;
        public float JumpHeight;
        public float SprintSpeedMultiplier;
        public AnimationClip IdelAnimation;
        public AnimationClip WalkAnimation;
        public AnimationClip SprintAnimation;
        public AnimationClip JumpRisingAnimation;
        public AnimationClip JumpLandingAnimation;
        public AnimationClip JumpLandedAnimation;
        public AnimationClip HurtAnimation;
        public AnimationClip StunAnimation;
        public AudioClip Footstep;
        public AudioClip Hurt;
        public AudioClip HeavilyHurt;
        [LabelText("技能列表")]
        public List<SpellConfigureBase> spellList = new List<SpellConfigureBase>();
       
    }
}