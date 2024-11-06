using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioSource;     // Reference to the AudioSource component
    private bool isPlaying = false;
    private Turntable turntable;

    // Reference to the animation clip
    public AnimationClip thunderboltClip;
    public Material postProcessMaterial;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        turntable = Camera.main.GetComponent<Turntable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isPlaying)
        {
            turntable.enabled = false;
            animator.Play("Thunderbolt", 0, 0f);
            audioSource.Play();
            StartCoroutine(CameraShake(1.6f));
            StartCoroutine(ResetAnimatorState(thunderboltClip.length));
            isPlaying = true;
        }
    }

    private IEnumerator CameraShake(float delay)
    {
        yield return new WaitForSeconds(delay);
        postProcessMaterial.SetFloat("_CameraShakeMagnitude", Random.Range(0.01f, 0.05f));

    }
    private IEnumerator ResetAnimatorState(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.Play("Idle");
        isPlaying = false;
        turntable.enabled = true;
        postProcessMaterial.SetFloat("_CameraShakeMagnitude", 0f);
    }
}