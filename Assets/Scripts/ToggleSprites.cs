using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSprites : MonoBehaviour
{
    public Sprite image1;
    public Sprite image2;

    public float toggleInterval = 1f;

    private Image childImage;

    // Start is called before the first frame update
    void Start()
    {
        childImage = GetComponentInChildren<Image>();
        StartCoroutine(ToggleImage());

    }   
    private IEnumerator ToggleImage()
    {
        while (true)
        {
            childImage.sprite = image1;
            yield return new WaitForSeconds(toggleInterval);
            childImage.sprite = image2;
            yield return new WaitForSeconds(toggleInterval);
        }
    }

}
