using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 20f;
    //[SerializeField] float slowSpeed = 20f;
    //[SerializeField] float boostSpeed = 35f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float sideMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float frontMovement = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.Translate(sideMovement, frontMovement, 0);
    }
}
