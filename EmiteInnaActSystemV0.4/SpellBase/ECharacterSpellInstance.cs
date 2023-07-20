using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// ��ɫʹ�ü�����ͨ���ľ�̬�ࡣ
    /// ��ʵûʲô�ã�ֻ��ͼ�����㡣
    /// </summary>
    public static class ECharacterSpellInstance
    {
        /// <summary>
        /// ��ɫʹ�ü��ܡ�
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="spell"></param>
        public static void CharacterApplySpell(CharacterControllerBase ch,SpellBase spell)
        {
            spell.OnSpellUse(ch);
        }
        /// <summary>
        /// ��ɫ���ܱ���ϡ�
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="spell"></param>
        /// <param name="eventName"></param>
        public static void CharacterCancelSpell(CharacterControllerBase ch,SpellBase spell,string eventName = "Stun")
        {
            spell.OnSpellCancel(ch, eventName);
        }
        /// <summary>
        /// ��ɫ���ܱ���ϣ���ִ�������¼���
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="spell"></param>
        /// <param name="commandName"></param>
        public static void CharacterCancelSpellWithCommandName(CharacterControllerBase ch,SpellBase spell,string commandName)
        {
            spell.OnSpellCancelWithCommandName(ch, commandName);
        }
    }
}