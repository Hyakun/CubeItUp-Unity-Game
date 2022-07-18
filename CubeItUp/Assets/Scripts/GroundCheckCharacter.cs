using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheckCharacter : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("ENDSTART"))
        {
            CharacterMovement.isgrounded = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ENDSTART"))
        {
            ScoreCount.started = true;
            Destroy(collision.gameObject);
        }
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("ENDSTART"))
    //    {
    //        CharacterMovement.isgrounded = true;
    //    }
    //    if (collision.gameObject.CompareTag("ENM"))
    //    {
    //        FindObjectOfType<AudioManager>().Play("DeathOrTakeDMG");
    //        AudioManager.score += 100;
    //        Debug.Log("asd");
    //        Destroy(collision.gameObject);
    //    }
    //}
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("ENDSTART"))
    //    {
    //        ScoreCount.started = true;
    //        Destroy(collision.gameObject);
    //    }
    //}
}
