using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace EmiteInnaACT
{
   

    [CreateAssetMenu(fileName = "NewConfigure", menuName = "EmiteInnaACT/Configure", order = 2)]
    public class ConfigureBase : SerializedScriptableObject
    {
        [SerializeField]
        protected Dictionary<string, object> configureList=new Dictionary<string, object>();
        /// <summary>
        /// 原则上配置表只可以读取，不可以更改。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetValue<T>(string key)
        {
            if(configureList.TryGetValue(key,out object value))
            {
                return (T)value;
            }
            else
            {
                return default(T);
            }
        }
    }
    
    public class EmiteInnaBool
    {
        bool value;
        [OdinSerialize]
        public bool Value { get => value; set => this.value = value; }
    }
}