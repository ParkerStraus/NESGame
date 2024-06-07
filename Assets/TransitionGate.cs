using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TransitionGate : MonoBehaviour
{
    public UnityEvent OnPass;
    public bool Ready;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Trigger the OnPass event
        if (OnPass != null && other.gameObject.tag == "Player" && Ready)
        {
            Ready = false;
            OnPass.Invoke();
        }
    }

    public void Reset()
    {
        Ready = true;
    }
}

