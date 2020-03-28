using UnityEngine;

public class PersonMovement : MonoBehaviour
{
    [Header("General")]
    [SerializeField]
    private float m_movemntSpeed = 1.0f;
    [SerializeField]
    private bool m_manualControl = false;
    


    [Header("Rendering")]
    [SerializeField]
    private Material m_ill;
    [SerializeField]
    private Material m_inspector;

    [Header("Interaction")]
    [SerializeField]
    private Transform m_community;
    [SerializeField]
    private bool m_comMovement;


    private RandVectUtil rv;
    private Rigidbody rb;
    private bool m_rmoving;
    private float m_timer, m_tLimit;
    private Vector3 dir, newPos;
    private MeshRenderer m_mr;
    private Collider m_collider;
    private int m_gTimer;
    


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rv = new RandVectUtil();
        m_mr = GetComponent<MeshRenderer>();
        m_collider = GetComponent<Collider>();

        m_rmoving = false;
        
        //m_comMovement = false;
    }

    // Update is called once per frame
    void Update()
    {
        if( m_gTimer == 100)
        {
            TravelProbability();
            m_gTimer = 0;
        }
        m_gTimer++;

        if(m_manualControl)
        {
            ControlMovement();
        }else
        {
            if(m_comMovement) MoveBetweenCommunity();
            else 
                MoveRandomly();
        }
        
    }

    private void MoveRandomly()
    {
        if(!m_rmoving)
        {
            m_tLimit = Random.Range(1.0f,5.0f);
            m_movemntSpeed = Random.Range(.5f,2.5f);
            dir = rv.GetRandVect2D();

            m_rmoving = true;
            m_timer = 0.0f;
        }else
        {
            m_timer += Time.deltaTime;
            newPos = transform.position + dir*Time.deltaTime*m_movemntSpeed;

           if( m_timer < m_tLimit)
           {
               rb.MovePosition(newPos);
           }else{
               m_rmoving = false;
           }
            
        }
    }


    /// <summary>
    /// Control player movement manually
    /// </summary>
    private void ControlMovement()
    {
        if(Input.GetKey(KeyCode.W))
        {
            gameObject.transform.position = gameObject.transform.position + Vector3.up * Time.deltaTime * m_movemntSpeed;
        }
        if(Input.GetKey(KeyCode.S))
        {
            gameObject.transform.position = gameObject.transform.position - Vector3.up * Time.deltaTime * m_movemntSpeed;
        }
        if(Input.GetKey(KeyCode.D))
        {
            gameObject.transform.position = gameObject.transform.position + Vector3.right * Time.deltaTime * m_movemntSpeed;
        }
        if(Input.GetKey(KeyCode.A))
        {
            gameObject.transform.position = gameObject.transform.position - Vector3.right * Time.deltaTime * m_movemntSpeed;
        }
    }


    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "ill")
        {
            gameObject.tag = "ill";
            m_mr.material = m_ill;
        }
        else if(other.gameObject.tag == "community")
        {
            if(m_comMovement)
            {
                m_collider.enabled = false;
            }
        }
    }
    

    private void MoveBetweenCommunity()
    {
        dir = m_community.position - gameObject.transform.position;
        dir.Normalize();
        rb.MovePosition(gameObject.transform.position + dir*Time.deltaTime);
        if( Vector3.Distance(gameObject.transform.position, m_community.position) <.1f )
        {
            print("Reached");
            m_comMovement =false;
            m_collider.enabled = true;
        }
    }

    private void TravelProbability()
    {
        int prob = Random.Range(0,100);

        if(prob > 10 && prob <12)
        {
            m_comMovement = true;
        }
    }
}
