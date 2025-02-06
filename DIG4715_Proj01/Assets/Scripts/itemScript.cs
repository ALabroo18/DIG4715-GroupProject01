using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class itemScript : MonoBehaviour
{
    public AudioClip audioClip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other){

         PlayerBehavior pB = other.GetComponent<PlayerBehavior>();

        Debug.Log("Reached trigger");
        if(this.gameObject.CompareTag("meal")){
            other.GetComponent<PlayerBehavior>().ChangeLives(100);
            Debug.Log("Got meal");
            Destroy(this.gameObject);

             pB.PlaySound(audioClip);

        }
        else if(this.gameObject.CompareTag("chest")){
            other.GetComponent<PlayerBehavior>().ChangeScore(500);
            Debug.Log("Got chest");
            Destroy(this.gameObject);

            pB.PlaySound(audioClip);
        }
        else if(this.gameObject.CompareTag("potion")){
            other.GetComponent<PlayerBehavior>().potionCount += 1;
            Debug.Log("Got potion");
            Destroy(this.gameObject);

            pB.PlaySound(audioClip);
        }
        else if(this.gameObject.CompareTag("key")){
            other.GetComponent<PlayerBehavior>().hasKey = true;
            Debug.Log("Got key");
            Destroy(this.gameObject);

            pB.PlaySound(audioClip);

        }
    }

    private void OnCollisionEnter2D(Collision2D coll){
        if(this.gameObject.CompareTag("gate")){
            
            Destroy(this.gameObject);
        }
    }
}
