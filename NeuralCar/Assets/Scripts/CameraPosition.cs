using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class CameraPosition : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Transform player;
    [SerializeField]
    Vector3 pos;

    [Range(0.01f, 1)]
    [SerializeField]
    float cameraSpeed = 0.1f;

    void Update()
    {
        //this.transform.position = player.position + pos;
        //this.transform.LookAt(player);

    }
}
