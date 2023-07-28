using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    public class EmiteInnaACTCore : MonoBehaviour
    {
        public static EmiteInnaACTCore Instance;
        public ConfigureService conf;
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            ConfigureInstance.InitializeConfigureService(conf);
            UIInstance.Initialize();
            EPoolInstance.Initialize();
            ESoundInstance.Initialize();
            Debug.Log("EmiteInnaCore�������");
            GameObject illusion = ConfigureInstance.GetValue<GameObject>("danmaku", "illusion");
            EPoolInstance.CreateObjectPool<GameObject>("Illusions", 50, 25, true, illusion);
            EPoolInstance.CreateObjectPool<Mesh>("NewMesh", 100, 100, false, null);
            Debug.Log("����ش������");
        }
        private void OnDestroy()
        {
            EPoolInstance.DestroyAll();
        }
    }
}