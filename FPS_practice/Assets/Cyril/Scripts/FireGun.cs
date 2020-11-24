using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGun : MonoBehaviour
{
    private Animation anim;
    private AudioSource audioSource;
    [SerializeField]
    private Camera mainCam;
    [SerializeField]
    private GameObject firePoint;
    [SerializeField]
    private GameObject muzzleFlash;
    [SerializeField]
    private GameObject hitEffect;
    private int audioClipNum = 2;
    [SerializeField]
    private AudioClip[] audioClip;


    void Start()
    {
        anim = GetComponent<Animation>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        TryFire();
    }

    private void TryFire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
    }

    private void Fire()
    {
        if (!anim.isPlaying)
        {
            PlayEffects();
            FireRay();
            anim.Play();
        }
    }

    private void FireRay()
    {
        Ray bullet = new Ray(firePoint.transform.position, firePoint.transform.forward);
        bool isHit = Physics.Raycast(bullet, out RaycastHit hitInfo, 100f);
        Debug.DrawRay(firePoint.transform.position, firePoint.transform.forward);
        if (isHit)
        {
            GameObject fx = Instantiate(hitEffect);
            fx.transform.position = hitInfo.point;
        }
    }

    private void PlayEffects()
    {
        //총구화염
        GameObject mFlash = Instantiate(muzzleFlash);
        mFlash.transform.position = firePoint.transform.position;
        mFlash.transform.forward = firePoint.transform.forward;

        //사운드 출력
        audioSource.clip = audioClip[0];
        audioSource.Play();
    }
}
