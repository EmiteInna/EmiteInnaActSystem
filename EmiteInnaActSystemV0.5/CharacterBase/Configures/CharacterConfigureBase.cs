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
        [LabelText("快捷键：方向键↑")]
        public KeyCode VerticalUp;
        [LabelText("快捷键：方向键→")]
        public KeyCode HorizontalRight;
        [LabelText("快捷键：方向键↓")]
        public KeyCode VerticalDown;
        [LabelText("快捷键：方向键←")]
        public KeyCode HorizontalLeft;
        [LabelText("快捷键：跳跃")]
        public KeyCode Jump;
        [LabelText("弃用")]
        public KeyCode Sprint;
        [LabelText("快捷键：攻击")]
        public KeyCode Attack;
        [LabelText("快捷键：防御")]
        public KeyCode Defend;
        [LabelText("快捷键：技能1")]
        public KeyCode SpellOne;
        [LabelText("快捷键：技能2")]
        public KeyCode SpellTwo;
        [LabelText("快捷键：必杀技")]
        public KeyCode Ulti;
        [LabelText("快捷键：技能交换(左)")]
        public KeyCode SwapSpellLeft;
        [LabelText("快捷键：技能交换(右)")]
        public KeyCode SwapSpellRight;
        [LabelText("最大生命值")]
        public float MaxHealth;
        [LabelText("当前生命值")]
        public float CurrentHealth;
        [LabelText("最大精神值")]
        public float MaxMental;
        [LabelText("当前精神值")]
        public float CurrentMental;
        [LabelText("移动速度")]
        public float MovementSpeed;
        [LabelText("旋转速度")]
        public float RotationSpeed;
        [LabelText("跳跃高度")]
        public float JumpHeight;
        [LabelText("长跳跃高度")]
        public float LongJumpHeight;
        [LabelText("奔跑速度倍率")]
        public float SprintSpeedMultiplier;
        [LabelText("空中移速倍率")]
        public float AirSpeedMultiplier;
        [LabelText("意志")]
        public float Mentality;
        [LabelText("韧性")]
        public float Resilence;
        [LabelText("冲刺距离")]
        public float DashDistance;
        [LabelText("防御时长")]
        public float DefendDuration;
        [LabelText("资源：静止动画")]
        public AnimationClip IdelAnimation;
        [LabelText("资源：走路动画")]
        public AnimationClip WalkAnimation;
        [LabelText("资源：奔跑动画")]
        public AnimationClip SprintAnimation;
        [LabelText("资源：跳跃上升动画")]
        public AnimationClip JumpRisingAnimation;
        [LabelText("资源：跳跃下降动画")]
        public AnimationClip JumpLandingAnimation;
        [LabelText("资源：着陆动画")]
        public AnimationClip JumpLandedAnimation;
        [LabelText("资源：受伤动画")]
        public AnimationClip HurtAnimation;
        [LabelText("资源：眩晕动画")]
        public AnimationClip StunAnimation;
        [LabelText("资源：防御动画")]
        public AnimationClip DefendAnimation;
        [LabelText("资源：冲刺动画")]
        public AnimationClip DashAnimation;
        [LabelText("资源：脚步声")]
        public AudioClip Footstep;
        [LabelText("资源：受伤音效")]
        public AudioClip Hurt;
        [LabelText("资源：重伤音效")]
        public AudioClip HeavilyHurt;
        [LabelText("技能列表")]
        public List<SpellConfigureBase> spellList = new List<SpellConfigureBase>();
       
    }
}