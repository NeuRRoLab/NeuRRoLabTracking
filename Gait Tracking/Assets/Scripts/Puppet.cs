﻿using UnityEngine;
using System.Collections;

public class Puppet : MonoBehaviour {

    GameObject pelvisModel;
    GameObject thighModel;
    GameObject shankModel;
    GameObject footModel;

    GameObject hipJointCenter;
    GameObject thighHipJointCenter;
    GameObject kneeJointCenter;
    GameObject shankKneeJointCenter;
    GameObject ankleJointCenter;
    GameObject footAnkleJointCenter;

    GameObject[] joints;

    Joints jointController;

    public enum side : int {Right = 1, None=0, Left = -1};
    private int sideIs;
    bool tPosed;

    // Use this for initialization
    void Start ()
    {
        sideIs = (int)side.None;
        tPosed = false;
        pelvisModel = GameObject.Find("hip");
        jointController = GameObject.Find("RigidBodyStruct").GetComponent<Joints>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(tPosed)
        {
            shankKneeJointCenter.transform.position = joints[(int)Joints.jointEnum.kneeJoint].transform.position;
            shankKneeJointCenter.transform.rotation = joints[(int)Joints.jointEnum.kneeJointPartner].transform.rotation;

            footAnkleJointCenter.transform.position = joints[(int)Joints.jointEnum.ankleJoint].transform.position;
            footAnkleJointCenter.transform.rotation = joints[(int)Joints.jointEnum.ankleJointPartner].transform.rotation;
        }
        else if(sideIs!=(int)side.None)
        {
            constructPuppet();
            tPosed = true;
        }
	}

    private void constructPuppet()
    {
        if (sideIs == (int)side.Right)
        {
            thighModel = GameObject.Find("rThigh");
            shankModel = GameObject.Find("rShin");
            footModel = GameObject.Find("rFoot");
            hipJointCenter = pelvisModel.transform.FindChild("RightJointCenter").gameObject;
            thighHipJointCenter = thighModel.transform.FindChild("RightHipCenter").gameObject;
            kneeJointCenter = thighModel.transform.FindChild("RightKneeCenter").gameObject;
            shankKneeJointCenter = shankModel.transform.FindChild("RightKneeCenter").gameObject;
            ankleJointCenter = shankModel.transform.FindChild("RightAnkleCenter").gameObject;
            footAnkleJointCenter = footModel.transform.FindChild("RightAnkleCenter").gameObject;
        }
        else
        {
            thighModel = GameObject.Find("lThigh");
            shankModel = GameObject.Find("lShin");
            footModel = GameObject.Find("lFoot");
            hipJointCenter = pelvisModel.transform.FindChild("LeftJointCenter").gameObject;
            thighHipJointCenter = thighModel.transform.FindChild("LeftHipCenter").gameObject;
            kneeJointCenter = thighModel.transform.FindChild("LeftKneeCenter").gameObject;
            shankKneeJointCenter = shankModel.transform.FindChild("LeftKneeCenter").gameObject;
            ankleJointCenter = shankModel.transform.FindChild("LeftAnkleCenter").gameObject;
            footAnkleJointCenter = footModel.transform.FindChild("LeftAnkleCenter").gameObject;
        }

        hipJointCenter.transform.parent = null;
        pelvisModel.transform.parent = hipJointCenter.transform;

        thighHipJointCenter.transform.parent = null;
        thighModel.transform.parent = null;
        thighHipJointCenter.transform.rotation = Quaternion.LookRotation(Vector3.Cross(thighHipJointCenter.transform.right, Vector3.Normalize(thighHipJointCenter.transform.position - kneeJointCenter.transform.position)), 
            Vector3.Normalize(thighHipJointCenter.transform.position - kneeJointCenter.transform.position));
        thighModel.transform.transform.parent = thighHipJointCenter.transform;

        shankKneeJointCenter.transform.parent = null;
        shankModel.transform.parent = null;
        shankKneeJointCenter.transform.rotation = Quaternion.LookRotation(Vector3.Cross(kneeJointCenter.transform.right, Vector3.Normalize(kneeJointCenter.transform.transform.position - ankleJointCenter.transform.position)), Vector3.Normalize(kneeJointCenter.transform.transform.position - ankleJointCenter.transform.position));
        shankModel.transform.parent = shankKneeJointCenter.transform;

        footAnkleJointCenter.transform.parent = null;
        footModel.transform.parent = null;
        footAnkleJointCenter.transform.rotation = Quaternion.LookRotation(Vector3.Normalize(Vector3.Cross(Vector3.Normalize(kneeJointCenter.transform.transform.position - ankleJointCenter.transform.position), ankleJointCenter.transform.right)), Vector3.Normalize(kneeJointCenter.transform.transform.position - ankleJointCenter.transform.position));
        footModel.transform.parent = footAnkleJointCenter.transform;

        joints = jointController.getJoints();

        hipJointCenter.transform.position = joints[(int)Joints.jointEnum.hipJoint].transform.position;
        hipJointCenter.transform.rotation = joints[(int)Joints.jointEnum.hipJoint].transform.rotation;
        hipJointCenter.transform.parent = joints[(int)Joints.jointEnum.hipJoint].transform;

        thighHipJointCenter.transform.position = joints[(int)Joints.jointEnum.hipJoint].transform.position;
        thighHipJointCenter.transform.rotation = joints[(int)Joints.jointEnum.kneeJoint].transform.rotation;
        thighHipJointCenter.transform.parent = joints[(int)Joints.jointEnum.kneeJoint].transform;

        shankKneeJointCenter.transform.position = joints[(int)Joints.jointEnum.kneeJoint].transform.position;
        shankKneeJointCenter.transform.rotation = joints[(int)Joints.jointEnum.kneeJointPartner].transform.rotation;
        shankKneeJointCenter.transform.parent = joints[(int)Joints.jointEnum.kneeJointPartner].transform;

        footAnkleJointCenter.transform.position = joints[(int)Joints.jointEnum.ankleJoint].transform.position;
        footAnkleJointCenter.transform.rotation = joints[(int)Joints.jointEnum.ankleJointPartner].transform.rotation;
        footAnkleJointCenter.transform.parent = joints[(int)Joints.jointEnum.ankleJointPartner].transform;

        float currentDistance = Vector3.Distance(hipJointCenter.transform.position, kneeJointCenter.transform.position);
        float targetDistance = Vector3.Distance(hipJointCenter.transform.position, joints[(int)Joints.jointEnum.kneeJoint].transform.position);
        //thighModel.transform.localScale = thighModel.transform.localScale * Vector3.Distance(hipJointCenter.transform.position, kneeJointCenter.transform.position) - Vector3.Distance(hipJointCenter.transform.position, kneeJointCenter.transform.position)
    }
    private void updateJoints()
    {

    }

    public void setSide(int side)
    {
        sideIs = side;
    }
}
