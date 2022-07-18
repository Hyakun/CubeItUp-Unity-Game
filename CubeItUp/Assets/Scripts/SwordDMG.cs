using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDMG : MonoBehaviour
{

    CharacterMovement playerRef;
    GameObject go;
    [SerializeField]
    private GameObject hitEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (CharacterMovement.isDashing == false && CharacterMovement.canGetHit)
            {
                CharacterMovement.health -= 1;
                FindObjectOfType<AudioManager>().Play("DeathOrTakeDMG");
                Instantiate(hitEffect, collision.transform.position, hitEffect.transform.rotation);
                go = GameObject.FindGameObjectWithTag("Player");
                playerRef = (CharacterMovement)go.GetComponent(typeof(CharacterMovement));
                playerRef.HitRestriction();
            }
        }
    }
}
