using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenScript : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private Animator anim;

    private Transform player;
    private Vector2 selectedDirection;

    private float spawnTime;

    CharacterMovement playerRef;
    GameObject go;
    [SerializeField]
    private GameObject hitEffect;

    GameObject personaj;



    void Start()
    {
        spawnTime = Time.time;
        personaj = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
    }


    private void FixedUpdate()
    {

        if (CharacterMovement.isAllive == true)
        {

            if (Time.time >= spawnTime + 1)
            {
                if (player == null)
                {
                    player = personaj.transform;

                    selectedDirection = player.transform.position;
                }
                anim.SetBool("Thrown", true);
                transform.position = Vector2.MoveTowards(transform.position, selectedDirection, speed * Time.fixedDeltaTime);
            }
        }

        if (transform.position == new Vector3(selectedDirection.x, selectedDirection.y, transform.position.z))
        {
            Destroy(gameObject);
        }

    }
    private void OnTriggerEnter2D(Collider2D collisionEnter)
    {
        if (collisionEnter.gameObject.CompareTag("Player") && CharacterMovement.canGetHit)
        {
            CharacterMovement.health -= 1;
            FindObjectOfType<AudioManager>().Play("DeathOrTakeDMG");
            Instantiate(hitEffect, collisionEnter.transform.position, hitEffect.transform.rotation);
            go = GameObject.FindGameObjectWithTag("Player");
            playerRef = (CharacterMovement)go.GetComponent(typeof(CharacterMovement));
            playerRef.HitRestriction();
            Destroy(gameObject);
        }
    }


}
