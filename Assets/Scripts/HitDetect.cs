using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HitDetectType
{
    GivesDamage,
    TakesDamage,
    AlertsEnemy,
    ForgetEnemy,
    SetCheckpoint,
    Explode

}
public class HitDetect : MonoBehaviour
{
    public HitDetectType thisHitDetectType;
    public LayerMask layerToDetect;
    public int damage;
    public bool isActive = true;
    public Vector3 force;
    public Character parent;
    public HitDetect overlapper;
    public bool alwaysDoDamage = false;
    private bool overlapping;

    private void Start()
    {
        switch (thisHitDetectType)
        {

            case HitDetectType.GivesDamage:

            //fallthrough
            case HitDetectType.TakesDamage:


            //fallthrough
            case HitDetectType.AlertsEnemy:


            //fallthrough


            case HitDetectType.ForgetEnemy:
                //fallthrough

                if (parent == null)
                {
                    parent = GetComponentInParent<Character>();
                }
                break;


            case HitDetectType.SetCheckpoint:
                break;



            default:
                break;


        }

    }
    public void AlwaysDoDamageToParent(int spDamage, Vector3 forcehit)
    {
        parent.TakeDamage(spDamage, forcehit);

    }
    public void DoDamageToParent(int spDamage)
    {
        if (isActive)
        {
            parent.TakeDamage(spDamage);
        }
    }

    public void DoDamageToParent(Vector3 forcehit)
    {
        if (isActive)
        {
            parent.TakeDamage(damage, forcehit);
        }
    }

    public void DoDamageToParent(int spDamage, Vector3 forcehit)
    {
        if (isActive)
        {
            parent.TakeDamage(spDamage, forcehit);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (layerToDetect.value == (layerToDetect.value | (1 << other.gameObject.layer)))
        {

            Vector3 forcehit = new Vector3();
            if (other.transform.position.x > transform.position.x)
            {
                forcehit.x = force.x;
                forcehit.y = force.y;
                forcehit.z = force.z;

            }
            else
            {

                forcehit.x = -force.x;
                forcehit.y = force.y;
                forcehit.z = force.z;
            }
            //Debug.Log("Switch type = " + thisHitDetectType + "parent is " + parent.name);
            switch (thisHitDetectType)
            {

                case HitDetectType.GivesDamage:
                    overlapper = other.gameObject.GetComponent<HitDetect>();
                    if (alwaysDoDamage)
                    {

                        overlapper.AlwaysDoDamageToParent(damage, forcehit);

                    }
                    else
                    {
                        if (isActive)
                        {
                            overlapper.DoDamageToParent(damage, forcehit);
                        }
                    }
                    overlapping = true;
                    Debug.Log("hit " + damage + " " + other.gameObject.GetComponent<Character>().Health);

                    break;

                case HitDetectType.TakesDamage:

                    break;
                case HitDetectType.AlertsEnemy:
                    parent.Alert();


                    break;

                case HitDetectType.ForgetEnemy:
                    break;


                case HitDetectType.SetCheckpoint:

                    other.gameObject.GetComponent<Player>().SetNewCheckpoint(transform.position);
                    break;



                default:
                    break;


            }


        }

    }

    private void Update()
    {
        if (overlapping)
        {


            Vector3 forcehit = new Vector3();
            if (overlapper.transform.position.x > transform.position.x)
            {
                forcehit.x = force.x;
                forcehit.y = force.y;
                forcehit.z = force.z;

            }
            else
            {

                forcehit.x = -force.x;
                forcehit.y = force.y;
                forcehit.z = force.z;
            }
            switch (thisHitDetectType)
            {

                case HitDetectType.GivesDamage:
                    if (isActive)
                    {
                        overlapper.gameObject.GetComponent<HitDetect>().DoDamageToParent(damage, forcehit);
                    }
                    break;

                case HitDetectType.TakesDamage:

                    break;
                case HitDetectType.AlertsEnemy:
                    parent.Alert();

                    break;

                case HitDetectType.ForgetEnemy:
                    parent.Forget();
                    break;

                case HitDetectType.SetCheckpoint:
                    break;



                default:
                    break;

            }

        }


    }

    private void OnTriggerExit(Collider other)
    {

        if (layerToDetect.value == (layerToDetect.value | (1 << other.gameObject.layer)))
        {

            switch (thisHitDetectType)
            {

                case HitDetectType.GivesDamage:
                    overlapping = false;
                    overlapper = null;
                    break;

                case HitDetectType.TakesDamage:

                    break;
                case HitDetectType.AlertsEnemy:
                    parent.Alert();

                    break;

                case HitDetectType.ForgetEnemy:
                    parent.Forget();
                    break;

                case HitDetectType.SetCheckpoint:
                    break;

                default:
                    break;


            }
        }


    }
}
