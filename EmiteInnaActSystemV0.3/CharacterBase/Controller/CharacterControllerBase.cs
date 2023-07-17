using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// �����࣬��ɫģ���ʵ�����࣬�������ģ���ͨ�ţ���Ҫ���صĻ�����ֻ��Initialize��
    /// ������ʱ������Collider��Rigidbody��ʵ���ˣ����������Ҫ�Լ�д����������д����ɡ�
    /// �����ɫ�ķ�����״̬����ʵ�֡�
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
        /// ��ɫ�ĳ�ʼ������awake�׶�ʹ�á�
        /// </summary>
        public virtual void Initialize()
        {
            rb = GetComponent<Rigidbody>();
            BindAnimationController();
        }
        /// <summary>
        /// ���ڶ�������View�㣬����Ҫ�����ӽڵ������֡�
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
        /// ע��״̬������ɫ��
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
        /// ���뵽ĳ��״̬��
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
        /// �Ƴ�ĳ��״̬��
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
        /// ע���¼�
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool RegisterCharacterEvent(string name, Action action)
        {
            return characterEvent.RegisterCharacterEvent(name, action);
        }
        /// <summary>
        /// �����¼�
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool EnableCharacterEvent(string name)
        {
            return characterEvent.EnableCharacterEvent(name);
        }
        /// <summary>
        /// �����¼�
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool DisableCharacterEvent(string name)
        {
            return characterEvent.DisableCharacterEvent(name);
        }
        /// <summary>
        /// ����һ���¼������û����ע�ᡣ
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        public void UseCharacterEvent(string name, Action action)
        {
            characterEvent.UseCharacterEvent(name, action);
        }
        /// <summary>
        /// ɾ��һ���¼�
        /// </summary>
        /// <param name="name"></param>
        public void DeleteCharacterEvent(string name)
        {
            characterEvent.DeleteCharacterEvent(name);
        }
        /// <summary>
        /// �����¼���ʵ����ڡ�
        /// </summary>
        /// <param name="name"></param>
        public void OnApplyCharacterEvent(string name)
        {
            characterEvent.ApplyCharacterEvent(name);
        }
        #endregion

        #region Animation
        /// <summary>
        /// ���Ŷ�����
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
