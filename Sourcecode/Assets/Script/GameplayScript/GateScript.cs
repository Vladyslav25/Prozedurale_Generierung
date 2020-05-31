using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : MonoBehaviour
{
    GameObject m_player;
    Vector3 m_inside;
    Vector3 m_outside;
    // Start is called before the first frame update
    void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_inside = transform.right * 2f + transform.position + Vector3.up * 2f;
        m_outside = -transform.right * 2f + transform.position + Vector3.up * 2f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision == null)
        {
            return;
        }
        if (collision.gameObject.tag == m_player.tag)
        {
            if (Vector3.Distance(m_player.transform.position, m_inside) <
                Vector3.Distance(m_player.transform.position, m_outside))
            {
                m_player.transform.position = m_outside;
            }
            else
            {
                m_player.transform.position = m_inside;
            }
        }
    }
}
