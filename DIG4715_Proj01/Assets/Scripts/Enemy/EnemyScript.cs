using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public static int EnemyHP;
    public static int  EnemyDmg;
    // public static float EnemySpeed;
    // private Color classColor;
    private string className;
    private string EnemySelection;
    //[SerializeField] private GameObject enemy;
    // public enum EnemyTypes
    // {
    //     Crawler,
    //     Speeder,
    //     Shooter
    // }
    // private EnemyTypes EnemySelect;
    // public EnemyTypes ChosenEnemy;

    void Start()
    {
        // ChosenEnemy = EnemySelect;
        // EnemySelection = ChosenEnemy.ToString();
        // EnemyClass(EnemySelection);
    }

    void EnemyClass(string EnemySelection)
    {



        // switch (EnemySelection)
        // {
        //     case "Crawler":
        //         //Debug.Log($"{EnemySelection}");
        //         EnemyHP = 100;
        //         EnemySpeed = 1.5f;
        //         EnemyMovement.speed = EnemySpeed;
        //         break;
        //     case "Speeder":
        //         //EnemyClass(EnemySelection);
        //         EnemyHP = 150;
        //         EnemySpeed = 2.5f;
        //         EnemyMovement.speed = EnemySpeed;
        //         break;
        //     case "Shooter":
        //         //EnemyClass(EnemySelection);
        //         EnemyHP = 200;
        //         EnemySpeed = 1.5f;
        //         EnemyMovement.speed = EnemySpeed;
        //         break;
        //     default:
        //         break;
      
        // }

        EnemyHP = 1;
    }
    public void EnemyTakeDamage()
    {
        // EnemyHP -= damage;

        // // If enemy has no more hp, destroy the object.
        // if (EnemyHP <= 0)
        // {
            //Debug.Log("Destroyed");
            Destroy(this.gameObject);
        // }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerBehavior>().TakeDamage();

            Destroy(this.gameObject);
        }
    }
}
