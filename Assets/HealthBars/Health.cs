using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

namespace HealthBarsPackage
{
    public class Health : MonoBehaviour
    {

        private Image fill; // autofind

        private Damage damage; // autoregister
        public void RegisterFade(Damage damage){
            this.damage = damage;
        }

        [SerializeField] private Player _player = default;
        public Player player
        {
            get { return _player;}
            set { _player = value; Reset();}
        }
        
        private float maxHealth;

        private void Awake() {
            fill = GetComponent<Image>();
            if(_player) Reset();
            else Debug.LogError("No player bound to healthBar !");
        }

        private void Reset() {
            maxHealth = _player.health;
            lastHealth = _player.health;
            fill.fillAmount = 1;
        }

        private float lastHealth;
        private void Update() {
            if (lastHealth == _player.health) return;
            fill.fillAmount = _player.health/maxHealth;
            lastHealth = _player.health;
            damage.Set();
        }
    }
}