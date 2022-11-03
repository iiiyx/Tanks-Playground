﻿using UnityEngine;

public class TankControl : UnitControl
{
    public Transform m_TurretTransform;

    protected bool m_TurretIsLocked = true;
    protected bool m_TurretIsReset = true;
    protected Quaternion m_InitialTurretLocalRotation;
    protected Quaternion m_LookAtRotation;

    protected override void Awake()
    {
        base.Awake();
        m_InitialTurretLocalRotation = m_TurretTransform.localRotation;
    }

    protected override void SetLookRotation(Vector3 targetPosition)
    {
        m_TurretIsLocked = false;
        m_TurretIsReset = false;
        m_LookAtRotation = Quaternion.LookRotation(targetPosition - m_TurretTransform.position);
    }

    protected override void UpdateLookRotation()
    {
        if (!m_TurretIsLocked && m_TurretTransform.rotation != m_LookAtRotation)
        {
            m_TurretTransform.rotation = Quaternion.RotateTowards(m_TurretTransform.rotation, m_LookAtRotation, m_TurnSpeed * 50 * Time.deltaTime);
        }

        if (m_TurretIsLocked && !m_TurretIsReset)
        {
            ResetTurretRotation();
        }
    }

    protected override bool IsGoodLookRotationForFire()
    {
        return Mathf.Abs(Quaternion.Angle(m_TurretTransform.rotation, m_LookAtRotation)) < 1f;
    }

    protected override void ResetLookRotation()
    {
        m_TurretIsLocked = true;
        m_TurretIsReset = false;
        ResetTurretRotation();
    }

    protected void ResetTurretRotation()
    {
        if (m_TurretTransform.localRotation != m_InitialTurretLocalRotation)
        {
            m_TurretTransform.localRotation = Quaternion.RotateTowards(m_TurretTransform.localRotation, m_InitialTurretLocalRotation, m_TurnSpeed * 50 * Time.deltaTime);
        }
        else
        {
            m_TurretIsReset = true;
        }
    }

    public void Move()
    {
        transform.position = Vector3.Lerp(transform.position, transform.position + transform.forward, m_MoveSpeed * Time.deltaTime);
        //m_Rigidbody.AddForce(transform.forward * m_MoveSpeed);
        //m_Rigidbody.velocity = transform.forward * m_MoveSpeed;
    }

    public void Stop()
    {
        transform.position = Vector3.Lerp(transform.position, transform.position - transform.forward, m_MoveSpeed * Time.deltaTime);
    }

    public void TurnChasisRight()
    {
        //Rotate the sprite about the Y axis in the positive direction
        transform.Rotate(Vector3.up * Time.deltaTime * m_TurnSpeed, Space.World);
    }

    public void TurnChasisLeft()
    {
        //Rotate the sprite about the Y axis in the positive direction
        transform.Rotate(Vector3.up * Time.deltaTime * -m_TurnSpeed, Space.World);
    }

    public void TurnTurretRight()
    {
        m_TurretTransform.Rotate(Vector3.up * Time.deltaTime * m_TurnSpeed, Space.World);
    }

    public void TurnTurretLeft()
    {
        m_TurretTransform.Rotate(Vector3.up * Time.deltaTime * -m_TurnSpeed, Space.World);
    }
}
