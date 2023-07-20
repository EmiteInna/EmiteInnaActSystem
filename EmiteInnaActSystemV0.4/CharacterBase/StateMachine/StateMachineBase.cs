using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// 角色状态机的基类，提供一些虚方法，本身不能直接使用。
    /// 这么看来这好像其实是个接口吧。
    /// 四个方法都需要重载，不然没有内容。
    /// </summary>
    public abstract class StateMachineBase
    {
        public CharacterControllerBase controller;
        /// <summary>
        /// 为状态机绑定一个controller，便于获取角色信息。
        /// </summary>
        /// <param name="controller"></param>
        public void BindController(CharacterControllerBase controller)
        {
            this.controller = controller;
        }
        /// <summary>
        /// 进入该状态时调用的方法
        /// </summary>
        public virtual void OnEnterState()
        {

        }
        /// <summary>
        /// 离开该状态时调用的方法
        /// </summary>
        public virtual void OnLeftState()
        {

        }
        /// <summary>
        /// 处于该状态时的update
        /// </summary>
        public virtual void OnUpdateState()
        {

        }
        /// <summary>
        /// 处于该状态时的fixedUpdate
        /// </summary>
        public virtual void OnFixedUpdateState()
        {

        }
    }
}