using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMov : MonoBehaviour
{
    private CharacterController characterController;
    private Animator anim;
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float turnSpeed = 190;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        var movement = new Vector3(horizontal, 0, vertical);

        anim.SetFloat("Speed",vertical);
        transform.Rotate(Vector3.up,horizontal * turnSpeed *Time.deltaTime);

        if (vertical != 0)
        {
            characterController.SimpleMove(transform.forward * moveSpeed * vertical);
        }

        ///***
        ///movimiento en coordenadas globales, no sirve a menos que la cámara sea fija
        ///
        //characterController.SimpleMove(movement * Time.deltaTime * moveSpeed);

        //anim.SetFloat("Speed", movement.magnitude);
        //if(movement.magnitude > 0) { 
        //    Quaternion newDirection = Quaternion.LookRotation(movement);
        //    transform.rotation = Quaternion.Slerp(transform.rotation,newDirection,Time.deltaTime * rotationSpeed);
        //}     
        ///***

    }
}
