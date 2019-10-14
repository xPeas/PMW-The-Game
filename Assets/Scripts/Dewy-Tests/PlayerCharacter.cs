using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Character;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public Camera gameCam;
    private CharacterController charController;
    private Transform camTrans;
    public Transform CharMesh;
    public Animator charAnimator;

    public float movementSpeed = 3f;
    public float acceleration = 3f;
    public float StoppingAcceleration = 9f;
    [Range(0f, 1f)]
    public float slideFriction = 0.5f;
    public float RotationSpeed = 90f;
    
    [Header("Jump Values")]
    public float groundedGravityMulti = 0.1f;
    public float JumpHeight = 5f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    
    [Header("Debug")] 
    public CollisionFlags moveFlags;

    public bool grounded;

    [SerializeField] private Vector3 _velocity;
    [SerializeField] private Vector3 floorNormal; 
    
    //Animation variables
    private static readonly int _animSpeed = Animator.StringToHash("speed");
    private static readonly int _animGrounded = Animator.StringToHash("isGrounded");
    private static readonly int _animHorizontal = Animator.StringToHash("horizontal");
    private static readonly int _animVertical = Animator.StringToHash("vertical");

    // Start is called before the first frame update
    void Start()
    {
        charController = GetComponent<CharacterController>();
        camTrans = gameCam.transform;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private Quaternion movementDiff;
    public Transform testCube;

    // Update is called once per frame
    void Update()
    {
        Vector2 input = Vector2.ClampMagnitude(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")),
            1.0f);
        Vector3 camForward = GetFlatVector(camTrans.forward);
        Vector3 camRight = GetFlatVector(camTrans.right);

        Vector3 movement = (camForward * input.y) + (camRight * input.x);
        Vector3 targetVelocity = movement * movementSpeed;

        if(Input.GetButtonDown("Jump") && grounded)
        {
            _velocity.y += Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y);
        }

        _velocity = Vector3.MoveTowards(_velocity, targetVelocity, 
            Time.deltaTime * (input.sqrMagnitude > 0f ? acceleration : StoppingAcceleration));
        _velocity += Physics.gravity * Time.deltaTime;
        
        Vector3 slopeMovement = Vector3.zero;
        //Adding Sliding Movement for slopes
        if (!grounded)
        {
            slopeMovement.x += (1f - floorNormal.y) * floorNormal.x * (1f - slideFriction);
            slopeMovement.z += (1f - floorNormal.y) * floorNormal.z * (1f - slideFriction);
        }
        
        moveFlags = charController.Move((_velocity * Time.deltaTime) + slopeMovement);
        
        //is grounded check
        grounded = ((moveFlags & CollisionFlags.CollidedBelow) != 0 && Vector3.Angle(floorNormal, Vector3.up) < charController.slopeLimit);
        
        if (grounded)
        {
            Vector3 newDir = Vector3.zero;
            
            if (Input.GetMouseButton(1))
            {
                //Keeping direction like lock on
                newDir = GetFlatVector(camForward);
            }
            else
            {
                //Rotate the Character Mesh towards the heading direction
                Vector3 heading = GetFlatVector(movement);
                float step = Mathf.Deg2Rad * RotationSpeed * Time.deltaTime;
                newDir = Vector3.RotateTowards(CharMesh.forward, heading, step, 0f);
            }
            
            CharMesh.rotation = Quaternion.LookRotation(newDir, Vector3.up);
        }
        
        //Update Animator values
        float CurrentSpeed = GetFlatVector(_velocity, false).magnitude / movementSpeed;
        Vector3 movementDir = GetFlatVector(_velocity);
        Vector3 charDir = CharMesh.forward;
        Vector3 animDir = Vector3.zero; 
        if (CurrentSpeed > 0.05f)
        {
            movementDiff = Quaternion.FromToRotation(charDir, movementDir);
            animDir = movementDiff * Vector3.forward;
            testCube.rotation = CharMesh.rotation * movementDiff;
            testCube.position = CharMesh.position;
        }
        
        Debug.LogFormat("Speed: {0}", CurrentSpeed);
        charAnimator.SetFloat(_animSpeed,CurrentSpeed);
        charAnimator.SetBool(_animGrounded, grounded);
        charAnimator.SetFloat(_animHorizontal, animDir.x);
        charAnimator.SetFloat(_animVertical, animDir.z);
        
        if (grounded && _velocity.y < 0f)
        {
            _velocity.y = Physics.gravity.y * groundedGravityMulti;
        }
        else if (_velocity.y < 0)
        {
            _velocity += Time.deltaTime * Physics.gravity.y * (fallMultiplier - 1) * Vector3.up;
        }
        else if (_velocity.y > 0 && !Input.GetButton("Jump"))
        {
            _velocity += Time.deltaTime * Physics.gravity.y * (lowJumpMultiplier - 1) * Vector3.up;
        }

        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            Cursor.lockState = CursorLockMode.None;
        }
        #endif    
    }

    private Vector3 GetFlatVector(Vector3 vector, bool normalize = true)
    {
        vector.y = 0f;
        return normalize ? vector.normalized : vector;
    }

    private float GetAngle(Vector3 a, Vector3 b)
    {
        float angle = Vector3.Angle(a, b);
        Vector3 cross = Vector3.Cross(a, b);
        if (cross.y < 0)
            angle = -angle;
        return angle;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        floorNormal = hit.normal;
    }
}
