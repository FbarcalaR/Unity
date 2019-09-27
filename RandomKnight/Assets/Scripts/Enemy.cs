using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health =1;
    //public GameObject bloodEffect;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
        GameController.instance.PlayerScored();
    }

    private void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
            GameController.instance.PlayerScored();
        }
    }

    public void TakeDamage(int damage)
    {
        //Instantiate(bloodEffect, transfor.position, Quaternion.identity);
        health -= damage;
        Debug.Log("Ahh ma cagondios");
    }
}
