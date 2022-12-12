using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Animations2 : MonoBehaviour
{
    public Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Vertical") != 0 | Input.GetAxis("Horizontal") != 0)
        {
            anim.SetBool("isWalk", true);
        }
        else
        {
            anim.SetBool("isWalk", false);
        }

        if (Movement.isSprinting == true)
        {
            anim.SetBool("isSprint", true);
        } else
        {
            anim.SetBool("isSprint", false);
        }

        if (Movement.jumping == false)
        {
            anim.SetBool("isJump", false);
        } else
        {
            anim.SetBool("isJump", true);
        }

        if (Movement.isCrouching == false)
        {
            anim.SetBool("isCrouch", false);
        }
        else
        {
            anim.SetBool("isCrouch", true);
        }
    }
}
