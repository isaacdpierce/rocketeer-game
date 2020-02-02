﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
  [SerializeField] float rcsThrust = 100f;
  [SerializeField] float mainThrust = 100f;
  Rigidbody rigidBody;
  AudioSource audioSource;
  // Start is called before the first frame update
  void Start()
  {
    rigidBody = GetComponent<Rigidbody>();
    audioSource = GetComponent<AudioSource>();

  }

  // Update is called once per frame
  void Update()
  {
    Thrust();
    Rotate();
  }

  void OnCollisionEnter(Collision collision)
  {
    switch (collision.gameObject.tag)
    {
      case "Friendly":
        print("You're OK!"); // TODO remove
        break;
      case "Power":
        print("Power Up!"); // TODO remove
        break;
      default:
        print("DEAD!"); // TODO remove
        break;
    }

    print("BOOM!!!");
  }
  private void Thrust()
  {
    if (Input.GetKey(KeyCode.Space))
    {
      rigidBody.AddRelativeForce(Vector3.up * mainThrust);
      if (!audioSource.isPlaying)
      {
        audioSource.Play();
      }
    }
    else
    {
      audioSource.Stop();
    }
  }

  private void Rotate()
  {
    rigidBody.freezeRotation = true;
    float rotationThisFrame = rcsThrust * Time.deltaTime;

    if (Input.GetKey(KeyCode.A))
    {
      transform.Rotate(Vector3.forward * rotationThisFrame);
    }
    else if (Input.GetKey(KeyCode.D))
    {
      transform.Rotate(-Vector3.forward * rotationThisFrame);
    }
    rigidBody.freezeRotation = false;
  }

}