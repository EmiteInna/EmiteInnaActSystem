using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// ��ɫ�ļ����б���࣬�������ܵ�ע����÷�����
    /// </summary>
    public class CharacterSpellBase
    {

        public Dictionary<string, SpellBase> spells = new Dictionary<string, SpellBase>();
        public SpellBase currentSpell;
        public CharacterControllerBase controller;
        /// <summary>
        /// �󶨡�
        /// </summary>
        /// <param name="controller"></param>
        public void InitializeSpellBase(CharacterControllerBase controller)
        {
            this.controller = controller;
        }
        /// <summary>
        /// ע�Ἴ�ܣ�ͨ�����ܣ�
        /// </summary>
        /// <param name="name"></param>
        /// <param name="spell"></param>
        public virtual void RegisterSpell(string name,SpellBase spell)
        {
            if (spells.ContainsKey(name))
            {
                Debug.Log(name + "�Ѿ���ע���ͬ�������ˣ�");
                return;
            }
            spells.Add(name, spell);
        }
        /// <summary>
        /// ע�Ἴ�ܣ�ͨ���������ã�
        /// </summary>
        /// <param name="name"></param>
        /// <param name="spellConfig"></param>
        public virtual void RegisterSpell(string name,SpellConfigureBase spellConfig)
        {
            RegisterSpell(name, new SpellBase(spellConfig));
        }
        /// <summary>
        /// ʹ�ü��ܡ�
        /// </summary>
        /// <param name="name"></param>
        public virtual void ApplySpell(string name)
        {
            if (!spells.ContainsKey(name))
            {
                Debug.Log("���ܲ�����");
                return;
            }
            currentSpell = spells[name];
            spells[name].OnSpellUse(controller);
        }
        /// <summary>
        /// ���¼���update
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
        /// ���¼���fixedUpdate
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
        /// ���ü����Ƿ�����
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