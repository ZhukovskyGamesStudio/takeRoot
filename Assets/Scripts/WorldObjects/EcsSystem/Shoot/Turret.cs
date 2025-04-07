using System.Collections.Generic;
using UnityEngine;

public class Turret : Shooter
{
    [SerializeField] private int magazineSize;
    private Stack<Projectile> projectiles;
    public override void Init(ECSEntity entity)
    {
        base.Init(entity);
        projectiles = new Stack<Projectile>(magazineSize);
    }

    protected override void Reload()
    {
        GameObject projectile = CreateProjectile(false);
        projectiles.Push(projectile.GetComponent<Projectile>());
        if (projectiles.Count == magazineSize)
            CanShoot = true;
    }

    protected override void Shoot()
    {
        Vector3 target3 = new Vector3(currentTarget.transform.position.x, currentTarget.transform.position.y);
        Vector3 diff = target3 - transform.position;
        //transform.position = target3 * CELL_SIZE;
        if (diff.x < 0) {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
        }

        if (diff.x > 0) {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        
        Projectile proj = projectiles.Pop();
        proj.gameObject.SetActive(true);
    }

    protected override void FinishShoot()
    {
        if (projectiles.Count == 0)
            CanShoot = false;
    }
}