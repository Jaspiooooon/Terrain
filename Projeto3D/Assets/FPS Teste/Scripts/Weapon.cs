using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bulletImpact;
    [SerializeField] float fireRate, spread, range;
    [SerializeField] int bulletsForShoot, timeBetweenShoots;

    float timeToShoot;

    public void Fire(bool crouching)
    {
        StartCoroutine(FireCoroutine(crouching));
    }

    private IEnumerator FireCoroutine(bool crouching)
    {
        if (Time.time > timeToShoot)
        {

            timeToShoot = Time.time + 1 / fireRate;

            for (int i = 0; i < bulletsForShoot; i++)
            {
                Shoot(crouching);
                yield return new WaitForSeconds(timeBetweenShoots);
            }

           
        }
    }

    private void Shoot(bool crouching)
    {
        RaycastHit hit;

        float newSpread = crouching ? spread/2 : spread;

        Vector3 direction = new Vector3(Random.Range(-newSpread, newSpread),
            Random.Range(-newSpread, newSpread), 0) + transform.forward;

        if (Physics.Raycast(firePoint.position, direction, out hit, range))
        {
            Collider obj = hit.transform.GetComponent<Collider>();
            if(obj != null)
            {
                Debug.Log(obj.gameObject.name);
                Instantiate(bulletImpact, hit.point, Quaternion.LookRotation(hit.normal));
            }  
        }

        Debug.DrawLine(firePoint.position, firePoint.position + direction * range);
    }

}
