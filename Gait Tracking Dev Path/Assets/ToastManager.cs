using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public delegate void ToastFunction();

public class Toast
{
    public string toastText;
    public bool buttonsPresent;
    public ToastFunction yesFunction = null;
    public ToastFunction noFunction = null;

    public Toast(string text)
    {
        toastText = text;
        buttonsPresent = false;
    }

    public Toast(string text, ToastFunction yes, ToastFunction no)
    {
        toastText = text;
        buttonsPresent = true;
        yesFunction = yes;
        noFunction = no;
    }
}

public class ToastManager : MonoBehaviour
{
    public Text toastText;
    public GameObject yesButton;
    public GameObject noButton;
    public GameObject okButton;
    public float lerpTime = 0.5f;

    private Vector3 restPosition = new Vector3(0,2087,0);
    private Vector3 viewPosition = new Vector3(0, 0, 0);
    //ToastFunction yesFunction;
    //ToastFunction noFunction;

    private Toast currentToast = null;

    private Queue<Toast> toastQueue = new Queue<Toast>();

    private void Start()
    {
        restPosition = transform.position;
        viewPosition = new Vector3(restPosition.x, Screen.height / 2, restPosition.z);
        Debug.Log(restPosition);
    }

    private void Update()
    {
        if (currentToast == null && toastQueue.Count > 0)
        {
            currentToast = toastQueue.Dequeue();
            toastText.text = currentToast.toastText;
            if (currentToast.buttonsPresent)
            {
                yesButton.SetActive(true);
                noButton.SetActive(true);
                okButton.SetActive(false);
            }
            else
            {
                yesButton.SetActive(false);
                noButton.SetActive(false);
                okButton.SetActive(true);
            }

            StartCoroutine(ShowToast());
        }
    }

    IEnumerator ShowToast()
    {
        restPosition = new Vector3(Screen.width * 0.75f, Screen.height * 6, restPosition.z);
        viewPosition = new Vector3(Screen.width * 0.75f, Screen.height / 2, restPosition.z);
        float startTime = Time.time;
        transform.position = restPosition;

        while (Time.time - startTime < lerpTime)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(restPosition.y, viewPosition.y, (Time.time - startTime) / lerpTime), transform.position.y);
            yield return null;
        }

        transform.position = viewPosition;
    }

    IEnumerator RemoveToast()
    {
        float startTime = Time.time;
        transform.position = viewPosition;

        while (Time.time - startTime < lerpTime)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(viewPosition.y, restPosition.y, (Time.time - startTime) / lerpTime), transform.position.y);
            yield return null;
        }

        transform.position = restPosition;

        currentToast = null;
    }

    IEnumerator ShowTimedToast()
    {
        StartCoroutine(ShowToast());

        yield return new WaitForSeconds(5f);

        StartCoroutine(RemoveToast());
    }

    public void RequestToast(Toast toast)
    {
        toastQueue.Enqueue(toast);
    }

    public void PressYes()
    {
        if (currentToast.yesFunction != null)
        {
            ButtonHandler(currentToast.yesFunction);
        }

        StartCoroutine(RemoveToast());
    }

    public void PressNo()
    {
        if (currentToast.noFunction != null)
        {
            ButtonHandler(currentToast.noFunction);
        }

        StartCoroutine(RemoveToast());
    }

    public void PressOK()
    {
        StartCoroutine(RemoveToast());
    }

    void ButtonHandler(ToastFunction function)
    {
        function();
    }
}
