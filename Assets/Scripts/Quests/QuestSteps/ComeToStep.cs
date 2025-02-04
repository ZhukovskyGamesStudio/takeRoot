using UnityEngine;

public class ComeToStep : QuestStep
{
    public Vector3 position;

    private void Start()
    {
        transform.position = position;
        UpdateQuestStepStatus(_status + $"{position.x} : {position.y}");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<SettlerData>(out var settler))
        {
            if (settler.Race == Race || Race == Race.Both)
            {
                FinishStep();
            }
        }
    }
}