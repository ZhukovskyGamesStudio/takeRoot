using UnityEngine;

public class StraightProjectile : Projectile
{

    protected override bool TargetReached()
    {
        return target.SpriteRenderer.bounds.Contains(transform.position);
    }

    protected override void OnTargetReached()
    {
        Debug.Log("OnTargetReached");
        //TODO: Take damage
        Destroy(gameObject);
    }
    
    protected override void MakeMove()
    {
        var targetPos = new Vector3(target.transform.position.x, target.transform.position.y + 1, target.transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, target.SpriteRenderer.bounds.center, speed * Time.deltaTime);
    }
}