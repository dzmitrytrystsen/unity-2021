using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDetector : MonoBehaviour
{
    private Touch _rotateTouch;
    private Vector2 _touchStartPosition, _touchEndPosition;
    private float _touchStartTime;
    private float _touchEndTime;
    private Camera mainCamera;
    private enum TouchType { RotateTouch, Default };
    private TouchType _touchType;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        int i = 0;

        while ( i < Input.touchCount)
        {
            Touch touch = Input.GetTouch(i);

            if (touch.phase == TouchPhase.Began)
            {
                _touchStartPosition = touch.position;
                _touchStartTime = Time.time;
            }
            else if (touch.phase == TouchPhase.Moved && GetTypeOfTouch(touch) == TouchType.RotateTouch)
            {
                gameObject.GetComponent<Player>().Turn(touch.deltaPosition.x, touch.deltaPosition.y);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                _touchEndPosition = touch.position;
                _touchEndTime = Time.time;
            }

            ++i;
        }
    }

    private TouchType GetTypeOfTouch(Touch touch)
    {
        if (touch.position.x > Screen.width / 2)
            return TouchType.RotateTouch;
        else
            return TouchType.Default;
    }

    private bool IfSwipeWasVertical(float x, float y)
    {
        return Mathf.Abs(y) > Mathf.Abs(x);
    }

    private bool IfSwipeWasFastEnough(float startTime, float endTime)
    {
        float touchSpeedNeeded = 0.5f;

        return endTime - startTime <= touchSpeedNeeded;
    }

    private bool IfSwipeWasLongEnough(Vector3 startPosition, Vector3 endPosition)
    {
        float touchLengthNeeded = 0.2f;

        return Vector3.Distance(mainCamera.ScreenToViewportPoint(startPosition),
            mainCamera.ScreenToViewportPoint(endPosition)) >= touchLengthNeeded;
    }
}
