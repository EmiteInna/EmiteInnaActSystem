using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// ���ܵĻ��࣬�ṩһЩ���ܶ���ķ�������Ҫ����CharacterController���ܽ���ʹ�á�
    /// �о�Ҳ�������������ˣ����������
    /// </summary>
    public class SpellBase
    {
        public float currentCD;
        public bool active;
        public Stack<Coroutine> stack = new Stack<Coroutine>();
        public SpellConfigureBase config;
        /// <summary>
        /// ���캯������һ��������˵�����������Ǳ���߱��ġ�
        /// </summary>
        /// <param name="config"></param>
        public SpellBase(SpellConfigureBase config)
        {
            this.config = config;
            currentCD = 0;
            active = false;
        }
        /// <summary>
        /// ���ü����Ƿ����ã�ֻ�����õļ��ܲ��ܱ�ʹ�ã��Լ�����CD��
        /// </summary>
        /// <param name="value"></param>
        public void SetActive(bool value)
        {
            active = value;
        }
        /// <summary>
        /// ���ܵ�fixupdate��Ĭ�Ϲ����Ǹ���CD��
        /// </summary>
        public virtual void OnSpellFixedUpdate()
        {
            if (currentCD > 0) currentCD -= Time.fixedDeltaTime;
        }
        /// <summary>
        /// ���ܵ�update��
        /// </summary>
        public virtual void OnSpellUpdate()
        {

        }
        /// <summary>
        /// ����CD��ע����õ�ʱ����
        /// </summary>
        public void EnterCooldown()
        {
            currentCD = config.SpellCoolDown;
        }
        /// <summary>
        /// ʹ�ü��ܡ�
        /// </summary>
        /// <param name="ch"></param>
        public virtual void OnSpellUse(CharacterControllerBase ch)
        {
            if (!active) return;
            if (currentCD > 0) return;
            ClearCoroutineStack();
            StartInterrupatbleCoroutine(ch, (DoSpellUse(ch)));
            //TODO:�ж�����
        }
        /// <summary>
        /// ʹ���¼��б�����¼�����ϼ��ܡ�
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
        /// ���ܱ����ʱ�����ķ�����
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="cancelCommand"></param>
        public virtual void OnSpellCancel(CharacterControllerBase ch, string cancelCommand = "Stun")
        {
            if (ConfigureInstance.GetValue<EmiteInnaBool>("uniform", "SpellDebug").Value)
                Debug.Log("��ϼ���" + config.SpellName);
            while (stack.Count > 0)
            {
                Coroutine co = stack.Peek();
                ch.StopCoroutine(co);
                stack.Pop();
            }
            ch.OnApplyCharacterEvent(cancelCommand);
        }
        /// <summary>
        /// ���Э��ջ��
        /// </summary>
        public void ClearCoroutineStack()
        {
            while (stack.Count > 0) stack.Pop();
        }
        /// <summary>
        /// ��ʼһ���ᱻ��ϵ�Э�̣������ܱ����ʱ����ЩЭ��Ҳ�ᱻ��ϡ�
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="c"></param>
        public void StartInterrupatbleCoroutine(CharacterControllerBase ch, IEnumerator c)
        {
            Coroutine co = ch.StartCoroutine(c);
            stack.Push(co);
        }
        /// <summary>
        /// ʵ��ʹ�õ�Э�̣�ע�����˳��
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
                Debug.Log("ʹ�ü���" + config.SpellName);
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