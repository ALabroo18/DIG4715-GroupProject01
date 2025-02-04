using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class itemScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other){

        
        if(this.gameObject.CompareTag("meal")){
            other.GetComponent<PlayerBehavior>().ChangeLives(100);
            Destroy(this.gameObject);

        }
        else if(this.gameObject.CompareTag("chest")){
            other.GetComponent<PlayerBehavior>().ChangeScore(500);
            Destroy(this.gameObject);
        }
        else if(this.gameObject.CompareTag("AOE")){
            other.GetComponent<PlayerBehavior>().hasPotion = true;
            Destroy(this.gameObject);
        }
        else if(this.gameObject.CompareTag("key")){
            other.GetComponent<PlayerBehavior>().hasKey = true;
            Destroy(this.gameObject);

        }
    }

    private void OnCollisionEnter2D(Collision2D coll){
        if(this.gameObject.CompareTag("gate")){
            Destroy(this.gameObject);
        }
    }
}
