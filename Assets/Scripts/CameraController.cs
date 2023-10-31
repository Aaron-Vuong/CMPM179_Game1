using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;
    public GameObject character;

    // Update is called once per frame
    void Update()
    {
        transform.position = character.transform.position + new Vector3(0, 5, -10);
    }
}
