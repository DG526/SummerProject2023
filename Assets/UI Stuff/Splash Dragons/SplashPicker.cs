using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashPicker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().SetInteger("Dragon", UnityEngine.Random.Range(1, 6));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        GetComponent<Animator>().SetInteger("Dragon", UnityEngine.Random.Range(1, 6));
    }
}
