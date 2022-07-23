using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputWithAnimations : MonoBehaviour
{
    public Transform camera;
    public Transform groundCheck;
    public LayerMask groundMask;
    public CinemachineFreeLook freeLook;

    CharacterController charCont;
    [SerializeField]
    private float speed;
    public float runningSpeed = 4f;
    public float walkingSpeed = 2f;
    private float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;
    public float gravity = -9.81f;
    public float groundDistance = 0.2f;
    public float jumpForce = 3f;

    public float deadZoneHeightGrounded;
    public float deadZoneHeightNoGrounded;
    public float vDampingNoGrounded;
    public float vDampingGrounded;

    public Animator anim;

    private Vector3 velocity;
    private bool isGrounded;

    public Vector3 actualSpeed;
    public float normalizadSpeed;


    // Start is called before the first frame update
    void Start()
    {
        charCont = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position,groundDistance,groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(hInput,0,vInput).normalized;
        
        if (direction.magnitude>=0.1f)
        {

            float targetAngle = Mathf.Atan2(direction.x,direction.z)* Mathf.Rad2Deg + camera.eulerAngles.y;//calcula el ángulo en el que mira la cámara
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime); 
            transform.rotation = Quaternion.Euler(0f,angle,0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            charCont.Move(moveDir.normalized*speed*Time.deltaTime);
        }

        //le pasamos el valor de la velocidad del character controller al animator (antes de aplicar gravedad)
        anim.SetFloat("speed", charCont.velocity.magnitude);
        actualSpeed = charCont.velocity;
        normalizadSpeed = charCont.velocity.magnitude;

        //calcula la gravedad y mueve el controller en base a la misma
        velocity.y += gravity * Time.deltaTime;
        charCont.Move(velocity * speed * Time.deltaTime);

        //si toca la barra espaciadora, salta
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2.0f * gravity);
            anim.SetTrigger("jump");
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = runningSpeed;
            anim.SetBool("isRunning", true);
        }
        else
        {
            speed = walkingSpeed;
            anim.SetBool("isRunning", false);
        }

        CinemachineVirtualCamera topRig = freeLook.GetRig(0);
        CinemachineVirtualCamera middleRig = freeLook.GetRig(1);
        CinemachineVirtualCamera bottomRig = freeLook.GetRig(2);

        var composerTop = topRig.GetCinemachineComponent<CinemachineComposer>();
        var composerMid = middleRig.GetCinemachineComponent<CinemachineComposer>();
        var composerBot = bottomRig.GetCinemachineComponent<CinemachineComposer>();

        if (!isGrounded)
        {
            composerTop.m_DeadZoneHeight = deadZoneHeightNoGrounded;
            composerTop.m_VerticalDamping = vDampingNoGrounded;
            composerBot.m_DeadZoneHeight = deadZoneHeightNoGrounded;
            composerBot.m_VerticalDamping = vDampingNoGrounded;
            composerMid.m_DeadZoneHeight = deadZoneHeightNoGrounded;
            composerMid.m_VerticalDamping = vDampingNoGrounded;
        }
        else
        {
            composerTop.m_DeadZoneHeight = deadZoneHeightGrounded;
            composerTop.m_VerticalDamping = vDampingGrounded;
            composerBot.m_DeadZoneHeight = deadZoneHeightGrounded;
            composerBot.m_VerticalDamping = vDampingGrounded;
            composerMid.m_DeadZoneHeight = deadZoneHeightGrounded;
            composerMid.m_VerticalDamping = vDampingGrounded;
        }

    }
}
