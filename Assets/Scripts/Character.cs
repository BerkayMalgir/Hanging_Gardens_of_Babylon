using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]   
public enum SIDE{Left,Mid,Right}

public class Character : MonoBehaviour
{
    public SIDE m_Side = SIDE.Mid;

    private float NewXPos = 0f;
    public bool SwipeLeft;
    public bool SwipeRight;
    public float XValue;
    private CharacterController m_char;
    
    // Start is called before the first frame update
    void Start()
    {
        m_char = GetComponent<CharacterController>();
        transform.position=Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        SwipeLeft = Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);
        SwipeRight = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);

        if (SwipeLeft)
        {
            if (m_Side == SIDE.Mid)
            {
                NewXPos = -XValue;
                m_Side = SIDE.Left;
            }
            else if (m_Side == SIDE.Right)
            {
                NewXPos = 0;
                m_Side = SIDE.Mid;
            }
            
        }
        if (SwipeRight)
        {
            if (m_Side == SIDE.Mid)
            {
                NewXPos = XValue;
                m_Side = SIDE.Right;
            }
            else if (m_Side == SIDE.Left)
            {
                NewXPos = 0;
                m_Side = SIDE.Mid;
            }
            
        }

        m_char.Move((NewXPos - transform.position.x) * Vector3.right);
    }
}
