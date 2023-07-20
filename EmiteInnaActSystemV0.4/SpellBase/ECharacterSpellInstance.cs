using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// 角色使用技能所通过的静态类。
    /// 其实没什么用，只是图个方便。
    /// </summary>
    public static class ECharacterSpellInstance
    {
        /// <summary>
        /// 角色使用技能。
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="spell"></param>
        public static void CharacterApplySpell(CharacterControllerBase ch,SpellBase spell)
        {
            spell.OnSpellUse(ch);
        }
        /// <summary>
        /// 角色技能被打断。
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="spell"></param>
        /// <param name="eventName"></param>
        public static void CharacterCancelSpell(CharacterControllerBase ch,SpellBase spell,string eventName = "Stun")
        {
            spell.OnSpellCancel(ch, eventName);
        }
        /// <summary>
        /// 角色技能被打断，并执行命令事件。
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