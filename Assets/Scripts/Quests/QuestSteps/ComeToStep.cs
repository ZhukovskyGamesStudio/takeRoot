using UnityEngine;

public class ComeToStep : QuestStep
{
    public Vector3 position;

    private void Start()
    {
        transform.position = position;
        UpdateQuestStepData(new QuestStepStatus(QuestStepState.Active, Race, $"Зайди на точку {position.x} {position.y}"));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<SettlerData>(out var settler))
        {
            if (settler.Race == Race.Plants)
            {
                FinishStep();
            }
        }
    }
}