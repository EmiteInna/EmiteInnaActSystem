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
            EPoolInstance.Initialize();
            ESoundInstance.Initialize();
            Debug.Log("EmiteInnaCoreº”‘ÿÕÍ±œ");
        }
    }
}