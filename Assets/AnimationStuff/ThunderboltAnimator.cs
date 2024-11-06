using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderboltAnimator : MonoBehaviour
{
    public GameObject thunderboltPlane;
    public GameObject thunderboltPlane2; // yes this is a lazy way to do this...
    private string animationStateName = "Thunderbolt";

    private float visibilityDelay = 1.6f;

    private Animator targetAnimator;

    private void Start()
    {
        // Get the Animator component on the same GameObject
        targetAnimator = GetComponent<Animator>();

        // Initially hide the thunderbolt plane
        thunderboltPlane.SetActive(false);
        thunderboltPlane2.SetActive(false);
    }

    private void Update()
    {
        thunderboltPlane.SetActive(false);
        thunderboltPlane2.SetActive(false);

        // Check if the target animation is playing and track its progress
        if (targetAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationStateName))
        {
            float animationTime = targetAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime * targetAnimator.GetCurrentAnimatorStateInfo(0).length;
            if (animationTime > visibilityDelay)
            {
                thunderboltPlane.SetActive(true);
                thunderboltPlane2.SetActive(true);
            }
        }

    }
}