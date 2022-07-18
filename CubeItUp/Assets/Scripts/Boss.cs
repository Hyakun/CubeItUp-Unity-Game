using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Boss : MonoBehaviour
{

    private int health;
    public Slider healthBar;
    public static int stage = 1;
    [SerializeField]
    private Animator anim;

    [SerializeField]
    private Transform player;
    public bool isFlipped = false;

    [SerializeField]
    BoxCollider2D collider;

    private bool isTrigger = false;
    CharacterMovement playerRef;
    GameObject go;

    [SerializeField]
    private GameObject bossDeathEffect;

    // Start is called before the first frame update
    void Start()
    {
        health = 30;
        stage = 1;
    }

    // Update is called once per frame
    void Update()
    {
        StageCheck();
        healthBar.value = health;
        if (health <= 0)
        {
            AudioManager.score += 1000;
            Instantiate(bossDeathEffect, transform.position, bossDeathEffect.transform.rotation);
            FindObjectOfType<AudioManager>().Play("BossDeath");
            AudioManager.bossDeath = true;
            Destroy(gameObject);
        }
    }

    public void StageCheck()
    {
        if(health == 25)
        {
            stage = 2;
        }
        if(health <= 20)
        {
            stage = 3;
            anim.SetBool("Stage3Started", true);
            ChangetoTrigger();
        }
    }

    private void ChangetoTrigger()
    {
        if (!isTrigger)
        {
            collider.isTrigger = true;
            isTrigger = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (CharacterMovement.isDashing == true)
            {
                FindObjectOfType<AudioManager>().Play("DeathOrTakeDMG");
                health -= 1;
            }
        }
    }

    public void LookAtPlayer()
    {
        if (player != null)
        {
            Vector3 flipped = transform.localScale;
            flipped.z *= -1f;

            if (transform.position.x > player.position.x && isFlipped)
            {
                transform.localScale = flipped;
                transform.Rotate(0f, 180f, 0f);
                isFlipped = false;
            }
            else if (transform.position.x < player.position.x && !isFlipped)
            {
                transform.localScale = flipped;
                transform.Rotate(0f, 180f, 0f);
                isFlipped = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(CharacterMovement.isDashing == true)
            {
                FindObjectOfType<AudioManager>().Play("DeathOrTakeDMG");
                health -= 1;
            }else if(CharacterMovement.isDashing == false && CharacterMovement.canGetHit)
            {
                CharacterMovement.health -= 1;
                FindObjectOfType<AudioManager>().Play("DeathOrTakeDMG");
                go = GameObject.FindGameObjectWithTag("Player");
                playerRef = (CharacterMovement)go.GetComponent(typeof(CharacterMovement));
                playerRef.HitRestriction();
            }
        }
    }
    
}
