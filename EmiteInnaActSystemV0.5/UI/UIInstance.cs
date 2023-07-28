using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EmiteInnaACT
{
    public static class UIInstance
    {
        public static GameUIController PlayerGameUIController;
        public static void Initialize()
        {
            PlayerGameUIController = GameObject.Find("PlayerGameUI").GetComponent<GameUIController>();
            if (PlayerGameUIController == null)
            {
                Debug.LogError("未找到PlayerGameUIController！");
                return;
            }
            //PlayerController player = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<PlayerController>();
            //player.BindGameUIController(PlayerGameUIController);
        }
    }
}