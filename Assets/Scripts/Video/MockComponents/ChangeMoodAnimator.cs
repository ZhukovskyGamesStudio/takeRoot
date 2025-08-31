using UnityEngine;

public class ChangeMoodAnimator : MonoBehaviour {
    [SerializeField]
    private SpriteRenderer _faceRend;

    [SerializeField]
    private AYellowpaper.SerializedCollections.SerializedDictionary<Mood, Sprite> _moodSprites;
    
    private Mood _currentMood;

    public void SetMood(Mood mood) {
        if (_currentMood == mood) return;
        
        _currentMood = mood;
        _faceRend.sprite = _moodSprites[_currentMood];
    }
}