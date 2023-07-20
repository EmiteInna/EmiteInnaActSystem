using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// Player��Configure�����������CharacterConfigure��һЩ������
    /// </summary>
    [CreateAssetMenu(fileName = "NewPlayerConfigure", menuName = "EmiteInnaACT/PlayerConfigure", order = 5)]
    public class PlayerConfigure : CharacterConfigureBase
    {
        public KeyCode BaseAttack;

    }
}