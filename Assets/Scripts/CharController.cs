using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour {

    [SerializeField] private float m_MaxSpeed = 7f;                    // The fastest the player can travel in the x axis.
    [SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
    [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
    [SerializeField] private string hAxis = "Horizontal";
    [SerializeField] private string vAxis = "Vertical";
    [SerializeField] private string bJump = "Jump";

    private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
    private Transform m_CeilingCheck;   // A position marking where to check for ceilings
    const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
    private Animator m_Anim;            // Reference to the player's animator component.
    private Rigidbody2D m_Rigidbody2D;

    private void Awake()
    {
        // Setting up references.
        m_GroundCheck = transform.Find("GroundCheck");
        m_CeilingCheck = transform.Find("CeilingCheck");
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                m_Grounded = true;
        }
        m_Anim.SetBool("Ground", m_Grounded);

        // Set the vertical animation
        //m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
        
        if (m_Grounded && Input.GetButtonDown(bJump))
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            m_Anim.SetBool("Ground", false);
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }

        Vector3 theScale = transform.localScale;
        if (theScale.x > 0 && Input.GetAxis(hAxis) > 0)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (theScale.x < 0 && Input.GetAxis(hAxis) < 0)
        {
            // ... flip the player.
            Flip();
        }
        
        //cap the speed
        if (m_Rigidbody2D.velocity.x < m_MaxSpeed)
        {
            m_Rigidbody2D.AddForce(new Vector3(Input.GetAxis(hAxis) * m_MaxSpeed, 0, 0));
        }
        m_Anim.SetFloat("Hspeed", System.Math.Abs(Input.GetAxis(hAxis) * m_MaxSpeed));

    }

    private void Flip()
    {
        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}