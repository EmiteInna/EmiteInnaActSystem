using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace EmiteInnaACT
{
    public class PlayerController : CharacterControllerBase
    {

        //暂时用public来赋值，之后会用assetbundle，注意关心是否需要实例化配置。
        public PlayerConfigure playerConfigure;

        /// <summary>
        /// 我们先想想这个initialize里应该initialize些什么。
        /// 配置表、状态机、事件注册，还有呢？
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this.configure = playerConfigure;
            PlayerIdelState idle = new PlayerIdelState();
            PlayerWalkState walk = new PlayerWalkState();
            PlayerSprinttate sprint = new PlayerSprinttate();
            PlayerJumpRisingState jumpRising = new PlayerJumpRisingState();
            PlayerJumpLandingState jumpLanding = new PlayerJumpLandingState();
            PlayerLandedState landed = new PlayerLandedState();
            PlayerCastingState casting = new PlayerCastingState();
            PlayerGroundDefendState grounddefend = new PlayerGroundDefendState();
            PlayerAirDefendState airdefend = new PlayerAirDefendState();
            RegisterStateMachine("Idle", idle);
            RegisterStateMachine("Walk", walk);
            RegisterStateMachine("Sprint", sprint);
            RegisterStateMachine("JumpRising", jumpRising);
            RegisterStateMachine("JumpLanding", jumpLanding);
            RegisterStateMachine("JumpLanded", landed);
            RegisterStateMachine("Casting", casting);
            RegisterStateMachine("GroundDefend", grounddefend);
            RegisterStateMachine("AirDefend", airdefend);

            EnterState("Idle");
            RegisterCharacterEvent("PlayFootStepSound", PlayFootStepSound);
            RegisterCharacterEvent("EnterIdle", EnterIdle);
            RegisterCharacterEvent("EnterCasting", EnterCasting);
            RegisterCharacterEvent("Dashdown", Dashdown);
            RegisterCharacterEvent("Spin", Spin);
            RegisterCharacterEvent("ReadyForSecondAttack", ReadyForSecondAttack);
            RegisterCharacterEvent("SecondAttack", SecondAttack);
            RegisterCharacterEvent("ReadyForThirdAttack", ReadyForThirdAttack);
            RegisterCharacterEvent("ThirdAttack", ThirdAttack);

            GetSpellsFromConfigure();
            BindGameUIController(UIInstance.PlayerGameUIController);
            //  CharacterSetSpellActive("dashdown", true);
            //  CharacterSetSpellActive("spin", true);

        }
        /// <summary>
        /// 播放脚步声
        /// </summary>
        public void PlayFootStepSound()
        {
            ESoundInstance.PlaySFX(playerConfigure.Footstep, transform.position, 2f);
        }

        #region Dashdown
        /// <summary>
        /// 向下冲刺，同时开始判定，如果着陆则用"finish"信号打断技能。
        /// </summary>
        public void Dashdown()
        {
            PlayAnimation(configure.JumpLandingAnimation, 0.1f);
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y - configure.JumpHeight * 10f, rb.velocity.z);
            CharacterStartInterruptableCoroutine(DodashdownUpdate());
        }
        IEnumerator DodashdownUpdate()
        {
            yield return new WaitUntil(() => { return rb.velocity.y >= 0; });
            //Debug.Log("dash down landed");
            CharacterCancelSpellWithCommand("finish");
        }
        #endregion
        #region Spin
        public void Spin()
        {
            CharacterStartInterruptableCoroutine(Dospin());
        }
        IEnumerator Dospin()
        {
            yield return new WaitForEndOfFrame();
            float timer = 0;
            int frame = 0;
            while (timer < 20f)
            {
                timer += Time.deltaTime;
                int frameCount = (int)(timer / Time.fixedDeltaTime);
                if (frameCount > frame)
                {
                    frame++;
                    transform.Rotate(Vector3.up, 15f);
                }
                if (Input.GetKeyDown(KeyCode.V))
                {
                    //延后一帧是为了避免刚停止又继续释放。
                    yield return new WaitForEndOfFrame();
                    CharacterCancelSpellWithCommand("finish");
                }
                yield return null;
            }
            CharacterCancelSpellWithCommand("finish");
        }
        #endregion
        #region AttackEventSeries
        /// <summary>
        /// 
        /// </summary>
        public void ReadyForSecondAttack()
        {
            float time = 1f;
            CharacterStartInterruptableCoroutine(DoReadyForSecondAttack(time));
        }
        public void SecondAttack()
        {
            CharacterApplySpell("attack2");
        }
        IEnumerator DoReadyForSecondAttack(float time)
        {
            float timer = 0;
            while (timer < time)
            {
                timer += Time.deltaTime;
                if (Input.GetKeyDown((configure as PlayerConfigure).BaseAttack))
                {
                    CharacterCancelSpell("SecondAttack");
                    break;
                }
                yield return null;
            }
        }
        public void ReadyForThirdAttack()
        {
            float time = 1f;
            CharacterStartInterruptableCoroutine(DoReadyForThirdAttack(time));
        }
        public void ThirdAttack()
        {
            CharacterApplySpell("attack3");
        }
        IEnumerator DoReadyForThirdAttack(float time)
        {
            float timer = 0;
            while (timer < time)
            {
                timer += Time.deltaTime;
                if (Input.GetKeyDown((configure as PlayerConfigure).BaseAttack))
                {
                    CharacterCancelSpell("ThirdAttack");
                    break;
                }
                yield return null;
            }
        }
        #endregion

        #region AttackAbout
        public override void ApplyAreaAttack(SpellAttackEvent eventData)
        {
            base.ApplyAreaAttack(eventData);
            if (ConfigureInstance.GetValue<EmiteInnaBool>("uniform", "AreaAttackDebug").Value)
                Debug.Log("攻击了");
            Vector3 center = eventData.centerOffset + transform.position;
            float maxBound = 12f * Mathf.Max(eventData.extends.x, Mathf.Max(eventData.extends.y, eventData.extends.z));
            Collider[] cols = Physics.OverlapBox(transform.position, new Vector3(maxBound, maxBound, maxBound));
            //  Debug.DrawLine(center, center + new Vector3(maxBound, maxBound, maxBound),Color.red, 5f);
            foreach (Collider col in cols)
            {
                if (ConfigureInstance.GetValue<EmiteInnaBool>("uniform", "AreaAttackDebug").Value)
                    Debug.Log("区域内有" + col.gameObject.name);
                EnemyController e = col.gameObject.GetComponent<EnemyController>();
                if (e == null) continue;
                if (EChara.TargetInAttackRange(transform, col, eventData))
                {
                    e.OnApplyCharacterEvent<float>("LightAttack", 0);
                }
            }
        }
        #endregion
    }
}