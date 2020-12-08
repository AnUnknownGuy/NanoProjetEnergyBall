using System.Xml.Xsl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HealthBarsPackage
{
    public class Fade : MonoBehaviour
    {
        enum Mode {
            slide,
            fade
        }

        [SerializeField][Range(0, 2)] private float delay = default;
        [SerializeField][Range(0.1f, 2)] private float timeToFade = 1.0f;
        [SerializeField] private Mode mode = default;

        private Slider healthSlider; // autofind
        private Slider fadeSlider; // autofind
        [SerializeField] private Image fill;
        private Color baseFillColor; // autofind

        // internal logic
        private float previousHealthValue;
        private float fadeSpeed = 0;
        private float delayCounter = 0;
        private float fadeLerpRatio = 0;

        private void Awake() {
            healthSlider = transform.parent.GetComponent<Slider>();
            fadeSlider = GetComponent<Slider>();
            previousHealthValue = healthSlider.value;
            baseFillColor = fill.color;
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
                    fadeSlider.value -= fadeSpeed * Time.deltaTime;
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
                    fadeSpeed = (previousHealthValue - healthSlider.value)/timeToFade;
                    break;

                case Mode.fade:
                    fadeLerpRatio = 1;
                    fill.color = baseFillColor;
                    break;

                default:
                    return;
            }
            fadeSlider.value = previousHealthValue;
            previousHealthValue = healthSlider.value;
        }
    }
}
