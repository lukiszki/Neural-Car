using System;
using UnityEngine;
using Random = System.Random;

public class CarController : MonoBehaviour
{
    Rigidbody rigidbody;

    static Random rnd = new System.Random();

    private Vector3 deltaV;

    private float rotation;

    [SerializeField]
    private float viewAngle = 0.3f;
    [SerializeField]
    private float rayLeng = 5f;

    [SerializeField]
    private float time = 5f;

    public LayerMask layerMask;

    // private Transform transform;

    [SerializeField]
    float constantSpeed = 1f;

    [Range(0f, 1f)]
    [SerializeField]
    private float forwardDist;

    [Range(0f,1f)]
    [SerializeField]
    private float forLeftDist;
    [Range(0f, 1f)]
    [SerializeField]
    private float forRightDist;

    [Range(0f, 1f)]
    [SerializeField]
    private float leftDist;
    [Range(0f, 1f)]
    [SerializeField]
    private float rightDist;


    

    [SerializeField]
    private float rotSpeed = 1f;

    private Network brain = new Network();

    [SerializeField]
    private bool isAI = true;

    [Header("FITNESS")]
    [SerializeField]
    public float fitness = 0f;


    public GameObject copy;

    public bool GOOD { get; private set; }

    private void Awake()
    {
        brain.LoadValues(ref rnd);
        //brain.InitValues();
    }
    void Start()
    {
        
        // transform = this.transform;
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = time;

        ShootRays();
        transform.position += (transform.forward * constantSpeed)*Time.deltaTime;
        if (isAI)
            transform.Rotate(new Vector3(0, brain.GetValue(forwardDist, forLeftDist, forRightDist,leftDist,rightDist)*Time.deltaTime)*rotSpeed);
        else
            transform.Rotate(new Vector3(0, Input.GetAxis("Horizontal")*Time.deltaTime)*rotSpeed);
        



    }
    private void FixedUpdate()
    {
        SetFitness();
        

    }

    private void SetFitness()
    {
        fitness += (forwardDist + forLeftDist + forRightDist + rightDist + leftDist) / 50;
        if (forwardDist < 0.1f || forLeftDist < 0.15f || forRightDist < 0.15f || rightDist < 1.1f || leftDist < 1.1f)
        {
            fitness *= 0.9f;
        }
    }

    private void ShootRays()
    {

        //forward line
        RaycastHit fHit;
        Ray fRay = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(fRay, out fHit, rayLeng, layerMask)) 
        {
            //Debug.Log(hit.distance);
            Debug.DrawRay(fRay.origin, transform.forward * fHit.distance);
            forwardDist = Normalize(fHit.distance, rayLeng);
        }
        else forwardDist = 1;

        //forward left line
        RaycastHit flHit;
        Ray flRay = new Ray(transform.position, -transform.right + transform.forward);
        if (Physics.Raycast(flRay, out flHit, rayLeng, layerMask))
        {
            //Debug.Log(hit.distance);
            Debug.DrawRay(flRay.origin, Vector3.Normalize(-transform.right + transform.forward) * flHit.distance);
            forLeftDist = Normalize(flHit.distance, rayLeng);
        }
        else forLeftDist = 1;




        //forward right line
        RaycastHit frHit;
        Ray frRay = new Ray(transform.position, transform.right + transform.forward);
        if (Physics.Raycast(frRay, out frHit, rayLeng, layerMask))
        {
            //Debug.Log(hit.distance);
            Debug.DrawRay(frRay.origin, Vector3.Normalize(transform.right + transform.forward) * frHit.distance);
            forRightDist = Normalize(frHit.distance, rayLeng);
        }
        else forRightDist = 1;







        //left line
        RaycastHit lHit;
        Ray lRay = new Ray(transform.position, -transform.right + transform.forward);
        if (Physics.Raycast(lRay, out lHit, rayLeng, layerMask))
        {
            //Debug.Log(hit.distance);
            Debug.DrawRay(lRay.origin, Vector3.Normalize(-transform.right) * lHit.distance);
            leftDist = Normalize(lHit.distance, rayLeng);
        }
        else leftDist = 1;




        //right line
        RaycastHit rHit;
        Ray rRay = new Ray(transform.position, transform.right + transform.forward);
        if (Physics.Raycast(rRay, out rHit, rayLeng, layerMask))
        {
            //Debug.Log(hit.distance);
            Debug.DrawRay(rRay.origin, Vector3.Normalize(transform.right) * rHit.distance);
            rightDist = Normalize(rHit.distance, rayLeng);
        }
        else rightDist = 1;



    }

    private float Normalize(float x, float maxValue)
    {
        return x / maxValue;
    }
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.name == "v1" || collision.gameObject.name == "v2" || collision.gameObject.name == "v3"||(collision.gameObject.name == "KILL"&&!GOOD))
        {
            Dead();
        }
        if (collision.gameObject.name == "mult3")
        {
            fitness *= 2;
            GOOD = true;
        }
    }

    private void Dead()
    {
        //brain.Save();
        
        Destroy(gameObject);
    }
    public void Save()
    {
        brain.Save();
    }
    public Values GetValues()
    {
         return brain.GetValues();
    }
}
