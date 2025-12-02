using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingList : MonoBehaviour
{
    [Header("Sliding options")]
    [SerializeField] List<RectTransform> options = new List<RectTransform>();
    Dictionary<RectTransform, int> optionIndex = new Dictionary<RectTransform, int>();

    [Header("Detecting touch")]
    [SerializeField] BoxCollider2D leftTouchDetector;
    [SerializeField] BoxCollider2D rightTouchDetector;

    [Header("Options animations")]
    [SerializeField] float animationTime = 0.5f;
    [SerializeField] AnimationCurve scaleCurve;
    [SerializeField] AnimationCurve positionXCurve;
    [SerializeField] AnimationCurve positionYCurve;

    int workingAnimationCoroutines = 0;

    int centerOptionIndex;

    void Start()
    {
        for (int i = 0; i < options.Count; i++)
		{
            optionIndex.Add(options[i], i);
		}
    }

    void Update()
    {
        if (workingAnimationCoroutines > 0)
            return;

        // Detectar toques táctiles (móvil)
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.touches[i].phase != TouchPhase.Began)
                continue;

            Vector3 touchScreenPos = Input.touches[i].position;
            touchScreenPos.z = -Camera.main.transform.position.z; // Asegurar Z correcta para 2D
            Vector2 touchWorldPos = Camera.main.ScreenToWorldPoint(touchScreenPos);

            if (leftTouchDetector.OverlapPoint(touchWorldPos) && centerOptionIndex > 0)
            {
                SlideRight();
                return;
            }
            else if (rightTouchDetector.OverlapPoint(touchWorldPos) && centerOptionIndex < options.Count - 1)
            {
                SlideLeft();
                return;
            }
        }

        // Detectar clics del mouse (para testing en editor)
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseScreenPos = Input.mousePosition;
            mouseScreenPos.z = -Camera.main.transform.position.z; // Asegurar Z correcta para 2D
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            
            if (leftTouchDetector.OverlapPoint(mousePosition) && centerOptionIndex > 0)
            {
                SlideRight();
                return;
            }
            else if (rightTouchDetector.OverlapPoint(mousePosition) && centerOptionIndex < options.Count - 1)
            {
                SlideLeft();
                return;
            }
        }

        // Navegación con teclado (alternativa)
        int horizontalInput = InputManager.GetHorizontalNavigation();
        if (horizontalInput < 0 && centerOptionIndex > 0)
        {
            SlideRight();
        }
        else if (horizontalInput > 0 && centerOptionIndex < options.Count - 1)
        {
            SlideLeft();
        }
    }

    public void SlideLeft()
	{
        centerOptionIndex++;
        foreach (var option in options)
		{
            int index = optionIndex[option]--; // save old index and decrease
            int newIndex = index - 1;
            StartCoroutine(AnimateSlide(option, index, newIndex));
        }
	}

    public void SlideRight()
	{
        centerOptionIndex--;
        foreach (var option in options)
        {
            int index = optionIndex[option]++; // save old index and increase
            int newIndex = index + 1;
            StartCoroutine(AnimateSlide(option, index, newIndex));
        }
    }

    IEnumerator AnimateSlide(RectTransform option, int oldIndex, int newIndex)
    {
        workingAnimationCoroutines++;

        float newScale, newX, newY;

        float elapsedTime = 0f;
        while (elapsedTime <= animationTime)
		{
            elapsedTime += Time.deltaTime;

            float currentIndex = Mathf.Lerp(oldIndex, newIndex, elapsedTime / animationTime);

            newScale = scaleCurve.Evaluate(currentIndex);
            newX = positionXCurve.Evaluate(currentIndex);
            newY = positionYCurve.Evaluate(currentIndex);

            option.localScale = new Vector3(newScale, newScale, 1f);
            option.localPosition = new Vector3(newX, newY);

            yield return null;
		}

        workingAnimationCoroutines--;
    }
}
