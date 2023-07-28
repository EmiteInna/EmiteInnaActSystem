using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EmiteInnaACT
{
    public struct GameUIView
    {
        public GameObject go;
        public RectTransform rect;
        public Image image;
    }
    /// <summary>
    /// 血条UI的控制器，主要负责控制玩家的血条和精神条View的变化和动画，以及技能轮盘的切换和技能的转CD功能。
    /// 直接挂在GameUI的Panel上就行。
    /// 主角的血条开启InitializeRightAfterAwake，怪物和Boss的血条最好在生成之后手动Initialize。
    /// </summary>
    /// 
    public class GameUIController : MonoBehaviour
    {
        public CharacterConfigureBase configure;
        GameUIView mental, mentalBottom, health, healthBottom, character,healthFlare,mentalFlare,mentalCover,healthCover;
        float currentStreakTime;
        float health_before;
        float healthBottom_before;
        float mental_before;
        float mentalBottom_before;

        float healthMaxRectRightValue;
        float mentalMaxRectRightValue;
        Vector3 originalPosition;
        bool streakBegin;
        Coroutine mentalCoroutine,healthCoroutine,mentalBottomCoroutine,healthBottomCoroutine,healthFlareCoroutine,mentalFlareCoroutine;


        public float streakTimeRange=2f;
        public float transitionTime = 0.1f;
        public bool InitializeRightAfterAwake=true;
        /// <summary>
        /// 按照格式读入组件。
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        GameUIView GetUIViewFromTransform(Transform transform)
        {
            GameUIView ret = new GameUIView();
            ret.go = transform.gameObject;
            ret.rect = ret.go.GetComponent<RectTransform>();
            ret.image = transform.GetComponent<Image>();
            return ret;
        }
        /// <summary>
        /// 设置血量槽血量，填入值域为[0,1]
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="value"></param>
        void SetAttributeBarValue(GameUIView ui,float value)
        {
            ui.image.fillAmount = value;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            if (configure == null)
            {
                Debug.LogError("没有放入角色信息，GameUI失效！");
                Destroy(gameObject);
                return;
            }
            mental=GetUIViewFromTransform(transform.GetChild(0).GetChild(1));
            mentalBottom=GetUIViewFromTransform(transform.GetChild(0).GetChild(0));
            mentalCover = GetUIViewFromTransform(transform.GetChild(0));
            health = GetUIViewFromTransform(transform.GetChild(2).GetChild(1));
            healthBottom = GetUIViewFromTransform(transform.GetChild(2).GetChild(0));
            healthCover = GetUIViewFromTransform(transform.GetChild(2));
            character = GetUIViewFromTransform(transform.GetChild(4));
            healthFlare = GetUIViewFromTransform(transform.GetChild(5));
            mentalFlare = GetUIViewFromTransform(transform.GetChild(6));
            healthFlare.image.color = new Color(1, 1, 1, 0);
            mentalFlare.image.color = new Color(1, 1, 1, 0);
            SetAttributeBarValue(mental, configure.CurrentMental/configure.MaxMental);
            SetAttributeBarValue(mentalBottom, configure.CurrentMental / configure.MaxMental );
            SetAttributeBarValue(health, configure.CurrentHealth / configure.MaxHealth );
            SetAttributeBarValue(healthBottom, configure.CurrentHealth / configure.MaxHealth );
            health_before = configure.CurrentHealth;
            healthBottom_before = configure.CurrentHealth;
            mental_before = configure.CurrentMental;
            mentalBottom_before = configure.CurrentMental;
            currentStreakTime = 0;
            streakBegin = false;
            originalPosition = transform.position;
        }
        /// <summary>
        /// 在角色受到伤害之后触发，更新属性条。
        /// 前属性条瞬间坠落，后属性条在连击结束后两秒开始坠落。
        /// </summary>
        public void UpdateAttributeBarAfterTakingDamage()
        {
            if (healthCoroutine != null) StopCoroutine(healthCoroutine);
            healthCoroutine = StartCoroutine(MoveAttributeBarInSecond(health, health_before / configure.MaxHealth, configure.CurrentHealth / configure.MaxHealth));
            if (healthFlareCoroutine != null) StopCoroutine(healthFlareCoroutine);
            healthFlareCoroutine = StartCoroutine(MoveHealthFlare(health_before / configure.MaxHealth, configure.CurrentHealth / configure.MaxHealth));
            health_before = configure.CurrentHealth;

            if (mentalCoroutine != null) StopCoroutine(mentalCoroutine);
            mentalCoroutine = StartCoroutine(MoveAttributeBarInSecond(mental, mental_before / configure.MaxMental, configure.CurrentMental / configure.MaxMental));
            if (mentalFlareCoroutine != null) StopCoroutine(mentalFlareCoroutine);
            mentalFlareCoroutine = StartCoroutine(MoveMentalFlare(mental_before / configure.MaxMental, configure.CurrentMental / configure.MaxMental));
            mental_before = configure.CurrentMental;

            streakBegin = true;
            currentStreakTime = streakTimeRange;
            Shake();
            CharacterImageFlash();
        }
        /// <summary>
        /// 在角色回复之后触发，更新属性条。
        /// </summary>
        public void UpdateAttributeBarAfterRegenerate()
        {
            if(healthCoroutine!=null)StopCoroutine(healthCoroutine);
            healthCoroutine= StartCoroutine(MoveAttributeBarInSecond(health, health_before/configure.MaxHealth, configure.CurrentHealth/configure.MaxHealth));
            health_before = configure.CurrentHealth;
            if (mentalCoroutine != null) StopCoroutine(mentalCoroutine);
            mentalCoroutine = StartCoroutine(MoveAttributeBarInSecond(mental, mental_before / configure.MaxMental, configure.CurrentMental / configure.MaxMental));
            mental_before = configure.CurrentMental;
            if (healthBottomCoroutine != null) StopCoroutine(healthBottomCoroutine);
            healthBottomCoroutine = StartCoroutine(MoveAttributeBarInSecond(healthBottom, healthBottom_before / configure.MaxHealth, configure.CurrentHealth / configure.MaxHealth));
            healthBottom_before = configure.CurrentHealth;
            if (mentalBottomCoroutine != null) StopCoroutine(mentalBottomCoroutine);
            mentalBottomCoroutine = StartCoroutine(MoveAttributeBarInSecond(mentalBottom, mentalBottom_before / configure.MaxMental, configure.CurrentMental / configure.MaxMental));
            mentalBottom_before = configure.CurrentMental;
        }
        IEnumerator MoveAttributeBarInSecond(GameUIView ui,float before,float after)
        {
            float timer = 0;
            while (timer < transitionTime)
            {
                timer += Time.fixedDeltaTime;
                SetAttributeBarValue(ui, Mathf.Lerp(before, after, timer / transitionTime));
                yield return CoroutineTool.WaitForFixedUpdate();
            }
            SetAttributeBarValue(ui, after);
        }
        IEnumerator MoveHealthFlare(float before,float after)
        {
            float timer = 0;
            float left=-healthCover.rect.rect.width/2+healthCover.rect.anchoredPosition.x;
            float right = + healthCover.rect.rect.width / 2 + healthCover.rect.anchoredPosition.x;
            healthFlare.image.color = new Color(1, 1, 1, 1);
            while (timer < transitionTime)
            {
                timer += Time.fixedDeltaTime;
                healthFlare.rect.anchoredPosition = new Vector2(Mathf.Lerp(Mathf.Lerp(left,right,before), Mathf.Lerp(left,right,after), timer / transitionTime),healthFlare.rect.anchoredPosition.y);
                yield return CoroutineTool.WaitForFixedUpdate();
            }
            healthFlare.image.color = new Color(1, 1, 1, 0);
        }
        IEnumerator MoveMentalFlare(float before, float after)
        {
            float timer = 0;
            float left = -mentalCover.rect.rect.width / 2 + mentalCover.rect.anchoredPosition.x;
            float right = +mentalCover.rect.rect.width / 2 + mentalCover.rect.anchoredPosition.x;
            mentalFlare.image.color = new Color(1, 1, 1, 1);
            while (timer < transitionTime)
            {
                timer += Time.fixedDeltaTime;
                mentalFlare.rect.anchoredPosition = new Vector2(Mathf.Lerp(Mathf.Lerp(left, right, before), Mathf.Lerp(left, right, after), timer / transitionTime), mentalFlare.rect.anchoredPosition.y);
                yield return CoroutineTool.WaitForFixedUpdate();
            }
            mentalFlare.image.color = new Color(1, 1, 1, 0);
        }
        /// <summary>
        /// 受击时的震荡效果
        /// </summary>
        public void Shake(float shakeDuration=0.4f,float shakeIntensity=5f,float shakeFrequency=25f)
        {
            StartCoroutine(DoShake(shakeDuration, shakeIntensity, shakeFrequency));
        }
        IEnumerator DoShake(float shakeDuration,float shakeIntensity,float shakeFrequency)
        {
            Vector3 pos = originalPosition;
            float buf=Random.Range(0.0f, 1.14514f);
            float timer = 0;
            while (timer <= shakeDuration)
            {
                float x = Mathf.PerlinNoise(timer * shakeFrequency, 0f) * 2f - 1f;
                float y = Mathf.PerlinNoise(0f, timer * shakeFrequency) * 2f - 1f;
                Vector3 off = new Vector3(x, y, 0) * shakeIntensity;
                transform.position = pos + off;
                timer += Time.fixedDeltaTime;
                yield return CoroutineTool.WaitForFixedUpdate();
            }
            transform.position = pos;
        }
        /// <summary>
        /// 受击时的头像颜色变化
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="strength"></param>
        public void CharacterImageFlash(float duration=0.4f,float strength=0.4f)
        {
            StartCoroutine(DoCharacterImageFlash(duration, strength));
        }
        IEnumerator DoCharacterImageFlash(float duration,float strength)
        {
            Color originalColor = Color.white;
            Color targetColor = new Color(1, 1 - strength, 1 - strength);
            float timer = 0;
            float rise = duration * 0.3f;
            character.image.color = Color.white;
            while (timer <= rise)
            {
                character.image.color=Color.Lerp(originalColor, targetColor, timer/rise);
                timer += Time.fixedDeltaTime;
                yield return CoroutineTool.WaitForFixedUpdate();
            }
            float fall = duration - rise;
            timer = 0;
            while (timer <= fall)
            {
                character.image.color = Color.Lerp(targetColor, originalColor, timer / fall);
                timer += Time.fixedDeltaTime;
                yield return CoroutineTool.WaitForFixedUpdate();
            }
            character.image.color = Color.white;
        }
        private void Awake()
        {
            if(InitializeRightAfterAwake)
                Initialize();
        }
        private void FixedUpdate()
        {
            if (configure == null) return;
            if (streakBegin)
            {
                currentStreakTime -= Time.fixedDeltaTime;
                if (currentStreakTime <= 0)
                {
                    streakBegin = false;
                    if (healthBottomCoroutine != null) StopCoroutine(healthBottomCoroutine);
                    healthBottomCoroutine = StartCoroutine(MoveAttributeBarInSecond(healthBottom, healthBottom_before / configure.MaxHealth, configure.CurrentHealth / configure.MaxHealth));
                    healthBottom_before = configure.CurrentHealth;
                    if (mentalBottomCoroutine != null) StopCoroutine(mentalBottomCoroutine);
                    mentalBottomCoroutine = StartCoroutine(MoveAttributeBarInSecond(mentalBottom, mentalBottom_before / configure.MaxMental, configure.CurrentMental / configure.MaxMental));
                    mentalBottom_before = configure.CurrentMental;
                }
            }
        }
    }
}