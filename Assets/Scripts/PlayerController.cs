using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    //Start variables
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;
    //State machine
    private enum State { idle, running, jumping, falling, hurt }
    private State state = State.idle;

    //Inspector variables
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float hurtForce;
    [SerializeField] private AudioSource footstep;
    [SerializeField] private AudioSource cherry;
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource powerupSound;
    [SerializeField] private AudioSource hurtSound;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        PermanentUI.perm.hpAmmount.text = PermanentUI.perm.hp.ToString();

    }

    // Update is called once per frame
    private void Update()
    {
        if (state != State.hurt)
        {
            MovementManager();
        }
        AnimationState();
        anim.SetInteger("state", (int)state); //sets animation based on enum state
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectable")
        {
            cherry.Play();
            Destroy(collision.gameObject);
            PermanentUI.perm.cherries++;
            PermanentUI.perm.cherryText.text = PermanentUI.perm.cherries.ToString();
        }
        if (collision.tag == "Powerup")
        {
            Destroy(collision.gameObject);
            PowerupSound();
            jumpForce = 20f;
            speed = 15f;
            GetComponent<SpriteRenderer>().color = Color.yellow;
            StartCoroutine(ResetPower());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (state == State.falling)
            {
                enemy.JumpedOn();
                Jump();
            }
            else
            {
                state = State.hurt;
                HurtSound();
                HandleHP();
                if (collision.gameObject.transform.position.x > transform.position.x)
                {
                    //enemy e in dreapta, damage + arunca stanga
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                }
                else
                {
                    //enemy e in stanga, damage + arunga dreapta
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                }
            }
        }
    }

    private void HandleHP()
    {
        PermanentUI.perm.hp -= 1;
        PermanentUI.perm.hpAmmount.text = PermanentUI.perm.hp.ToString();
        if (PermanentUI.perm.hp <= 0)
        {
            PermanentUI.perm.hp = 5;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void MovementManager()
    {
        float hDirection = Input.GetAxis("Horizontal");


        //Move left
        if (hDirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }

        //Move right
        else if (hDirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }

        //Jump
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.jumping;
    }

    private void AnimationState()
    {
        if (state == State.jumping)
        {
            if (rb.velocity.y < .1f)
            {
                state = State.falling;
            }
        }
        else if (state == State.falling)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }
        //script sa verifice daca caracterul cade in timp ce alearga sau sta pe loc, si daca da, sa schimbe state-ul in falling
        else if ((state == State.idle || state == State.running) && coll.IsTouchingLayers(ground) == false)
        {
            state = State.falling;
        }
        else if (state == State.hurt)
        {
            if (Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle;
            }
        }
        else if (Mathf.Abs(rb.velocity.x) > 2f)
        {
            //mers dreapta
            state = State.running;
        }
        else
        {
            state = State.idle;
        }
    }

    private IEnumerator ResetPower()
    {
        yield return new WaitForSeconds(10);
        jumpForce = 15f;
        speed = 7f;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void PowerupSound()
    {
        powerupSound.Play();
    }
    private void JumpSound()
    {
        jumpSound.Play();
    }
    private void Footstep()
    {
        footstep.Play();
    }
    private void HurtSound()
    {
        hurtSound.Play();
    }
}
