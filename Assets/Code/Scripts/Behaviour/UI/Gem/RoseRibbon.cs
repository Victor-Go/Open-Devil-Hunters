using System;
using System.Collections;
using System.Collections.Generic;
using Code.Scripts.Src.Types;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Scripts.Behaviour.UI
{
  public class RoseRibbon : MonoBehaviour
  {
    public float Delay = 0.25f;
    public float Amplitude;
    public float K;
    public float Rotation;
    public float RotationSpeed;
    public float AngleOffset;
    public float Speed;
    public bool UseZigzag;

    private readonly Func<float, float> r = theta => Mathf.Sin(theta);
    private readonly Func<float, float> zigZagR = theta => 2 * Mathf.Asin(Mathf.Sin(theta)) / Mathf.PI;

    private RectTransform rectTransform;
    private TrailRenderer trailRenderer;
    private float currentAngle;

    private bool inPlace;
    private float currentRotation;

    private float startTimer;

    private void Awake()
    {
      rectTransform = GetComponent<RectTransform>();
      trailRenderer = GetComponent<TrailRenderer>();
    }

    private void Start()
    {
      transform.position = Vector2.zero;
      currentAngle = AngleOffset;
    }

    private void Polar(float angle)
    {
      var theta = angle * Mathf.Deg2Rad;

      if (rectTransform)
      {
        var a = Amplitude * 50;
        Vector3 newPosition;
        if (UseZigzag)
        {
          newPosition = Quaternion.AngleAxis(Rotation + currentRotation + currentAngle, Vector3.forward) *
                        Vector2.right * (a * zigZagR(K * theta));
        }
        else
        {
          newPosition = Quaternion.AngleAxis(Rotation + currentRotation + currentAngle, Vector3.forward) *
                        Vector2.right * (a * r(K * theta));
        }
        rectTransform.localPosition = new Vector3(newPosition.x, newPosition.y, 0);
      }
      else
      {
        if (UseZigzag)
          transform.position = Quaternion.AngleAxis(Rotation + currentRotation + currentAngle, Vector3.forward) * Vector2.right * (Amplitude * zigZagR(K * theta));
        else
          transform.position = Quaternion.AngleAxis(Rotation + currentRotation + currentAngle, Vector3.forward) * Vector2.right * (Amplitude * r(K * theta));
      }

      if (inPlace) return;
      inPlace = true;
      trailRenderer.Clear();
    }

    void Update()
    {
      startTimer += Time.deltaTime;
      
      if (startTimer < Delay) return;
      
      Polar(currentAngle);

      currentAngle += Time.deltaTime * Speed;
      currentRotation += Time.deltaTime * RotationSpeed;
    }
  }
}