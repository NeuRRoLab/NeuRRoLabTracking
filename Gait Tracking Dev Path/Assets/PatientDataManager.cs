using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PatientDataManager : MonoBehaviour
{
    public string filePath = "";
    public string fileName = "";
    public string patientCode = "DefaultPatientCode";
    public string behavior = "Baseline";
    public string leg = "Right";
    public int day = 1;
    public int trialNum = 1;
    public float totalTime = 60;
    float distalP = 0.12f;
    float medialP = 0.11f;
    float posteriorP = 0.21f;
    int anklePathLength = 240;

    private bool patientDataSet = false;

    private static PatientDataManager instance;

    void Awake()
    {
        // Singleton pattern to prevent duplicates
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        // Register scene reload detection
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Unregister to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name);

        // Scene has been reloaded, you can reset or reapply any scene-specific values here
        ApplyPatientDataToScene();
    }

    private void ApplyPatientDataToScene()
    {
        // Put logic here to apply your saved values to scene-specific components
        Debug.Log("Reapplying patient data...");
        // Example:
        // FindObjectOfType<SomeUIController>().UpdateDisplay(patientCode, behavior, leg, day);

        if (patientCode == "DefaultPatientCode") return;

        Joints joints = GameObject.Find("RigidBodyStruct").GetComponent<Joints>();

        GameObject.Find("PatientCodeInput").GetComponent<InputField>().text = patientCode;
        joints.SetPatientCode();

        if (trialNum != 1)
            joints.updateTrialNumber(trialNum);

        GameObject.Find("DayInput").GetComponent<InputField>().text = day.ToString();
        joints.SetDay();

        Dropdown dropdown = GameObject.Find("LegInput").GetComponent<Dropdown>();
        if (leg == "Left")
            dropdown.value = 1;
        joints.leg = leg;

        // Change behavior dropdown
        dropdown = GameObject.Find("BehaviorInput").GetComponent<Dropdown>();
        int behaviorIndex = 0;
        for (behaviorIndex = 0; behaviorIndex < dropdown.options.Count; behaviorIndex++)
        {
            if (behavior == dropdown.options[behaviorIndex].text)
                dropdown.value = behaviorIndex;
        }
        joints.behavior = behavior;

        GameObject.Find("TimeTotalInput").GetComponent<InputField>().text = totalTime.ToString();
        joints.SetTotalTime();

        joints.capacity = anklePathLength;
        GameObject.Find("TrailInput").GetComponent<InputField>().text = anklePathLength.ToString();
        joints.ChangeTrailLength();
    }

    // Optional: method to set patient data
    public void SetPatientData(string code, string behav, string limb, int sessionDay, int trial, float _totalTime, float distal, float medial, float posterior, int _anklePathLength)
    {
        patientCode = code;
        behavior = behav;
        leg = limb;
        day = sessionDay;
        patientDataSet = true;
        trialNum = trial;
        totalTime = _totalTime;
        distalP = distal;
        medialP = medial;
        posteriorP = posterior;
        anklePathLength = _anklePathLength;
    }
}
