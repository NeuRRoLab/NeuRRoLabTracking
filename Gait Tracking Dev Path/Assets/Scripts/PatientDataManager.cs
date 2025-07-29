using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// This file is here almost entirely for the functionality of the reset calibration button, since 
// recalibration was not intended as a feature of the original program. It is triggered by Joints.cs
// when the reset button is pressed, and is otherwise not a consistent tracker of any of the data
// it keeps between scenes. That data is all in Joints, largely with similar or the same names.
// This file exists as a band-aid solution to resetting calibration by just reloading the whole scene.

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
    string distalP = "12";
    string medialP = "11";
    string posteriorP = "21";
    string anklePathLength = "240";

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
        // Scene has been reloaded, reset or reapply any scene-specific values here
        ApplyPatientDataToScene();
    }

    private void ApplyPatientDataToScene()
    {
        Joints joints = GameObject.Find("RigidBodyStruct").GetComponent<Joints>();

        GameObject.Find("TimeTotalInput").GetComponent<InputField>().text = totalTime.ToString();
        joints.SetTotalTime();

        GameObject.Find("TrailInput").GetComponent<InputField>().text = anklePathLength;

        GameObject.Find("Distal%Input").GetComponent<InputField>().text = distalP;
        GameObject.Find("Medial%Input").GetComponent<InputField>().text = medialP;
        GameObject.Find("Posterior%Input").GetComponent<InputField>().text = posteriorP;

        if (patientCode == "DefaultPatientCode") return;

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
    }

    // Called by Joints.cs when the reset button is pressed
    public void SetPatientData(string code, string behav, string limb, int sessionDay, int trial, float _totalTime, int _anklePathLength)
    {
        patientCode = code;
        behavior = behav;
        leg = limb;
        day = sessionDay;
        patientDataSet = true;
        trialNum = trial;
        totalTime = _totalTime;
        distalP = GameObject.Find("Distal%Input").GetComponent<InputField>().text;
        medialP = GameObject.Find("Medial%Input").GetComponent<InputField>().text;
        posteriorP = GameObject.Find("Posterior%Input").GetComponent<InputField>().text;
        anklePathLength = GameObject.Find("TrailInput").GetComponent<InputField>().text;
    }
}
