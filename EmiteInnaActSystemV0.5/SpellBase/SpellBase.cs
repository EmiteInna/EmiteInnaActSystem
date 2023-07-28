using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// 技能的基类，提供一些技能对象的方法，需要传入CharacterController才能进行使用。
    /// 感觉也不会有派生类了，这个东西。
    /// </summary>
    public class SpellBase
    {
        public float currentCD;
        public bool active;
        public Stack<Coroutine> stack = new Stack<Coroutine>();
        public SpellConfigureBase config;
        /// <summary>
        /// 构造函数，对一个技能来说，技能配置是必须具备的。
        /// </summary>
        /// <param name="config"></param>
        public SpellBase(SpellConfigureBase config)
        {
            this.config = config;
            currentCD = 0;
            active = false;
        }
        /// <summary>
        /// 设置技能是否启用，只有启用的技能才能被使用，以及更新CD。
        /// </summary>
        /// <param name="value"></param>
        public void SetActive(bool value)
        {
            active = value;
        }
        /// <summary>
        /// 技能的fixupdate，默认功能是更新CD。
        /// </summary>
        public virtual void OnSpellFixedUpdate()
        {
            if (currentCD > 0) currentCD -= Time.fixedDeltaTime;
        }
        /// <summary>
        /// 技能的update。
        /// </summary>
        public virtual void OnSpellUpdate()
        {

        }
        /// <summary>
        /// 进入CD，注意调用的时机。
        /// </summary>
        public void EnterCooldown()
        {
            currentCD = config.SpellCoolDown;
        }
        /// <summary>
        /// 使用技能。
        /// </summary>
        /// <param name="ch"></param>
        public virtual void OnSpellUse(CharacterControllerBase ch)
        {
            if (!active) return;
            if (currentCD > 0) return;
            ClearCoroutineStack();
            StartInterrupatbleCoroutine(ch, (DoSpellUse(ch)));
            //TODO:判断蓝耗
        }
        /// <summary>
        /// 使用事件列表里的事件来打断技能。
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="commandName"></param>
        public virtual void OnSpellCancelWithCommandName(CharacterControllerBase ch, string commandName)
        {
            if (config.cancelEvents.TryGetValue(commandName, out string eventName))
            {
                OnSpellCancel(ch, eventName);
            }
        }
        /// <summary>
        /// 技能被打断时触发的方法。
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="cancelCommand"></param>
        public virtual void OnSpellCancel(CharacterControllerBase ch, string cancelCommand = "Stun")
        {
            if (ConfigureInstance.GetValue<EmiteInnaBool>("uniform", "SpellDebug").Value)
                Debug.Log("打断技能" + config.SpellName);
            while (stack.Count > 0)
            {
                Coroutine co = stack.Peek();
                ch.StopCoroutine(co);
                stack.Pop();
            }
            ch.OnApplyCharacterEvent(cancelCommand);
        }
        /// <summary>
        /// 清空协程栈。
        /// </summary>
        public void ClearCoroutineStack()
        {
            while (stack.Count > 0) stack.Pop();
        }
        /// <summary>
        /// 开始一个会被打断的协程，当技能被打断时，这些协程也会被打断。
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="c"></param>
        public void StartInterrupatbleCoroutine(CharacterControllerBase ch, IEnumerator c)
        {
            Coroutine co = ch.StartCoroutine(c);
            stack.Push(co);
        }
        /// <summary>
        /// 实际使用的协程，注意调用顺序。
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        IEnumerator DoSpellUse(CharacterControllerBase ch)
        {
            float timer = 0;
            int idx_Animation = 0;
            int idx_Script = 0;
            int idx_Audio = 0;
            int idx_Attack = 0;
            if (ConfigureInstance.GetValue<EmiteInnaBool>("uniform", "SpellDebug").Value)
                Debug.Log("使用技能" + config.SpellName);
            while (timer <= config.duration * config.timeMultiplier)
            {
                while (idx_Animation < config.animationEvent.Count)
                {
                    if (timer >= config.animationEvent[idx_Animation].happenTime * config.timeMultiplier)
                    {
                        ch.PlayAnimation(config.animationEvent[idx_Animation].clip, 0.05f, 1 / config.timeMultiplier);
                        idx_Animation++;
                    }
                    else break;
                }
                while (idx_Script < config.scriptEvents.Count)
                {
                    if (timer >= config.scriptEvents[idx_Script].happenTime * config.timeMultiplier)
                    {
                        ch.OnApplyCharacterEvent(config.scriptEvents[idx_Script].eventName);
                        idx_Script++;
                    }
                    else break;
                }
                while (idx_Audio < config.audioEvents.Count)
                {
                    if (timer >= config.audioEvents[idx_Audio].happenTime * config.timeMultiplier)
                    {
                        ESoundInstance.PlaySFX(config.audioEvents[idx_Audio].clip, ch.transform.position +
                            config.audioEvents[idx_Audio].offset, config.audioEvents[idx_Audio].clip.length,
                            config.audioEvents[idx_Audio].playVolume, config.audioEvents[idx_Audio].playPitch / config.timeMultiplier);
                        idx_Audio++;
                    }
                    else break;
                }
                while (idx_Attack < config.attackEvents.Count)
                {
                    if (timer >= config.attackEvents[idx_Attack].happenTime * config.timeMultiplier)
                    {
                        ch.ApplyAreaAttack(config.attackEvents[idx_Attack]);
                        idx_Attack++;
                    }
                    else break;
                }
                timer += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
        }
    }
}