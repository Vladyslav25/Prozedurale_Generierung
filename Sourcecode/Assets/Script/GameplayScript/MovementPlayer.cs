using UnityEngine;

public class MovementPlayer : MonoBehaviour
{
    [SerializeField]
    private float m_playerSpeed = 5f;
    [SerializeField]
    private float m_sprintSpeed = 10f;

    void FixedUpdate()
    {
        ProcessManualMovement(); // Movment
    }

    //If Player run in a zombie
    private void OnCollisionEnter(Collision collision)
    {
        //while he have the sword
        if (collision.gameObject.tag == "NPC" && tag == "Evil")
        {
            //say npc to die
            collision.gameObject.GetComponent<NPC>().Die();
            //Add a Point
            GameEventManager.current.GetPoint();
        }
        //while he have NO sword
        if (collision.gameObject.tag == "NPC" && tag == "Player")
        {
            //Player die
            Die();
        }
    }

    private void Die()
    {
        MySceneManager.ChangeScene("Lose");
    }

    private void ProcessManualMovement()
    {
        Vector3 move = Vector3.zero;
        move += new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        move = move.normalized;

        float Speed = m_playerSpeed;
        //If player is sprinting, overwride the Speed
        if (Input.GetButton("Sprint"))
        {
            Speed = m_sprintSpeed;
        }

        transform.Translate(move * Speed * Time.fixedDeltaTime);
    }
}
