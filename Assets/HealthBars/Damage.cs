using UnityEngine;
using UnityEngine.UI;

namespace HealthBarsPackage
{
    public class Damage : MonoBehaviour
    {
        enum Mode {
            slide,
            fade
        }

        [SerializeField] private bool updateOnDamage = true;
        [SerializeField][Range(0, 2)] private float delay = default;
        [SerializeField][Range(0.1f, 2)] private float timeToFade = 1.0f;
        [SerializeField] private Mode mode = default;
        [SerializeField] private Image health = default;
        
        private Image fill; // autofind
        private Color baseFillColor; // autofind

        // internal logic
        private float previousHealthValue;
        [SerializeField] private float fadeSpeed = 0;
        private float delayCounter = 0;
        private float fadeLerpRatio = 0;

        private void Awake() {
            Load();
        }

        public void Load()
        {
            fill = GetComponent<Image>();
            health.GetComponent<Health>().RegisterFade(this);
            previousHealthValue = 1;
            baseFillColor = fill.color;
            fill.fillAmount = 1;
        }

        // Update is called once per frame
        void Update()
        {
            if(delayCounter > 0) {
                delayCounter -= Time.deltaTime;
                return;
            }

            switch (mode)
            {
                case Mode.slide :
                    fill.fillAmount -= fadeSpeed * Time.deltaTime;
                    break;

                case Mode.fade:
                    fadeLerpRatio = fadeLerpRatio - (Time.deltaTime/timeToFade);
                    Color lerpedColor = Color.Lerp(Color.clear, baseFillColor, fadeLerpRatio);
                    fill.color = lerpedColor;
                    break;

                default:
                    return;
            }
        }

        public void Set()
        {
            delayCounter = delay;
            switch (mode)
            {
                case Mode.slide :
                    fadeSpeed = (fill.fillAmount - health.fillAmount)/timeToFade;
                    if (fadeSpeed < 0) fadeSpeed = 0;
                    break;

                case Mode.fade:
                    fadeLerpRatio = 1;
                    fill.color = baseFillColor;
                    break;

                default:
                    return;
            }
            if(updateOnDamage || health.fillAmount > fill.fillAmount || mode == Mode.fade)
            {
                fill.fillAmount = previousHealthValue;                
            }
            previousHealthValue = health.fillAmount;
        }
    }
}
