using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayerTest : MonoBehaviour
{
    [SerializeField] private Player player = default;
    [SerializeField][Range(10, 200)] private float damage = 50;

    public void Damage()
    {
        player.health -= damage;
    }
    
}
