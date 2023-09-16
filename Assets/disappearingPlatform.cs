using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disappearingPlatform : MonoBehaviour
{
    SpriteRenderer rend;
    // Start is called before the first frame update

    private void OnCollisionEnter2D(Collision2D col)
    {   
        if(col.collider.CompareTag("Player")){
            StartCoroutine(startFading());
        }
        
    }
    void Start()
    {
        rend = GetComponent<SpriteRenderer> ();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator FadeOut() 
    {
       
        for (float f = 1f; f>=-0.5f; f-=0.05f){
            Color c = rend.material.color;
            c.a = f;
            rend.material.color = c;
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator FadeIn() 
    {
       for (float f = 0f; f<=1f; f+=0.05f){
            Color c = rend.material.color;
            c.a = f;
            rend.material.color = c;
            yield return new WaitForSeconds(0.05f);
        } 
    }
    IEnumerator startFading(){
        for (float f = 1f; f>=-0.5f; f-=0.05f){
            Color c = rend.material.color;
            c.a = f;
            rend.material.color = c;
            yield return new WaitForSeconds(0.05f);
        }
        Destroy(gameObject);
    }
}
