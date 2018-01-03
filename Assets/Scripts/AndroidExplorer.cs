using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AndroidExplorer : MonoBehaviour
{
    #region Public Members

    public GameObject m_cube;
    public float m_speed;
    public Text m_acc;
    public Text m_compass;
    public Text m_gyro;
    public Text m_lat;
    public Text m_long;

    #endregion

    #region Public void

    #endregion

    #region System

    private void Awake()
    {
    }

    void Start()
    {
        // Initialize vars
        m_renderer = m_cube.GetComponent<Renderer>();
        m_transform = m_cube.GetComponent<Transform>();
        Input.gyro.enabled = true;
        Input.compass.enabled = true;

        Input.location.Start();

        // First, check if user has location service enabled
        //if (!Input.location.isEnabledByUser)
        //{
        //    Debug.Log("Position not enabled");
        //    m_data = ("Location disabled");
        //    yield break;
        //}
        //else { Debug.Log("Position enabled"); m_data = "position enabled"; }

        //// Start service before querying location
        //Debug.Log("Start location service");
        //Input.location.Start();

        //// Wait until service initializes
        //int maxWait = 20;
        //while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        //{
        //    yield return new WaitForSeconds(1);
        //    maxWait--;
        //    Debug.Log(maxWait);
        //    m_data = maxWait.ToString();
        //}

        //// Service didn't initialize in 20 seconds
        //if (maxWait < 1)
        //{
        //    Debug.Log("Timed out");
        //    yield break;
        //}

        //// Connection has failed
        //if (Input.location.status == LocationServiceStatus.Failed)
        //{
        //    Debug.Log("Unable to determine device location");
        //    yield break;
        //}
        //else
        //{
        //    // Access granted and location value could be retrieved
        //    Debug.Log("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        //}

        // Stop service if there is no need to query location updates continuously
        ////Input.location.Stop();
    }
	
	void Update()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        m_isPlayerPressing = Input.touchCount > 0;

        
        m_lat.text = Input.location.lastData.latitude.ToString(); // Latitude
        m_long.text = Input.location.lastData.longitude.ToString();
#else
        m_isPlayerPressing = Input.GetMouseButtonDown(0);
#endif
        if (m_isPlayerPressing)
        {
            Touch touch = Input.GetTouch(0); // get first touch since touch count is greater than zero

            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                // get the touch position from the screen touch to world point
                Vector3 touchedPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
                // lerp and set the position of the current object to that of the touch, but smoothly over time.
                transform.position = Vector3.Lerp(transform.position, touchedPos, Time.deltaTime);
            }
            if (m_released)
            {
                m_released = false;
                Color newColor = new Color(Random.value, Random.value, Random.value, 1.0f);
                m_renderer.material.color = newColor;
            }
        }
        else m_released = true;


        //RotateX(Input.gyro.rotationRateUnbiased.x);
        RotateY(Input.gyro.rotationRateUnbiased.y);
        RotateZ(Input.gyro.rotationRateUnbiased.z);
        
        PrintInfo(Input.location.isEnabledByUser.ToString());
    }


    private void RotateX(float axis)
    {
        transform.RotateAround(transform.position, new Vector3(1, 0, 0), -axis * Time.deltaTime * 50);
    }
    private void RotateY(float axis)
    {
        transform.RotateAround(transform.position, new Vector3(0, 1, 0), -axis * Time.deltaTime * 50);
    }
    private void RotateZ(float axis)
    {
        transform.RotateAround(transform.position, new Vector3(0, 0, 1), -axis * Time.deltaTime * 50);
    }



    #endregion

    #region Tools Debug And Utility

    private void PrintInfo(string data)
    {
        m_acc.text = Input.acceleration.ToString(); // acceleration
        m_compass.text = Input.compass.magneticHeading.ToString(); // direction magnétique
        m_gyro.text = data; // gyroscope
        //m_gyro.text = Input.gyro.attitude.ToString(); // gyroscope
        m_lat.text = Input.location.lastData.latitude.ToString(); // Latitude
        m_long.text = Input.location.lastData.longitude.ToString();
    }

    private string m_data;

    #endregion

    #region Private an Protected Members

    private bool m_isPlayerPressing;
    private bool m_released = true;
    private int m_touchCount;
    private Renderer m_renderer;
    private Transform m_transform;

    #endregion
}