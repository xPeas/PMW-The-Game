using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTest : MonoBehaviour
{
    public float standingGravityDivider = 3f;

    private CharacterController charController;

    private Vector3 velocity;
    
    // Start is called before the first frame update
    void Start()
    {
        charController = GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //Jump code here
        
        //if(velocity.)
    }
}
