using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] protected Joystick joystick;
    [HideInInspector] public float horizontalInput;
    private Animator anim;

    private void Awake()
    {
        if (joystick == null)
            joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<Joystick>();
    }

    private void Update()
    {
        if (joystick)
        {
            CrouchHeld();
            DashPressed();
            SprintingHeld();
            JumpPressed();
            JumpHeld();
            MovementPressed();
        }
<<<<<<< Updated upstream
        WeaponFired();
=======

        WeaponFired();
        WeaponFiredHeld();
>>>>>>> Stashed changes
    }

    public virtual bool MovementPressed()
    {
        //Check if movement button is pressed
        if (joystick.Horizontal != 0)
        {
            horizontalInput = joystick.Horizontal;
            return true;
        }

        else
            return false;
    }

    public virtual bool CrouchHeld()
    {
        if (joystick.Vertical <= -.5f)
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
        if (joystick.Vertical >= .5f)
        {
            return true;
        }
        else
            return false;
    }

    public virtual bool JumpPressed()
    {
        if (joystick.Vertical >= .5f)
        {
            return true;
        }
        else
            return false;
    }

    public virtual bool WeaponFired()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            return true;
        }
        else
            return false;
    }

<<<<<<< Updated upstream
=======
    public virtual bool WeaponFiredHeld()
    {
        if (Input.GetButton("Fire1"))
        {
            return true;
        }
        else
            return false;
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

>>>>>>> Stashed changes
}
