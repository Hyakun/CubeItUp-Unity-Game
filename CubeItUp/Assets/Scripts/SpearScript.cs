using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearScript : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private Rigidbody2D rb;

    private Transform player;
    private Vector2 selectedDirection;

    private float spawnTime;
    private bool stopRotation;
    CharacterMovement playerRef;
    GameObject go;
    [SerializeField]
    private GameObject hitEffect;

    GameObject personaj;
    void Start()
    {
        spawnTime = Time.time;
        stopRotation = false;
        personaj = GameObject.FindGameObjectWithTag("Player");

    }

    private void Update()
    {
    }


    private void FixedUpdate()
    {

        if (CharacterMovement.isAllive == true)
        {
            if(stopRotation == false)
            {
                Vector3 direction = personaj.transform.position - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                rb.rotation = angle;
            }

            if (Time.time >= spawnTime + 1)
            {
                if (player == null)
                {
                    player = personaj.transform;

                    selectedDirection = player.transform.position;
                }
                stopRotation = true;
                transform.position = Vector2.MoveTowards(transform.position, selectedDirection, speed * Time.fixedDeltaTime);

            }
        }

            if(transform.position == new Vector3(selectedDirection.x, selectedDirection.y, transform.position.z))
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
