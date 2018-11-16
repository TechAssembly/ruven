using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayerMovement : MonoBehaviour
{

    public float MoveSpeed = 5.0f;
    public Camera childCamera = null;
    public float LookSensitivity = 3.0f; //amount of look per mouse move, higher look faster
    public float LookSmooth = 2.0f; //less jagged look movment
    private Vector2 LookDirection;
    private float CameraDegree = -90f; //use 25 for 3rd person, -90 for top down
    private string MoveType = "";
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        // we might need this based on the platform just for development
        Cursor.lockState = CursorLockMode.Locked;
        //getting the reference to the animator
        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //if its enabled in the start method we need this line too just for development
        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
        	Cursor.lockState = CursorLockMode.None;
        }

        ControlMovement();
        ControlLookAround();
        ControlSpecialMoves();

        void ControlMovement()
        {
            float xAxisMove = Input.GetAxis("Horizontal");
            float zAxisMove = Input.GetAxis("Vertical");

          

            //press shift and character walks
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                MoveSpeed = 2.0f;
                MoveType = "Walk";
            }
            //raise shift and character runs
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                MoveSpeed = 5.0f;
                MoveType = "Run";
            }

            this.transform.Translate(xAxisMove * MoveSpeed * Time.deltaTime, 0.0f, zAxisMove * MoveSpeed * Time.deltaTime);
            
            if(xAxisMove != 0.0f || zAxisMove != 0.0f)
            {
                anim.SetBool("isRunning", true);
            }
            else
            {
                anim.SetBool("isRunning", false);
            }
        }

        void ControlLookAround()
        {
            //mouse movement
            Vector2 mouseDirection = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            mouseDirection = Vector2.Scale(mouseDirection, new Vector2(LookSensitivity, LookSensitivity));

            Vector2 LookDelta = new Vector2();
            LookDelta.x = Mathf.Lerp(LookDelta.x, mouseDirection.x, 1.0f / LookSmooth);
            LookDelta.y = Mathf.Lerp(LookDelta.y, mouseDirection.y, 1.0f / LookSmooth);
            LookDirection += LookDelta;

            //limit look up and down (use for 3rd person)
            LookDirection.y = Mathf.Clamp(LookDirection.y, -30.0f, 0f);

            //use this for top down, 3rd person
            //LookDirection.y = Mathf.Clamp(LookDirection.y, CameraDegree, CameraDegree);

            //move camera up and down
            childCamera.transform.localRotation = Quaternion.AngleAxis(-LookDirection.y, Vector3.right);

            //rotate player
            this.transform.localRotation = Quaternion.AngleAxis(LookDirection.x, this.transform.up);
        }
    }

    private void ControlSpecialMoves()
    {
        //jump with space
        if (Input.GetKey(KeyCode.Space))
        {
            anim.SetBool("isJumping", true);
        }
        else
        {
            anim.SetBool("isJumping", false);
        }
    }
}
