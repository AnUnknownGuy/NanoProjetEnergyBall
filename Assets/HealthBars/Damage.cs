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

        [SerializeField][Range(0, 2)] private float delay = default;
        [SerializeField][Range(0.1f, 2)] private float timeToFade = 1.0f;
        [SerializeField] private Mode mode = default;
        [SerializeField] private Image health = default;
        
        private Image fill; // autofind
        private Color baseFillColor; // autofind

        // internal logic
        private float previousHealthValue;
        private float fadeSpeed = 0;
        private float delayCounter = 0;
        private float fadeLerpRatio = 0;

        private void Awake() {
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
                    fadeSpeed = (previousHealthValue - health.fillAmount)/timeToFade;
                    break;

                case Mode.fade:
                    fadeLerpRatio = 1;
                    fill.color = baseFillColor;
                    break;

                default:
                    return;
            }
            fill.fillAmount = previousHealthValue;
            previousHealthValue = health.fillAmount;
        }
    }
}
