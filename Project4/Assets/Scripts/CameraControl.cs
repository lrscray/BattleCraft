using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    //Set our ground layer
   public LayerMask groundLayer;
    //Classes for our settings

    [System.Serializable]
    public class PositionSettings
    {
        //allow for pan in oposite direction when mouse pressed. Can be changed in game
        public bool invertPan = true;
        //Speed of panning calculations
        public float panSmooth = 7f;
        //How far up from the ground camera starts at
        public float distanceFromGround = 40;
        //allow user to zoom in
        public bool allowZoom = true;
        public float zoomSmooth = 20;
        //increments of zooming
        public float zoomStep = 20;
        public float maxZoom = 25;
        public float minZoom = 80;

        [HideInInspector]
        public float newDistance = 40;

    }

    //Settings for turning and rotation
    [System.Serializable]
    public class OrbitSettings
    {
        //angling the camera down
        public float ogRotation = 50;

        //This will be modified through inputs
        public float xRotation = 0;
        public float yRotation = 0;
        //allow modification of y rotation
        public bool allowOrbit = true;
        //rotation speed
        public float RotationSpeed = 10.0f;
    }

    //Modifying inputs for zooming, panning, etc..
    [System.Serializable]
    public class InputSettings
    {
        //setting inputs to allow for actions to be made
        public string PAN = "MousePan";
        public string ORBIT_Y = "MouseTurn";
        //public string ORBIT_X = "MouseTurn";
        public string ZOOM = "Mouse ScrollWheel";
    }

    public PositionSettings position = new PositionSettings();
    public OrbitSettings orbit = new OrbitSettings();
    public InputSettings input = new InputSettings();

    Vector3 destination = Vector3.zero;
    Vector3 camvel = Vector3.zero;
    Vector3 previousMousepos = Vector3.zero;
    Vector3 currentMousePos = Vector3.zero;
    float panInput, orbitInput, zoomInput, orbitInput2;
    int panDirection = 0;

    void Start()
    {
        panInput = 0;
        orbitInput = 0;
        //orbitInput2 = 0;
        zoomInput = 0;
    }

    //setting input variables
    void GetInput()
    {
        panInput = Input.GetAxis(input.PAN);
        orbitInput = Input.GetAxis(input.ORBIT_Y);
        //orbitInput2 = Input.GetAxis(input.ORBIT_X);
        zoomInput = Input.GetAxis(input.ZOOM);

        previousMousepos = currentMousePos;
        currentMousePos = Input.mousePosition;
    }

    void Update()
    {
        GetInput();

        if (position.allowZoom)
        {
            Zoom();
        }
        if (orbit.allowOrbit)
        {
            Rotate();
        }
        PanWorld();
    }

    //handle camera distance
    void FixedUpdate()
    {
        HandleCameraDistance();
    }

    void PanWorld()
    {
        Vector3 targetPos = transform.position;

        if (position.invertPan)
        {
            panDirection = -1;
        }
        else
        {
            panDirection = 1;
        }

        if (panInput > 0)
        {
            //HandleCameraDistance x component and z component of panning
            targetPos += transform.right * (currentMousePos.x - previousMousepos.x) * position.panSmooth * panDirection * Time.deltaTime;
            //Cross allows us to get and derive our local forward vector
            targetPos += Vector3.Cross(transform.right, Vector3.up) * (currentMousePos.y - previousMousepos.y) * position.panSmooth * panDirection * Time.deltaTime;
        }
        transform.position = targetPos;
    }

    void HandleCameraDistance()
    {
        //allows our camera to maintain distance from our set ground
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        //maintain our distance from ground layer
        if (Physics.Raycast(ray, out hit, 100, groundLayer))
        {
            //EVEN with hills!!!!.... we maintain distance
            destination = Vector3.Normalize(transform.position - hit.point) * position.distanceFromGround;
            destination += hit.point;

            transform.position = Vector3.SmoothDamp(transform.position, destination, ref camvel, 0.3f);
        }
    }

    void Zoom()
    {
        //modifying distance from ground variable
        position.newDistance += position.zoomStep * -zoomInput;

        position.distanceFromGround = Mathf.Lerp(position.distanceFromGround, position.newDistance, position.zoomSmooth * Time.deltaTime);

        //Check zoom stays inside bounds
        if (position.distanceFromGround < position.maxZoom)
        {
            position.distanceFromGround = position.maxZoom;
            position.newDistance = position.maxZoom;
        }

        if (position.distanceFromGround > position.minZoom)
        {
            position.distanceFromGround = position.minZoom;
            position.newDistance = position.minZoom;
        }
    }

    void Rotate()
    {
        //check if pressing our orbit key
        if (orbitInput > 0)
        {
            orbit.yRotation += (currentMousePos.x - previousMousepos.x) * orbit.RotationSpeed * Time.deltaTime;
        }

       /* if (orbitInput2 > 0)
        {
            orbit.xRotation += (currentMousePos.y - previousMousepos.y) * orbit.RotationSpeed * Time.deltaTime;
        }*/

        transform.rotation = Quaternion.Euler(orbit.ogRotation, orbit.yRotation, 0);
       // transform.rotation = Quaternion.Euler(orbit.xRotation, orbit.yRotation, 0);
    }
}
