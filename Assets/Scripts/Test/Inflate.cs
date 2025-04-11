using System;
using UnityEngine;

public class PipeInflate : MonoBehaviour
{
    [Range(-1f, 1f)] public float inflateAmount = 0f;
    public Vector2 inflateCenter = new Vector2(0.5f, 0.5f); // Центр (0-1 в UV-координатах)
    public float inflateSpeed = 0.5f;
    
    private Material material;

    void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
    }

    void Update()
    {
        // Пример: надувание/сдувание по клавише E
        if (Input.GetKey(KeyCode.E))
            inflateAmount = Mathf.Min(inflateAmount + inflateSpeed * Time.deltaTime, 1f);
        else
            inflateAmount = Mathf.Max(inflateAmount - inflateSpeed * Time.deltaTime, 0f);

        material.SetFloat("_InflateAmount", inflateAmount);
        material.SetVector("_InflateCenter", new Vector4(inflateCenter.x, inflateCenter.y, 0, 0));
    }

    private void OnValidate()
    {
        material = GetComponent<SpriteRenderer>().sharedMaterial;
        material.SetFloat("_InflateAmount", inflateAmount);
        material.SetVector("_InflateCenter", new Vector4(inflateCenter.x, inflateCenter.y, 0, 0));
    }
    
}