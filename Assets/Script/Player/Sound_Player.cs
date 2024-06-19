using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Player : MonoBehaviour
{
    private AudioSource m_AudioSource;
    public AudioClip Sound_Jump;
    public AudioClip Sound_FootStep;
    public AudioClip Sound_Shoot;
    public AudioClip Sound_AbilitySelect;
    public AudioClip Sound_Hit;
    public AudioClip Sound_Die;


    // Start is called before the first frame update
    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FootStep()
    {
        AudioSource.PlayClipAtPoint(Sound_FootStep, transform.position);
    }

    public void Jump()
    {
        m_AudioSource.PlayOneShot(Sound_Jump);
    }

    public void Shoot()
    {
        m_AudioSource.PlayOneShot(Sound_Shoot);
    }

    public void AbilitySelect()
    {
        m_AudioSource.PlayOneShot(Sound_AbilitySelect);
    }

    public void Hit()
    {
        m_AudioSource.PlayOneShot(Sound_Hit);
    }

    public void Die()
    {
        AudioSource.PlayClipAtPoint(Sound_Die, transform.position);
    }
}
