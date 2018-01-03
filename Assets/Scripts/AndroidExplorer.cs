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

        Input.location.Start(2f, 2f);
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
            if (Input.touchCount == 1)
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
            else if (Input.touchCount == 2)
            {
                if(m_distance > 0.001f)
                {
                    float distance_temp;
                    distance_temp = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                    m_transform.localScale = m_transform.localScale * (distance_temp / m_distance);
                }
                m_distance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
            }
            if (Input.touchCount != 2) m_distance = 0f;
        }
        else m_released = true;


        //RotateX(Input.gyro.rotationRateUnbiased.x);
        RotateY(Input.gyro.rotationRateUnbiased.y);
        RotateZ(Input.gyro.rotationRateUnbiased.z);
        
        PrintInfo(m_distance.ToString());
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
    private float m_distance = 0f;
    private Renderer m_renderer;
    private Transform m_transform;

    #endregion
}