using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [HideInInspector] public float horizontalInput;
    private Animator anim;

    private bool animating;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        CrouchHeld();
        //DashPressed();
        //SprintingHeld();
        JumpPressed();
        JumpHeld();
        MovementPressed();
        WeaponFired();
        WeaponFiredHeld();
        Melee();
    }

    public virtual bool MovementPressed()
    {
        //Check if movement button is pressed
        if (SimpleInput.GetAxisRaw("Horizontal") >= .5f || SimpleInput.GetAxisRaw("Horizontal") <= -.5f)
        {
            horizontalInput = SimpleInput.GetAxisRaw("Horizontal");
            return true;
        }

        else
            return false;
    }

    public virtual bool CrouchHeld()
    {
        if (SimpleInput.GetAxisRaw("Vertical") < -.5f)
        {
            return true;
        }
        return false;
    }

    public virtual bool DashPressed()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            return true;
        }

        else
            return false;
    }

    public virtual bool SprintingHeld()
    {
        //Check if sprint button is pressed
        if (Input.GetKey(KeyCode.LeftShift))
            return true;

        else
            return false;
    }

    //Check if jump button is held
    public virtual bool JumpHeld()
    {
        if (SimpleInput.GetAxisRaw("Vertical") >= .3f)
        {
            return true;
        }
        else
            return false;
    }

    public virtual bool JumpPressed()
    {
        if (SimpleInput.GetAxisRaw("Vertical") >= .3f)
        {
            return true;
        }
        else
            return false;
    }

    public virtual bool WeaponFired()
    {
        if (SimpleInput.GetButtonDown("Fire3"))
        {
            return true;
        }
        else
            return false;
    }

    public virtual bool WeaponFiredHeld()
    {
        if (SimpleInput.GetButton("Fire3"))
        {
            return true;
        }
        else
            return false;
    }

    public virtual void Melee()
    {
        if (SimpleInput.GetButtonDown("Jump") && !animating)
            StartCoroutine(StartMelee());
    }

    private IEnumerator StartMelee()
    {
        animating = true;
        anim.SetBool("Melee", true);
        yield return new WaitForSeconds(.4f);
        animating = false;
        anim.SetBool("Melee", false);
    }

    public virtual bool ChangeWeaponPressed()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            return true;
        }
        else
            return false;
    }

    public virtual bool GamePausePressed()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            return true;
        }
        else
            return false;
    }
}
