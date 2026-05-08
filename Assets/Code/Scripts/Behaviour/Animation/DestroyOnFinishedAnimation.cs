using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnFinishedAnimation : MonoBehaviour
{
    public Animator Animator;

    private Animator animator;

    protected void Awake()
    {
        if (Animator != null)
        {
            animator = Animator;
        }
    }

    public void OnAnimationFinished()
    {
        Destroy(gameObject);
    }
}
