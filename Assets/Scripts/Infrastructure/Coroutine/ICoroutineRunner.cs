using System.Collections;
using UnityEngine;

public interface ICoroutineRunner : IService {
    public Coroutine StartCoroutine(IEnumerator routine);
}