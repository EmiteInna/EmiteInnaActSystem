using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmiteInnaACT{
    public class GolemEnemyController : EnemyController
    {
        public CharacterConfigureBase golemconfigure;
        public override void Initialize()
        {
            base.Initialize();
            this.configure=golemconfigure;
            GolemIdle idle = new GolemIdle();
            GolemHurt hurt = new GolemHurt();
            RegisterStateMachine("Idle", idle);
            RegisterStateMachine("Hurt", hurt);
            EnterState("Idle");
            RegisterCharacterEvent<float>("LightAttack",OnLightAttacked);
            GetSpellsFromConfigure();
            //  CharacterSetSpellActive("dashdown", true);
            //  CharacterSetSpellActive("spin", true);

        }
        public void OnLightAttacked(float dmg)
        {
            Debug.Log("±ª¥Ú¡À");
            EnterHurt(2f);
        }
    }
}