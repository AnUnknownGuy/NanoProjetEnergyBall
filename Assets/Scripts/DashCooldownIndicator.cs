using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashCooldownIndicator : MonoBehaviour
{
    // References
    public Player Player {
        set {
            this.player = value;
            SetUp();
        }
    }
    private Player player = default;
    private Image cooldownIndicator; //autofind

    // Internal logic
    private float dashCooldown;
    private float timeCounter;

    private void Start() {
        if(player) SetUp();
    }

    private void SetUp()
    {
        dashCooldown = player.timeBetweenDash;
        cooldownIndicator = GetComponent<Image>();
        cooldownIndicator.fillAmount = 0;
        timeCounter = dashCooldown;
    }

    private void Update() {
        timeCounter += Time.deltaTime;
        float fillAmount = timeCounter / dashCooldown;
        if (fillAmount >1) fillAmount = 0;
        cooldownIndicator.fillAmount = fillAmount;
    }

    public void Launch()
    {
        timeCounter = 0;
    }
}
