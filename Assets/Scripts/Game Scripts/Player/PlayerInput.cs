using UnityEngine;
using System;
using System.Collections;
using Gamekit3D;


public class PlayerInput : SingletonBehaviour<PlayerInput>
{
    public static PlayerInput Instance
    {
        get { return s_Instance; }
    }

    protected static PlayerInput s_Instance;

    [HideInInspector]
    public bool playerControllerInputBlocked;

    protected Vector2 m_Movement;
    protected Vector2 m_Camera;
    protected bool m_Frisk;
    protected bool m_Pull;
    protected bool m_Pause;
    protected bool m_Reset;
    protected bool m_ExternalInputBlocked;

    public Vector2 MoveInput
    {
        get
        {
            if(playerControllerInputBlocked || m_ExternalInputBlocked)
                return Vector2.zero;
            return m_Movement;
        }
    }

    public Vector2 CameraInput
    {
        get
        {
            if(playerControllerInputBlocked || m_ExternalInputBlocked)
                return Vector2.zero;
            return m_Camera;
        }
    }

    public bool Frisk
    {
        get { return m_Frisk && !playerControllerInputBlocked && !m_ExternalInputBlocked; }
    }

    public bool Pull
    {
        get { return m_Pull && !playerControllerInputBlocked && !m_ExternalInputBlocked; }
    }

    public bool Pause
    {
        get { return m_Pause; }
    }

    WaitForSeconds m_PullInputWait;
    Coroutine m_PullWaitCoroutine;

    public const float k_PullInputDuration = 3f;

    void Awake()
    {
        m_PullInputWait = new WaitForSeconds(k_PullInputDuration);

        if (s_Instance == null)
            s_Instance = this;
        else if (s_Instance != this)
            throw new UnityException("There cannot be more than one PlayerInput script.  The instances are " + s_Instance.name + " and " + name + ".");
    }


    void Update()
    {
        m_Movement.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        m_Camera.Set(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        m_Frisk = Input.GetButtonDown("Frisk");

        m_Pull = Input.GetButtonDown("Fire1");
        if (Input.GetButtonDown("Fire1"))
        {
            /*if (m_PullWaitCoroutine != null)
                StopCoroutine(m_PullWaitCoroutine);

            m_PullWaitCoroutine = StartCoroutine(PullWait());*/
        }

        m_Pause = Input.GetButtonDown ("Pause");
    }

    /*IEnumerator PullWait()
    {
        m_Pull = true;

        yield return m_PullInputWait;

        m_Pull = false;
    }*/

    public bool HaveControl()
    {
        return !m_ExternalInputBlocked;
    }

    public void ReleaseControl()
    {
        m_ExternalInputBlocked = true;
    }

    public void GainControl()
    {
        m_ExternalInputBlocked = false;
    }
}
