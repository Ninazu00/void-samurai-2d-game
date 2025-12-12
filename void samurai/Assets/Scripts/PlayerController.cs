using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float moveSpeed; //how fast the character moves
    public float jumpHeight; //how high the character jumps
    //private bool IsFacingRight; //check if player is facing right
    public KeyCode Spacebar; //Jump is the name we gave a keyboard button we chose to be the jump button. In this case, 
    //we chose the Space button, and called it Spacebar. To allocate the Space button to the name Spacebar, go to the Script 
    //component of your player character, and choose Space from the dropdown list
    public KeyCode L;//L is the name we gave a keyboard button we chose to be the left movement button.
    public KeyCode R;//R is the name we gave a keyboard button we chose to be the right movement button.
    public Transform groundCheck; //an invisible point in space. We use it to see if the player is touching the ground
    public float groundCheckRadius; //a value to determine how big the circle around the player's feet is, and therefore determine how
    //close he is to the ground
    public LayerMask whatIsGround; //this variable stores what is considered a ground to the character
    private bool grounded; //check if the character is standing on solid ground;
    // Use this for initialization
    private Animator anim;
	void Start () {
        anim =GetComponent<Animator>();
    }
	// Update is called once per frame
	void Update () {
        //anim.SetFloat("Speed",Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x));
        //anim.SetFloat("Height", GetComponent<Rigidbody2D>().velocity.y);
        //anim.SetBool("Ground", grounded);
        if(Input.GetKeyDown(Spacebar) && grounded) //When user presses the space button ONCE
        {
            Jump(); //see function definition below   
        }
       
        if (Input.GetKey(L)) //When user presses the left arrow button
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-moveSpeed, GetComponent<Rigidbody2D>().velocity.y); 
            //player character moves horizontally to the left along the x-axis without disrupting jump

            if(GetComponent<SpriteRenderer>()!=null)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }           
        }
      
        if (Input.GetKey(R)) //When user presses the left arrow button
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed, GetComponent<Rigidbody2D>().velocity.y); 
            //player character moves horizontally to the right along the x-axis without disrupting jump

            if(GetComponent<SpriteRenderer>()!=null)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }   
        }
    }

    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround); //this statement calculates when 
        //exactly the character is considered by Unity's engine to be standing on the ground. 
    }

    void Jump()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpHeight); //player character jumps 
        //vertically along the y-axis without disrupting horizontal walk      
    }
}
