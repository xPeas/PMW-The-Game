using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public bool damageActive = false;
    
    public float DamageToDeal;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (damageActive == false)
            return;
        
        GameObject o;
        BasicEnemy enemy = (o = other.gameObject).GetComponent<BasicEnemy>();
        
        Debug.Log($"Weapon hit something {o.name}");

        if (enemy != null)
        {
            enemy.TakeDamage(DamageToDeal);
        }
    }
}
