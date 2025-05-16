using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(MeshRenderer))]
public class GlassWall : MonoBehaviour
{
    [SerializeField] private GameObject _particleSystem;
    private AudioSource _audioSource;
    private BoxCollider _boxCollider;
    private MeshRenderer _meshRenderer;
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _boxCollider = GetComponent<BoxCollider>();
    }
    public void Break()
    {
        _audioSource.Play();
        Vector3 particlePosition = _particleSystem.transform.position;
        _particleSystem.transform.parent = null;
        _particleSystem.transform.position = particlePosition;
        _particleSystem.transform.localScale = Vector3.one;
        _particleSystem.SetActive(true);

        _meshRenderer.enabled = false;
        _boxCollider.enabled = false;
    }
}
