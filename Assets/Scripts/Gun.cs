using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    [Range(0.1f,1.5f)]
    private float fireRate = 1f;
    [SerializeField]
    [Range(1, 10)]
    private int damage = 1;

    [SerializeField] private float timer;
    [SerializeField] private ParticleSystem muzzleParticle;
    [SerializeField] private AudioSource fireAudioSource;

    void Start()
    {
        
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > fireRate)
        {
            if (Input.GetButton("Fire1"))
            {
                timer = 0;
                FireGun();
            }
        }
    }

    private void FireGun()
    {
        muzzleParticle.Play();
        fireAudioSource.Play();

        

        Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
        RaycastHit hitInfo;
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2f);

        if (Physics.Raycast(ray,out hitInfo,100f))
        {
            //Destroy(hitInfo.collider.gameObject);
            var health = hitInfo.collider.GetComponent<Health>();
            if (health)
            {
                health.TakeDamage(damage);
            }
            
        }
    }
}
