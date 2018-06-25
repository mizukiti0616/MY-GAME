using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクトを左右に動かし、プレイヤーが接触した時にはそのオブジェクトを子オブジェクトに設定する。
/// プレイヤーが上に乗った時にプレイヤーも一緒に動いて、オブジェクトから落ちないようにする機能を提供する。
/// </summary>
[RequireComponent(typeof(Collider))]
public class MovingPlatformController2 : MonoBehaviour
{
    /// <summary>振り幅</summary>
    public float m_amplitude = 1f;

    public float m_speed = 2.0f;
    public bool m_moveLeftFirst = false;
    private float m_timer;
    private Vector3 m_initialPosition;

    private void Start()
    {
        m_initialPosition = transform.position;
    }

    void FixedUpdate()
    {
        // オブジェクトを左右に揺らす
        m_timer += Time.deltaTime * m_speed;
        float posXSin = Mathf.Sin(m_timer) * m_amplitude;
        Vector3 pos = m_initialPosition;
        if (m_moveLeftFirst)
            pos -= new Vector3(0,posXSin,  0);
        else
            pos += new Vector3(0,posXSin,  0);
        transform.position = pos;
    }

    // プレイヤーが上に乗った時、それを子オブジェクトとすることによりプレイヤーはオブジェクトと一緒に動く
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Player")
            collision.collider.gameObject.transform.SetParent(transform);
    }

    // プレイヤーがオブジェクトから離れた時は、親子関係を解除する
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Player")
            collision.collider.gameObject.transform.SetParent(null);
    }
}