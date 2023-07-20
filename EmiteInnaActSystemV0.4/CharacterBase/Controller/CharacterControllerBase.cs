using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// 控制类，角色模块的实际中枢，负责各个模块的通信，需要重载的基本上只有Initialize。
    /// 物理暂时还是用Collider和Rigidbody来实现了，后面如果需要自己写物理再上手写组件吧。
    /// 具体角色的方法在状态机中实现。
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent (typeof(Collider))]
    public class CharacterControllerBase : MonoBehaviour
    {
        public Rigidbody rb;
        public CharacterAnimationController animationController;
        public CharacterConfigureBase configure;
        public CharacterEvent characterEvent=new CharacterEvent();
        public StateMachineBase currentState=null;
        public CharacterSpellBase spell=new CharacterSpellBase();
        public Dictionary<string,StateMachineBase> states = new Dictionary<string,StateMachineBase>();

        #region Initialize Functions
        /// <summary>
        /// 角色的初始化，在awake阶段使用。
        /// </summary>
        public virtual void Initialize()
        {
            rb = GetComponent<Rigidbody>();
            BindAnimationController();
            spell.InitializeSpellBase(this);
        }
        /// <summary>
        /// 由于动画机在View层，所以要遍历子节点来发现。
        /// </summary>
        public virtual bool BindAnimationController()
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                CharacterAnimationController c = transform.GetChild(i).gameObject.GetComponent<CharacterAnimationController>();
                if (c != null)
                {
                    c.Initialize(this);
                    return true ;
                }
            }
            return false;
        }
        /// <summary>
        /// 从配置文件中获得技能，记得这个要在configure绑定之后用。
        /// </summary>
        public virtual void GetSpellsFromConfigure()
        {
            if (configure == null) return;
            foreach(SpellConfigureBase i in configure.spellList)
            {
                spell.RegisterSpell(i.Name,i);
                Debug.Log("成功读取技能" + i.SpellName+" 使用名为"+i.Name);
                //TODO:到底要不要设置启用呢
                spell.SetSpellActive(i.Name,true);
            }

        }
        #endregion


        #region StateMachine
        /// <summary>
        /// 注册状态机到角色。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="state"></param>
        public void RegisterStateMachine(string name,StateMachineBase state)
        {
            if (!states.ContainsKey(name))
            {
                states.Add(name, state);
                state.controller = this;
            }
        }
        /// <summary>
        /// 进入到某个状态。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool EnterState(string name)
        {
            if (states.ContainsKey(name))
            {
                if (currentState != null) currentState.OnLeftState();
                StateMachineBase state = states[name];
                state.OnEnterState();
                currentState = state;
            }
            return false;
        }
        /// <summary>
        /// 移除某个状态。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool DeleteState(string name)
        {
            if (states.ContainsKey(name))
            {
                states.Remove(name);
                return true;
            }
            return false;
        }
        #endregion

        #region Event
        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool RegisterCharacterEvent(string name, Action action)
        {
            return characterEvent.RegisterCharacterEvent(name, action);
        }
        #region 重载
        public bool RegisterCharacterEvent<E>(string name, Action<E> action)
        {
            return characterEvent.RegisterCharacterEvent<E>(name, action);
        }
        public bool RegisterCharacterEvent<E,M>(string name, Action<E,M> action)
        {
            return characterEvent.RegisterCharacterEvent<E,M>(name, action);
        }
        public bool RegisterCharacterEvent<E, M,I>(string name, Action<E, M,I> action)
        {
            return characterEvent.RegisterCharacterEvent<E, M,I>(name, action);
        }
        public bool RegisterCharacterEvent<E, M, I,T>(string name, Action<E, M, I,T> action)
        {
            return characterEvent.RegisterCharacterEvent<E, M, I,T>(name, action);
        }
        public bool RegisterCharacterEvent<E, M, I, T,N>(string name, Action<E, M, I, T,N> action)
        {
            return characterEvent.RegisterCharacterEvent<E, M, I, T,N>(name, action);
        }
        public bool RegisterCharacterEvent<E, M, I, T, N,A>(string name, Action<E, M, I, T, N,A> action)
        {
            return characterEvent.RegisterCharacterEvent<E, M, I, T, N,A>(name, action);
        }
        #endregion
        /// <summary>
        /// 启用事件，已弃用
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        //public bool EnableCharacterEvent(string name)
        //{
        //    return characterEvent.EnableCharacterEvent(name);
        //}
        /// <summary>
        /// 禁用事件，已弃用
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        //public bool DisableCharacterEvent(string name)
        //{
        //    return characterEvent.DisableCharacterEvent(name);
        //}
        /// <summary>
        /// 启用一个事件，如果没有则注册。
        /// 已弃用
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        //public void UseCharacterEvent(string name, Action action)
        //{
        //    characterEvent.UseCharacterEvent(name, action);
        //}
        /// <summary>
        /// 删除一个事件
        /// </summary>
        /// <param name="name"></param>
        public void DeleteCharacterEvent(string name)
        {
            characterEvent.DeleteCharacterEvent(name);
        }
        /// <summary>
        /// 调用事件的实际入口。
        /// </summary>
        /// <param name="name"></param>
        public void OnApplyCharacterEvent(string name)
        {
            if (ConfigureInstance.GetValue<EmiteInnaBool>("uniform", "EventDebug").Value)
                Debug.Log(name + " 触发事件 "+name);
            characterEvent.ApplyCharacterEvent(name);
        }
        #region 重载
        public void OnApplyCharacterEvent<E>(string name,E arg1)
        {
            if (ConfigureInstance.GetValue<EmiteInnaBool>("uniform", "EventDebug").Value)
                Debug.Log(name + " 触发事件 " + name);
            characterEvent.ApplyCharacterEvent<E>(name,arg1);
        }
        public void OnApplyCharacterEvent<E,M>(string name, E arg1,M arg2)
        {
            if (ConfigureInstance.GetValue<EmiteInnaBool>("uniform", "EventDebug").Value)
                Debug.Log(name + " 触发事件 " + name);
            characterEvent.ApplyCharacterEvent<E,M>(name, arg1,arg2);
        }
        public void OnApplyCharacterEvent<E, M,I>(string name, E arg1, M arg2,I arg3)
        {
            if (ConfigureInstance.GetValue<EmiteInnaBool>("uniform", "EventDebug").Value)
                Debug.Log(name + " 触发事件 " + name);
            characterEvent.ApplyCharacterEvent<E, M,I>(name, arg1, arg2,arg3);
        }
        public void OnApplyCharacterEvent<E, M, I,T>(string name, E arg1, M arg2, I arg3,T arg4)
        {
            if (ConfigureInstance.GetValue<EmiteInnaBool>("uniform", "EventDebug").Value)
                Debug.Log(name + " 触发事件 " + name);
            characterEvent.ApplyCharacterEvent<E, M, I,T>(name, arg1, arg2, arg3,arg4);
        }
        public void OnApplyCharacterEvent<E, M, I,T,N>(string name, E arg1, M arg2, I arg3,T arg4,N arg5)
        {
            if (ConfigureInstance.GetValue<EmiteInnaBool>("uniform", "EventDebug").Value)
                Debug.Log(name + " 触发事件 " + name);
            characterEvent.ApplyCharacterEvent<E, M, I,T,N>(name, arg1, arg2, arg3,arg4,arg5);
        }
        public void OnApplyCharacterEvent<E, M, I, T, N,A>(string name, E arg1, M arg2, I arg3, T arg4, N arg5,A arg6)
        {
            if (ConfigureInstance.GetValue<EmiteInnaBool>("uniform", "EventDebug").Value)
                Debug.Log(name + " 触发事件 " + name);
            characterEvent.ApplyCharacterEvent<E, M, I, T, N,A>(name, arg1, arg2, arg3, arg4, arg5,arg6);
        }
        #endregion

        #endregion

        #region Animation
        /// <summary>
        /// 播放动画。
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="transition"></param>
        /// <param name="speed"></param>
        public void PlayAnimation(AnimationClip clip,float transition=0f,float speed = 1f)
        {
            animationController.PlayAnimation(clip,transition,speed);
            
        }
        #endregion

        #region Functions
        public void CharacterMove(Vector3 direction, float movementSpeed, float rotationSpeed, float accelerate = 0.5f)
        {
            EChara.CharacterMove(rb, direction, movementSpeed, rotationSpeed);
        }
        public void CharacterJump(float jumpHeight)
        {
            EChara.CharacterJump(rb,jumpHeight);
        }
        #endregion

        #region Spell
        /// <summary>
        /// 使用技能。
        /// </summary>
        /// <param name="name"></param>
        public void CharacterApplySpell(string name)
        {
            spell.ApplySpell(name);
        }
        /// <summary>
        /// 设置某个技能是否启用
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void CharacterSetSpellActive(string name, bool value)
        {
            spell.SetSpellActive(name, value);
        }
        /// <summary>
        /// 取消当前释放的技能(通过事件名)
        /// </summary>
        /// <param name="eventName"></param>
        public void CharacterCancelSpell(string eventName)
        {
            if (spell.currentSpell != null)
                spell.currentSpell.OnSpellCancel(this, eventName);
        }
        /// <summary>
        /// 取消当前释放的技能(通过命令名)
        /// </summary>
        /// <param name="eventName"></param>
        public void CharacterCancelSpellWithCommand(string commandName)
        {
            if (spell.currentSpell != null)
                spell.currentSpell.OnSpellCancelWithCommandName(this, commandName);
        }
        /// <summary>
        /// 启用可打断事件。
        /// </summary>
        /// <param name="e"></param>
        public void CharacterStartInterruptableCoroutine(IEnumerator e)
        {
            if (spell.currentSpell != null)
                spell.currentSpell.StartInterrupatbleCoroutine(this, e);
        }
        #endregion

        #region 自带事件
        /// <summary>
        /// 进入静置状态
        /// </summary>
        public void EnterIdle()
        {
            EnterState("Idle");
        }
        /// <summary>
        /// 进入眩晕状态
        /// </summary>
        public void EnterCasting()
        {
            EnterState("Casting");
        }
        /// <summary>
        /// 进入受伤状态，time秒后回到idle
        /// </summary>
        /// <param name="time"></param>
        public void EnterHurt(float time)
        {
            EnterState("Hurt");
            if(currentState is UniformHurtState)
            {
                (currentState as UniformHurtState).timer = time;
                (currentState as UniformHurtState).starttocount = false;
            }
        }
        /// <summary>
        /// 进入受伤状态，持续time秒
        /// </summary>
        /// <param name="time"></param>
        public void EnterStun(float time)
        {
            EnterState("Stun");
            if (currentState is UniformStunState)
            {
                (currentState as UniformStunState).timer = time;
                (currentState as UniformStunState).starttocount = false;
            }
        }
        public virtual void ApplyAreaAttack(SpellAttackEvent eventData)
        {

        }
        #endregion
        public virtual void Awake()
        {
            Initialize();
        }
        public virtual void Update()
        {
            if(currentState != null)currentState.OnUpdateState();
            if (spell != null) spell.UpdateCharacterSpell();
        }
        public virtual void FixedUpdate()
        {
            if (currentState != null) currentState.OnFixedUpdateState();
            if (spell != null) spell.FixedUpdateCharacterSpell();
        }
        public virtual void OnDestroy()
        {
            currentState = null;
            configure = null;
            characterEvent = null;
            states.Clear();
            states = null;
            animationController.Destroy();
            animationController = null;
            spell.OnDestroy();
        }
        /// <summary>
        /// 技能debug用的框框
        /// </summary>
        public void OnDrawGizmos()
        {
            if (spell != null)
            {
                if(spell.spells != null)
                {
                    foreach(KeyValuePair<string,SpellBase> sp in spell.spells)
                    {
                        if (sp.Value.config.attackEvents != null)
                        {
                            foreach(SpellAttackEvent e in sp.Value.config.attackEvents)
                            {
                                if (e.showDebugGizmos == false) continue;
                                Gizmos.color = Color.yellow;
                                float maxBound = 12f * Mathf.Max(e.extends.x, Mathf.Max(e.extends.y, e.extends.z));
                                Gizmos.DrawWireCube(transform.position,new Vector3(maxBound, maxBound, maxBound));


                                if (e.areaType == AreaType.CUBE)
                                {
                                    Vector4 center = e.centerOffset;
                                    Vector4 p1 = center + new Vector4(e.extends.x, e.extends.y, e.extends.z,1);
                                    Vector4 p2 = center + new Vector4(-e.extends.x, e.extends.y, e.extends.z, 1);
                                    Vector4 p3 = center + new Vector4(e.extends.x,- e.extends.y, e.extends.z, 1);
                                    Vector4 p4 = center + new Vector4(e.extends.x, e.extends.y, -e.extends.z, 1);
                                    Vector4 p5 = center + new Vector4(-e.extends.x,- e.extends.y, e.extends.z, 1);
                                    Vector4 p6 = center + new Vector4(-e.extends.x, e.extends.y,- e.extends.z, 1);
                                    Vector4 p7 = center + new Vector4(e.extends.x, -e.extends.y, -e.extends.z, 1);
                                    Vector4 p8 = center + new Vector4(-e.extends.x,- e.extends.y,- e.extends.z, 1);
                                    p1 = transform.localToWorldMatrix * p1;
                                    p2 = transform.localToWorldMatrix * p2;
                                    p3 = transform.localToWorldMatrix * p3;
                                    p4 = transform.localToWorldMatrix * p4;
                                    p5 = transform.localToWorldMatrix * p5;
                                    p6 = transform.localToWorldMatrix * p6;
                                    p7 = transform.localToWorldMatrix * p7;
                                    p8 = transform.localToWorldMatrix * p8;
                                    Gizmos.color = Color.green;
                                    Gizmos.DrawLine(p1, p2);
                                    Gizmos.DrawLine(p1, p3);
                                    Gizmos.DrawLine(p1, p4);
                                    Gizmos.DrawLine(p2, p6);
                                    Gizmos.DrawLine(p2, p5);
                                    Gizmos.DrawLine(p3, p5);
                                    Gizmos.DrawLine(p3, p7);
                                    Gizmos.DrawLine(p4, p6);
                                    Gizmos.DrawLine(p4, p7);
                                    Gizmos.DrawLine(p5, p8);
                                    Gizmos.DrawLine(p6, p8);
                                    Gizmos.DrawLine(p7, p8);
                                }else if (e.areaType == AreaType.ELLIPSE)
                                {
                                    Gizmos.color = Color.green;
                                    int delta = 10;
                                    Vector4 center = e.centerOffset;
                                    if (e.angle.y >= 180)
                                    {
                                        for (int i = 0; i < 360; i+=delta)
                                        {
                                            float degnow = (float)i / 360 * 2 * Mathf.PI;
                                            float degnext = (float)(i + delta) / 360 * 2 * Mathf.PI;
                                            Vector4 pnow = center + new Vector4(-e.extends.x * Mathf.Sin(degnow), e.extends.y, e.extends.z * Mathf.Cos(degnow), 1);
                                            Vector4 pnext = center + new Vector4(-e.extends.x * Mathf.Sin(degnext), e.extends.y, e.extends.z * Mathf.Cos(degnext), 1);
                                            pnow = transform.localToWorldMatrix * pnow;
                                            pnext = transform.localToWorldMatrix * pnext;
                                            Gizmos.DrawLine(pnow, pnext);
                                            pnow = center + new Vector4(-e.extends.x * Mathf.Sin(degnow), -e.extends.y, e.extends.z * Mathf.Cos(degnow), 1);
                                            pnext = center + new Vector4(-e.extends.x * Mathf.Sin(degnext), -e.extends.y, e.extends.z * Mathf.Cos(degnext), 1);
                                            pnow = transform.localToWorldMatrix * pnow;
                                            pnext = transform.localToWorldMatrix * pnext;
                                            Gizmos.DrawLine(pnow, pnext);
                                        }
                                    }
                                    else
                                    {
                                        delta = (int)(e.angle.y/18);
                                        if (delta == 0) delta++;
                                        Vector4 pcBtm = center + new Vector4(0, -e.extends.y, 0, 1);
                                        Vector4 pcTop = center + new Vector4(0, e.extends.y, 0, 1);
                                        pcBtm = transform.localToWorldMatrix * pcBtm;
                                        pcTop = transform.localToWorldMatrix * pcTop;
                                        bool flg = false;
                                        Vector4 psBtm=Vector4.one, psTop = Vector4.one, peBtm = Vector4.one, peTop = Vector4.one;
                                        for (int i =(int)(e.angle.x-e.angle.y); i < (int)(e.angle.x + e.angle.y); i+=delta)
                                        {
                                          //  float down = e.angle.x - e.angle.y;
                                          ////  if (down < 0) down += 360;
                                          //  float up = e.angle.x + e.angle.y;
                                          ////  if (up >= 360) up -= 360;
                                          //  if ((i < down || i + delta >up)&&(i-360<down||i+delta-360>up)) continue;
                                            

                                            float degnow = (float)i / 360 * 2 * Mathf.PI;
                                            float degnext = (float)(i + delta) / 360 * 2 * Mathf.PI;


                                            Vector4 pnow = center + new Vector4(-e.extends.x * Mathf.Sin(degnow), e.extends.y, e.extends.z * Mathf.Cos(degnow),1);
                                            Vector4 pnext = center + new Vector4(-e.extends.x * Mathf.Sin(degnext), e.extends.y, e.extends.z * Mathf.Cos(degnext),1);
                                            pnow = transform.localToWorldMatrix * pnow;
                                            pnext = transform.localToWorldMatrix * pnext;
                                            if (!flg)
                                            {
                                                psTop = pnow;
                                            }
                                            peTop = pnext;
                                            Gizmos.DrawLine(pnow, pnext);
                                            pnow = center + new Vector4(-e.extends.x * Mathf.Sin(degnow), -e.extends.y, e.extends.z * Mathf.Cos(degnow),1);
                                            pnext = center + new Vector4(-e.extends.x * Mathf.Sin(degnext), -e.extends.y, e.extends.z * Mathf.Cos(degnext),1);
                                            pnow = transform.localToWorldMatrix * pnow;
                                            pnext = transform.localToWorldMatrix * pnext;
                                            if (!flg)
                                            {
                                                psBtm = pnow;
                                                flg = true;
                                            }
                                            peBtm = pnext;
                                            Gizmos.DrawLine(pnow, pnext);
                                            
                                        }
                                        //Debug.Log(psTop + " " + psBtm + " " + peTop + " " + peBtm);
                                        Gizmos.DrawLine(pcTop, pcBtm);
                                        Gizmos.DrawLine(pcTop, psTop);
                                        Gizmos.DrawLine(psTop, psBtm);
                                        Gizmos.DrawLine(pcTop, peTop);
                                        Gizmos.DrawLine(pcBtm, psBtm);
                                        Gizmos.DrawLine(pcBtm, peBtm);
                                        Gizmos.DrawLine(peTop, peBtm);
                                    }

                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
