using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private AudioClip[] _mouseClips;
    [SerializeField] private AudioSource _mouseSource;

    private void Update()
    {
        InputCheck();
    }

    private void InputCheck()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            _mouseSource.clip = _mouseClips[0];
            _mouseSource.Play();
        }
        else if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            _mouseSource.clip = _mouseClips[1];
            _mouseSource.Play();
        }
    }
}
