using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.XR;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //Normal movement
    public float _playerSpeed = 60f;
    float _playerSpeedBackwards;
    [SerializeField] float _playerSideSpeed = 60f;

    //to do with sprinting
    public float _originalSpeed;
    public float _sprintspeed;
    public float _originalSprintSpeed;
    public float _stamina = 2;
    bool _staminaAvailable = true;
    bool _sprintOn = true;

    //character rotation
    public Transform _cameraBody;//also used for camera fov zoom effect when sprinting
    float _yCharacterRotation = 180f;
    
    //so that i can add forces to the characters body
    public Rigidbody _characterPhysics;

    public bool _abilityReady = false;

    public Slider _staminaBar_R;
    public Slider _staminaBar_L;
//----------------------------------------------------Methods--------------------------------------------------------------------------------------

    private void Start()
    {
        _playerSpeedBackwards = _playerSpeed * -1;
       
        _originalSpeed = _playerSpeed;//temp attribute to store original player speed in cases it changes and needs to change back

        _sprintspeed = _playerSpeed * 1.5f;//the sprint speed is set to be 50% larger than normal speed, only once at the beginning so it doesn't grow exponentially with every new drawn frame

        _originalSprintSpeed = _sprintspeed;
    }


    void FixedUpdate()
    {
        CharacterMovement();
        _staminaBar_R.value = _stamina;//updates size of stamina bar based on stamina float value
        _staminaBar_L.value = _stamina;//two stamina bars as I have a mirrored effect going on, this makes one stamina bar which shrinks inwards
    }

    public void CharacterRotation(float _yInput)
    {
        _yCharacterRotation = _yInput;//stores the _yRotationValue from MainCamera class
        transform.localRotation = Quaternion.Euler(0f, _yCharacterRotation, 0f);//updates the character rotation every time the camera y rotation value changes
    }

 

    void CharacterMovement()
    {
        MainCamera _mainCam = _cameraBody.transform.GetComponent<MainCamera>();//stores the main camera class as _sprintZoom var so that this class can access its public methods

        if (Input.GetKey(KeyCode.LeftShift) == true)
        {
            _playerSpeed = _sprintspeed;//changes players normal speed to sprint speed regardless if the player is tired, it then checks if player is tired to turn it back to original speed

            if (_sprintOn == true)
            {
                if (Input.GetKey(KeyCode.W) == true)//if player is not tired AND is pressing w then stamina is reduced and camera fov effect is applied
                {//validation so that player doesnt lose stamina if they're holding the sprint button but not going forward
                    _staminaAvailable = Stamina();
                    _mainCam.SprintZoom();

                }
            }

            if (_staminaAvailable == false)//if last frame the player became tired and player is still attempting to sprint, then turn their speed back to normal, start stamina regen and reverse fov effect
            {
                _playerSpeed = _originalSpeed;
                StaminaRegen();
                _mainCam.NormalZoom();
            }
            
        }
        else
        {

            if (_stamina < 2f)//this is so that if the player isnt holding sprint they can regen stamina and reverse the camera fov effect in case they just stopped sprinting
            { 
                StaminaRegen();
                _mainCam.NormalZoom();
            }
        }

        //-------------------------Character Movement--------------------------------------------------

        if (Input.GetKey(KeyCode.W) == true)
        {
            _characterPhysics.AddRelativeForce(new Vector3(0f, 0f, _playerSpeed * Time.deltaTime), ForceMode.Impulse);
            _playerSpeed = _originalSpeed;
            //turns sprint back to original incase the player stops holding sprint by next frame, if not then the speed will become sprint speed before taking effect anyway
        }
        if (Input.GetKey(KeyCode.S) == true)
        {
            _characterPhysics.AddRelativeForce(new Vector3(0f, 0f, _playerSpeedBackwards * Time.deltaTime), ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.D) == true)
        {
            _characterPhysics.AddRelativeForce(new Vector3(_playerSideSpeed * Time.deltaTime, 0f, 0f), ForceMode.Impulse);
            _mainCam.RightMovement();
        }
        if (Input.GetKey(KeyCode.A) == true)
        {
            _characterPhysics.AddRelativeForce(new Vector3(-_playerSideSpeed * Time.deltaTime, 0f, 0f), ForceMode.Impulse);
            _mainCam.LeftMovement();
        }
        if (Input.GetKey(KeyCode.D) == false && Input.GetKey(KeyCode.A) == false)
        {
            _mainCam.CamTiltReset();
        }
        if (Input.GetKey(KeyCode.Space) == true && _abilityReady == true)//checks if the ability is ready when player presses the ability activation button
        {
            _abilityReady = false;
            Abilities _abilities = transform.GetComponent<Abilities>();
            if (_abilities._playersAbility == "Time Stop" || _abilities._playersAbility == "Predator")//Damnger zone never deactivates therefore this must always be turned off to allow player to pick up points
            {
                _abilities._abilityActive = true;//This stops the player from picking up points until their ability is turned off
            }
            _abilities.AbilityActivation();
        }
        //------------------------------------------------------------------------------------------------
    }


    bool Stamina()//_stamina is set to 2 by default
    {
        _stamina -= 1 * Time.deltaTime;//reduces stamina by 1 every second
        if (_stamina <= 0f)
        {
            _sprintOn = false;//disables sprinting until player has full stamina, this is so that players get punished if they completely drain stamina
            return false;//used to disable sprinting in this frame only
        }
        else
        {
            return true;
        }
    }

    void StaminaRegen()
    {
        _stamina += 0.5f * Time.deltaTime;//regens stamina by 0.5 per second, so it takes 4 seconds to regen a completely drained stamina
        _stamina = Mathf.Clamp(_stamina, 0f, 2f);//clamps stamina between 0 and 2
        if (_stamina >= 2)
        {
            _sprintOn = true;//if stamina full then allows players that completely drained it to sprint again
        }
    }
}
