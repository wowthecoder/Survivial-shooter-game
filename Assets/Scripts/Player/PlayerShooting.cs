using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    public static int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;
    //public Slider enemyHealth;
    public Image CritImage;
    public Text CritText;
    public static bool IsShot;

    float timer;
    Ray shootRay = new Ray();
    RaycastHit shootHit;
    int shootableMask;
    ParticleSystem gunParticles;
    LineRenderer gunLine;
    AudioSource gunAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f;


    void Awake ()
    {
        shootableMask = LayerMask.GetMask ("Shootable");
        gunParticles = GetComponent<ParticleSystem> ();
        gunLine = GetComponent<LineRenderer> ();
        gunAudio = GetComponent<AudioSource> ();
        gunLight = GetComponent<Light> ();
        CritImage.enabled = false;
        CritText.text = "";
    }


    void Update ()
    {
        timer += Time.deltaTime;

		if(Input.GetButton ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            Shoot ();
        }

        if(timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects ();
        }
    }

    public void DisableEffects ()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }


    void Shoot ()
    {
        timer = 0f;

        gunAudio.Play ();

        gunLight.enabled = true;

        gunParticles.Stop ();
        gunParticles.Play ();

        gunLine.enabled = true;
        gunLine.SetPosition (0, transform.position);

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
        {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();
            if(enemyHealth != null)
            {
                IsShot = true;
                int CritChance = Random.Range(1, 100);
                if (CritChance <= 30)
                {
                    damagePerShot = Random.Range(30, 70);
                    CritImage.enabled = true;
                    CritText.text = damagePerShot.ToString();
                    StartCoroutine(Wait());
                }
                else
                {
                    damagePerShot = 20;
                    CritImage.enabled = false;
                }
                enemyHealth.TakeDamage (damagePerShot, shootHit.point);
            }
            gunLine.SetPosition (1, shootHit.point);
        }
        else
        {
            gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
            IsShot = false;
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        CritImage.enabled = false;
        CritText.text = "";
    }
}
