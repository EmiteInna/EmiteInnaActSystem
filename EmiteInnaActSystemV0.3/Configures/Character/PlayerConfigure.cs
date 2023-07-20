using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT
{
    /// <summary>
    /// Player的Configure，比起基础的CharacterConfigure多一些东西。
    /// </summary>
    [CreateAssetMenu(fileName = "NewPlayerConfigure", menuName = "EmiteInnaACT/PlayerConfigure", order = 5)]
    public class PlayerConfigure : CharacterConfigureBase
    {
        public KeyCode BaseAttack;

    }
}