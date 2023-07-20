using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// 角色的技能列表基类，包含技能的注册调用方法。
    /// </summary>
    public class CharacterSpellBase
    {

        public Dictionary<string, SpellBase> spells = new Dictionary<string, SpellBase>();
        public SpellBase currentSpell;
        public CharacterControllerBase controller;
        /// <summary>
        /// 绑定。
        /// </summary>
        /// <param name="controller"></param>
        public void InitializeSpellBase(CharacterControllerBase controller)
        {
            this.controller = controller;
        }
        /// <summary>
        /// 注册技能（通过技能）
        /// </summary>
        /// <param name="name"></param>
        /// <param name="spell"></param>
        public virtual void RegisterSpell(string name,SpellBase spell)
        {
            if (spells.ContainsKey(name))
            {
                Debug.Log(name + "已经被注册过同名技能了！");
                return;
            }
            spells.Add(name, spell);
        }
        /// <summary>
        /// 注册技能（通过技能配置）
        /// </summary>
        /// <param name="name"></param>
        /// <param name="spellConfig"></param>
        public virtual void RegisterSpell(string name,SpellConfigureBase spellConfig)
        {
            RegisterSpell(name, new SpellBase(spellConfig));
        }
        /// <summary>
        /// 使用技能。
        /// </summary>
        /// <param name="name"></param>
        public virtual void ApplySpell(string name)
        {
            if (!spells.ContainsKey(name))
            {
                Debug.Log("技能不存在");
                return;
            }
            currentSpell = spells[name];
            spells[name].OnSpellUse(controller);
        }
        /// <summary>
        /// 更新技能update
        /// </summary>
        public virtual void UpdateCharacterSpell()
        {
            foreach(KeyValuePair<string,SpellBase> sp in spells)
            {
                if(sp.Value.active)
                    sp.Value.OnSpellUpdate();
            }
        }
        /// <summary>
        /// 更新技能fixedUpdate
        /// </summary>
        public virtual void FixedUpdateCharacterSpell()
        {
            foreach (KeyValuePair<string, SpellBase> sp in spells)
            {
                if (sp.Value.active)
                    sp.Value.OnSpellFixedUpdate();
            }
        }
        /// <summary>
        /// 设置技能是否被启用
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public virtual void SetSpellActive(string name,bool value)
        {
            if (spells.ContainsKey(name))
            {
                spells[name].SetActive(value);
            }
        }
        public virtual void OnDestroy()
        {
            currentSpell = null;
            spells.Clear();
            spells = null;
            controller = null;
        }
    }
}