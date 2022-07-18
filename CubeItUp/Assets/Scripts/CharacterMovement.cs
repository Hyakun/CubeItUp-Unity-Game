using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyJoystick;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterMovement : MonoBehaviour
{
    public static int health;
    public static bool isAllive = true;
    [SerializeField] private float speed = 5f;
    private float movement = 0f;
    private Rigidbody2D rigidBody;
    [SerializeField] private Joystick joystick;

    [SerializeField]
    private Slider healthBar;

    public static bool isDashing;
    public float dashTime = 0.2f;
    public float dashSpeed = 50f;
    public float distanceBetweenImages = 0.1f;
    public float dashCooldown = 2.5f;
    private float dashTimeLeft;
    private float lastImageXpos;
    private float lastDash = -100f;
    private int facingDirection = 1;
    public static bool isgrounded = true;

    public GameObject effectPlayer;

    [SerializeField]
    private Animator anim;

    public static bool canGetHit;



    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        health = 3;
        canGetHit = true;

    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            healthBar.value = health;
        }
        catch { };
        movement = joystick.Horizontal();
        //movement = Input.GetAxisRaw("Horizontal");


        changeDirection();

        rigidBody.velocity = new Vector2(movement * speed, rigidBody.velocity.y);


        direction();
        CheckDash();
        if(health<=0)
        {
            isAllive = false;
            Instantiate(effectPlayer, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        
    }
    public void changeDirection()
    {
        if (movement > 0)
        {
            transform.localScale = new Vector3(0.5f, transform.localScale.y, transform.localScale.z);
        }
        else if (movement < 0)
        {
            transform.localScale = new Vector3(-0.5f, transform.localScale.y, transform.localScale.z);
        }
    }
    public void Jump()
    {
        if(isgrounded)
        {
            FindObjectOfType<AudioManager>().Play("Jump");
            rigidBody.AddForce(new Vector2(0, 23), ForceMode2D.Impulse);
            isgrounded = false;
        }
       
    }

    void OnCollisionEnter2D(Collision2D theCollision)
    {
        if (theCollision.gameObject.name.Equals("MovingPlatform"))
        {
            this.transform.parent = theCollision.transform;
        }

        if(theCollision.gameObject.CompareTag("Lava"))
        {
            FindObjectOfType<AudioManager>().Play("DeathOrTakeDMG");
            ScoreCount.isAlive = false;
            health = 0;
        }

        if (theCollision.gameObject.CompareTag("END"))
        {
            //AudioManager.sceneCount = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            
        }

        if (theCollision.gameObject.CompareTag("ENDT"))
        {
            SceneManager.LoadScene(0);
        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("MovingPlatform"))
        {
            this.transform.parent = null;
        }
    }

    public void Dash()
    {
        if (Time.time >= (lastDash + dashCooldown))
        {
            FindObjectOfType<AudioManager>().Play("Dash");
            AttemptToDash();
        }
        
    }


    private void AttemptToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;

        PlayerAfterImagePool.Instance.GetFromPool();
        lastImageXpos = transform.position.x;

    }
    private void CheckDash()
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                rigidBody.velocity = new Vector2(dashSpeed * facingDirection, rigidBody.velocity.y);
                dashTimeLeft -= Time.deltaTime;
                if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
                {
                    PlayerAfterImagePool.Instance.GetFromPool();
                    lastImageXpos = transform.position.x;
                }
            }
            if(dashTimeLeft <= 0)
            {
                isDashing = false;
            }
            
        }
    }
    private void direction()
    {
        if(transform.localScale == new Vector3(0.5f, transform.localScale.y,transform.localScale.z))
        {
            facingDirection = 1;
        }else if (transform.localScale == new Vector3(-0.5f, transform.localScale.y, transform.localScale.z))
        {
            facingDirection = -1;
        }
        
    }

    public void HitRestriction()
    {
        StartCoroutine(Hittable());
    }
    
    IEnumerator Hittable()
    {
        canGetHit = false;
        anim.SetTrigger("GotHit");
        yield return new WaitForSeconds(1);
        canGetHit = true;
        anim.ResetTrigger("GotHit");
    }

}
