using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Light _light;
    public Timer _timer;
    private AudioSource _audioSource;
    public AudioClip[] _clips;
    private CharacterController _characterController;
    [SerializeField] private Camera _camera;
    public float camSpeed;
    private float xRotation;
    private float inputCoolDown;    //avoid spamming the numbers
    private float inputCoolDownMax;
    public LayerMask _layerMask;
    private string inputCode = "";

    public TextAsset textFileWithCodes;
    private List<string> codeList;
    
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.Locked;
        _characterController = GetComponent<CharacterController>();
        inputCoolDownMax = .2f;
        
        //instantiate the list
        codeList = new List<string>();
        var codesFromFile = textFileWithCodes.text.Split('\n');
        // this code loops through every single line in the text file
        for (int i = 0; i < codesFromFile.Length; i++)
        {
            // add each line to the list of codelist
            codeList.Add(codesFromFile[i].ToUpper().Trim());
        }
    }

    // Update is called once per frame
    void Update()
    {
        //camera control
        float mouseX = Input.GetAxis("Mouse X") * camSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * camSpeed * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        transform.Rotate(Vector3.up * mouseX);
        _camera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        
        //input passwords
        if (inputCoolDown > 0)
        {
            inputCoolDown -= Time.deltaTime;
        }

        if (inputCoolDown <= 0)
        {
            inputCoolDown = 0;
        }
        
        Debug.DrawRay(_camera.transform.position, _camera.transform.TransformDirection(Vector3.forward) * 2f, Color.yellow);
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(_camera.transform.position, _camera.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, _layerMask))
        {
            Number _number = hit.collider.gameObject.GetComponent<Number>();
            if (_number != null)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (inputCoolDown == 0)
                    {
                        //audio
                        _audioSource.PlayOneShot(_clips[0]);
            
                        //get pressed keys
                        inputCode += _number.inputNumber;
                
                        //reset input cool down
                        inputCoolDown = inputCoolDownMax;
                    }
                    
                    //Check if the input code matches the access code
                    for (int i = 0; i < codeList.Count; i++)
                    {
                        if (inputCode == codeList[i])
                        {
                            _audioSource.PlayOneShot(_clips[1]);
                            Debug.Log("success");
                            _timer.stop = true;
                            inputCode = "";
                        }
                        else if (inputCode.Length >= 3)
                        {
                            _timer.timeRemaining -= 10;
                            _audioSource.PlayOneShot(_clips[2]);
                            inputCode = "";
                            Debug.Log("reset");
                        }
                    }
                }
            }
        }
        
        // game over
        if (_timer.timeRemaining == 0)
        {
            if (_light.intensity < 500)
            {
                _light.intensity += 100 * Time.deltaTime;
            }
        }
    }
}
