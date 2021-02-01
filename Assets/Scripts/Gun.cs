using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Gun : MonoBehaviour {
    public float damage = 10f;
    public float range = 100f;
    public float impactForce = 30f;
    public float fireRate = 20f;

    private float nextTimeToFire = 0f;
    public float ammo = 20;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public Rigidbody playerRigibody;
    public TMPro.TextMeshProUGUI text;
    public Transform player;

    private void Start() {
        text.text = ammo.ToString();
    }
    void Update() {
        if(Input.GetKey("r")) {
            ammo = 20;
            text.text = ammo.ToString();
        }
        if(Input.GetButton("Fire2") && Time.time >= nextTimeToFire) {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
            text.text = ammo.ToString();
        }
    }
    void Shoot() {
        if(ammo <= 0) { Debug.Log("RELOAD"); return; }
        ammo--;
        muzzleFlash.Play();
        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range)) {
            Target target = hit.transform.GetComponent<Target>();
            if(target != null) {
                target.TakeDamage(damage);
            }
            if(hit.rigidbody != null) {
                hit.rigidbody.AddForce(-hit.point * impactForce);
            }
            //playerRigibody.AddForce(hit.normal * impactForce + -player.forward);
            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);
        }
    }
}
