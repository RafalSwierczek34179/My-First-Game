using System;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    //players transform component, allows this class to use getcomponent<> to use player components
    public Transform player;

    //used to elevate camera 0.7 above players centre
    private Vector3 offset = new Vector3(0f, 0.7f, 0f);

    //camera rotation stuff
    [SerializeField] float _mouseSensitivity = 100f;
    float _xRotation = 0f;
    float _yRotation = 180f;
    float _zRotation = 0f;

    //camera FOV stuff
    float _originalZoom = 90f;

//-------------------------------------------Methods--------------------------------------------------------------------------------

    void Start()
    {
       Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = player.position + offset;//Camera position
       
        MouseMovement();
        
    }

    void MouseMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);//stops camera from looking too far up and down

        _yRotation += mouseX;

        transform.localRotation = Quaternion.Euler(_xRotation, _yRotation, _zRotation);//updates camera rotation


        //used to update the character rotation right after updating camera rotation
        Player _updatePlayer = player.transform.GetComponent<Player>();
        _updatePlayer.CharacterRotation(_yRotation);
        

    }

    public void SprintZoom()//ran from player class
    {
        float _tempClamp = Camera.main.fieldOfView;
        _tempClamp -= 8 * Time.deltaTime;
        _tempClamp = Mathf.Clamp(_tempClamp, 74f, 90f);
        Camera.main.fieldOfView = _tempClamp;
        //Camera.main.fieldOfView -= 8 * Time.deltaTime;//decreases fov by 8 every second
    }

    public void NormalZoom()//ran from player class
    {
        _originalZoom = Camera.main.fieldOfView;//gets the current fov of camera
        _originalZoom += 50 * Time.deltaTime;//increases the current fov by 50 every second
        _originalZoom = Mathf.Clamp(_originalZoom, 0f, 90f);//prevents the value from exceeding 90
        Camera.main.fieldOfView = _originalZoom;//increases fov by 50 every second and caps at 90
    }

    public void LeftMovement()
    {
        _zRotation += 10f * Time.deltaTime;
        _zRotation = Mathf.Clamp(_zRotation, -2f, 1.8f);
    }

    public void RightMovement()
    {
        _zRotation -= 10f * Time.deltaTime;
        _zRotation = Mathf.Clamp(_zRotation, -1.8f, 2f);
    }

    public void CamTiltReset()
    {
        if (_zRotation >= 0f)
        {
            _zRotation -= 10f * Time.deltaTime;
            _zRotation = Mathf.Clamp(_zRotation, 0f, 2f);
        }
        else
        {
            _zRotation += 10f * Time.deltaTime;
            _zRotation = Mathf.Clamp(_zRotation, -2f, 0f);
        }
    }
}
