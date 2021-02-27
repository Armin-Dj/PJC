using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Start variables
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;

    public int cherries = 0;
    //State machine
    private enum State {idle, running, jumping, falling}
    private State state = State.idle;

    //Inspector variables
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 7f;
    [SerializeField] private float jumpForce = 8f;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        MovementManager();

        AnimationState();
        anim.SetInteger("state", (int)state); //sets animation based on enum state
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Collectable")
        {
            Destroy(collision.gameObject);
            cherries++;
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
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            state = State.jumping;
        }
    }

    private void AnimationState()
    {
        if (state == State.jumping)
        {
            if(rb.velocity.y < .1f)
            {
                state = State.falling;
            }
        }
        else if ( state == State.falling)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }
        else if ( (state == State.idle || state == State.running) && coll.IsTouchingLayers(ground) == false)
        {
            state = State.falling;
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
}
