using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EenemyPlayerColision : MonoBehaviour
{
    private float speed = 5f;
    [SerializeField]
    private Rigidbody2D rigidBody;
    private Vector3 initialPose;
    private int direction = 1;
    [SerializeField]
    private GameObject effect;
    [SerializeField]
    private GameObject effectPlayer;

    [SerializeField]
    private Transform groundDetectionLeft;
    [SerializeField]
    private Transform groundDetectionRight;

    GameObject go;

    CharacterMovement playerRef;
    [SerializeField]
    GameObject Q;




    // Start is called before the first frame update
    void Start()
    {
        initialPose = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        enemyWalk();
        VerifyGround();

    }

    private void VerifyGround()
    {
        RaycastHit2D groundInfoLeft = Physics2D.Raycast(groundDetectionLeft.position, Vector2.down, 2f);
        RaycastHit2D groundInfoRight = Physics2D.Raycast(groundDetectionRight.position, Vector2.down, 2f);

        if (groundInfoLeft.collider == false)
        {
            direction = 1;
        }
        if (groundInfoRight.collider == false)
        {
            direction = -1;
        }
    }

    

    private void enemyWalk()
    {
        if (direction == 1)
        {
            rigidBody.velocity = new Vector2(1 * speed, rigidBody.velocity.y);
            if (transform.position.x > initialPose.x + 3)
            {
                direction = -1;
            }

        }
        else if (direction == -1)
        {
            rigidBody.velocity = new Vector2(-1 * speed, rigidBody.velocity.y);
            if (transform.position.x < initialPose.x - 3)
            {
                direction = 1;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(CharacterMovement.isDashing)
            {
                Instantiate(effect, transform.position, Quaternion.identity);
                FindObjectOfType<AudioManager>().Play("DeathOrTakeDMG");
                AudioManager.score  += 100;
                GameObject qAdded = Instantiate(Q) as GameObject;
                qAdded.transform.SetParent(GameObject.FindGameObjectWithTag("CNV").transform, false);
                Destroy(gameObject);
            }
            else
            {
                if(CharacterMovement.canGetHit)
                {
                CharacterMovement.health -= 1;
                FindObjectOfType<AudioManager>().Play("DeathOrTakeDMG");
                go = GameObject.FindGameObjectWithTag("Player");
                playerRef = (CharacterMovement)go.GetComponent(typeof(CharacterMovement));
                playerRef.HitRestriction();
                }
            }
        }

        if(collision.gameObject.CompareTag("Ground"))
        {
            if(direction == -1)
            {
                direction = 1;
            }else if(direction == 1)
            {
                direction = -1;
            }
        }
    }

}