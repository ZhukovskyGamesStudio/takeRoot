using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionPlayer : MonoBehaviour {
	
	public SpriteRenderer Bubble;
	public SpriteRenderer Icon;
	
	public List<Sprite> Bubbles;
	public List<Sprite> Icons;

	[Min(0.0f)]
	public float playTime;
	private Coroutine _emotionCoroutine;
	private Transform _parentTransform;
	private void Start() {
		_parentTransform = transform.parent;
	}

	public void PlayEmotion(BubbleType bubble, EmotionType emotion) {
		Icon.gameObject.transform.localScale = _parentTransform.localScale;
		SetBubble(bubble);
		SetIcon(emotion);
		if (_emotionCoroutine != null) {
			Bubble.gameObject.SetActive(false);
			StopCoroutine(_emotionCoroutine);
			_emotionCoroutine = null;
		}
		_emotionCoroutine = StartCoroutine(PlayForSeconds(playTime));
	}
	private void SetBubble(BubbleType bubble) {
		switch (bubble) {
			case BubbleType.Exclamation:
				Bubble.sprite = Bubbles[0];
				break;
			case BubbleType.Talk:
				Bubble.sprite = Bubbles[1];
				break;
			case BubbleType.Think:
				Bubble.sprite = Bubbles[2];
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(bubble), bubble, null);
		}
	}
	private void SetIcon(EmotionType emotion) {
		switch (emotion) {
			case EmotionType.Sad:
				Icon.sprite = Icons[0];
				break;
			case EmotionType.Like:
				Icon.sprite = Icons[1];
				break;
			case EmotionType.Attention:
				Icon.sprite = Icons[2];
				break;
			case EmotionType.Sleep:
				Icon.sprite = Icons[3];
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(emotion), emotion, null);
		}
	}

	private IEnumerator PlayForSeconds(float seconds) {
		Bubble.gameObject.SetActive(true);
		yield return new WaitForSeconds(seconds);
		Bubble.gameObject.SetActive(false);
	}
}

public enum BubbleType {
	Exclamation,
	Talk,
	Think
}

public enum EmotionType {
	Sad,
	Like,
	Attention,
	Sleep
}