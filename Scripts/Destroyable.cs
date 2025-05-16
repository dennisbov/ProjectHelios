using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Destroyable : MonoBehaviour
{
    [SerializeField] private GameObject _destroyParticles;
    [SerializeField] private float _timeToDestroy;

    [NonSerialized] public UnityEvent OnDestroy = new UnityEvent();

    private AudioSource _audioSource;
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    public void Destroy()
    {
        StartCoroutine(DestroyByTime());    
    }

    private IEnumerator DestroyByTime()
    {
        OnDestroy.Invoke();
        yield return new WaitForSeconds(_timeToDestroy);
        _audioSource.Play();
        _destroyParticles.transform.parent = null;
        _destroyParticles.transform.position = transform.position;
        _destroyParticles.transform.localScale = Vector3.one;
        _destroyParticles.SetActive(true);
        Destroy(gameObject);
    }

}
