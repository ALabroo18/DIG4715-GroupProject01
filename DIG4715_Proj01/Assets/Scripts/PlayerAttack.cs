using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using UnityEngine;

// Make sure this script is assigned to each of the player weapons (the arrow weapon used for each class).
public class PlayerAttack : MonoBehaviour
{
    //public GameObject explosionAnimation; // This needs to be assigned in the inspector of the player weapon. (Not doing).
    private PlayerBehavior pB; // This is a reference to the player behavior script which is used in different parts of the script.
    private int enemyHitScore = 100; // This is the amount of score the player is given when they hit an enemy.
    public static int damageDealt = 25; // This is the amount of damage the player's weapon deals to enemies.
    public GameObject bombExplosion;
    // public static int bombDamageDealt = 35;
    // private LevelBar levelBar;

    void Start()
    {
        // Set reference to level bar.
        // levelBar = GameObject.FindWithTag("LevelBar").GetComponent<LevelBar>();

        // Assign the reference to the player script.
        pB = GameObject.FindWithTag("Player").GetComponent<PlayerBehavior>();

        // Destroy the player attack after a set amount of time so it does not exist for too long.

        Destroy(this.gameObject, 0.42f);

        //THE REST OF THE CODE IS USELESS FOR THIS PROJECT
        // if (this.CompareTag("Sword"))
        // {
            
        // }
        // else
        // {
        //     Destroy(this.gameObject, 4.5f);
        // }
    }

    // When the weapon collides with something, check the tag to see what it is. 
    void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.CompareTag("Enemy"))
        {
            // Give the player some experience.
            // levelBar.levelSystem.AddExperience(15);

            // If the arrow hits an enemy, deal damage to the enemy, change the player's score, and destroy the arrow.
            // Right before destroying, call player weapon destroyed function to allow the player to attack again.

            Debug.Log("hi");
            hit.GetComponent<EnemyScript>().EnemyTakeDamage();
            // if (this.CompareTag("Arrow") || this.CompareTag("Sword"))
            // {
                
            // }
            // else if (this.CompareTag("Bomb"))
            // {
            //     //Use explosion script here
            //     hit.GetComponent<EnemyScript>().EnemyTakeDamage();
            // }
           
            // Check to make sure the weapon is not the sword so that the sword attack cannot be spammed.
            // if (!this.CompareTag("Sword"))
            // {
            //     pB.WeaponDestroyed();
            // }

            pB.ChangeScore(enemyHitScore);

            Debug.Log("Enemy hit");
            Destroy(this.gameObject);
        }
        else if(hit.CompareTag("Spawner")){
            pB.ChangeScore(200);
            Destroy(this.gameObject);

        }
        // else if (hit.CompareTag("Projectile"))
        // {
        //     // Give the player some experience.
        //     // levelBar.levelSystem.AddExperience(5);

        //     Destroy(hit.gameObject);
        // }
    }
}