using UnityEngine;
using System.Collections;

public class CharacterMotor : MonoBehaviour 
{
    CharacterController controller;
    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float sprintSpeed = 12;
    public float gravity = 20.0F;
    
    private Vector3 moveDirection = Vector3.zero;

    public AudioClip stepSound;
    float stepSoundFrequency = 0.1f;
    float curTime;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        curTime = stepSoundFrequency * speed;
    }

    void Update()
    {
        UpdatePosition();

    }

    void UpdatePosition()
    {
        if (!PauseSystem.IsPaused)
        {
            if (controller.isGrounded)
            {
				moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                moveDirection = transform.TransformDirection(moveDirection);

				if(moveDirection != Vector3.zero)
					moveDirection.Normalize();

                if(Input.GetButton("Sprint"))
                    moveDirection *= sprintSpeed;
                else
                    moveDirection *= speed;
                if (Input.GetButton("Jump"))
                    moveDirection.y = jumpSpeed;

            }
            moveDirection.y -= gravity * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime);

            if(moveDirection.x > 0 || moveDirection.z > 0)
                PlayStepSound();
        }
    }

    void PlayStepSound()
    {
        if (curTime > 0)
        {
            curTime -= Time.deltaTime;
        }
        if (curTime <= 0)
        {
            if(Input.GetButton("Sprint"))
                curTime = (stepSoundFrequency / 2) * speed;
            else
                curTime = stepSoundFrequency * speed;
            SoundManager.instance.Spawn3DSound(stepSound, Player.instance.transform.position, 1, 5);
        }
    }
}
