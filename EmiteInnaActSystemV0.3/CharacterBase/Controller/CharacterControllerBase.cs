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
        public Dictionary<string,StateMachineBase> states = new Dictionary<string,StateMachineBase>();

        #region Initialize Functions
        /// <summary>
        /// 角色的初始化，在awake阶段使用。
        /// </summary>
        public virtual void Initialize()
        {
            rb = GetComponent<Rigidbody>();
            BindAnimationController();
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
        /// <summary>
        /// 启用事件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool EnableCharacterEvent(string name)
        {
            return characterEvent.EnableCharacterEvent(name);
        }
        /// <summary>
        /// 禁用事件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool DisableCharacterEvent(string name)
        {
            return characterEvent.DisableCharacterEvent(name);
        }
        /// <summary>
        /// 启用一个事件，如果没有则注册。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        public void UseCharacterEvent(string name, Action action)
        {
            characterEvent.UseCharacterEvent(name, action);
        }
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
            characterEvent.ApplyCharacterEvent(name);
        }
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
        public virtual void Awake()
        {
            Initialize();
        }
        public virtual void Update()
        {
            if(currentState != null)currentState.OnUpdateState();
        }
        public virtual void FixedUpdate()
        {
            if (currentState != null) currentState.OnFixedUpdateState();
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
        }
    }
}
