﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotController
{
    public struct MyQuat
    {
        public float w;
        public float x;
        public float y;
        public float z;
    }
    public struct MyVec
    {
        public float x;
        public float y;
        public float z;
    }

    public class MyRobotController
    {
        #region public methods
        public string Hi()
        {
            string s = "Arnau Pradas Soriano & Emma Polanco Garcia";
            return s;
        }

        #region Fields
        // First, we need arrays to store initial and final joint angles
        private float[] initialAngles;
        // We need an array to store rotation axes for joints, that instead of being a float will need to be MyVec
        private MyVec[] axisRotation;
        // Variables for controlling iterations, that will simply add as flags
        private bool iteration2ControlFlag;
        private bool iteration3ControlFlag;
        #endregion

        #region Constructor
        public MyRobotController()
        {
            // Initialize initialAngles
            initialAngles = new float[5] { 74, -10, 80, 40, 0 };

            // Initialize axisRotation
            axisRotation = new MyVec[5];
            axisRotation[0] = new MyVec { x = 0, y = 1, z = 0 };
            axisRotation[1] = new MyVec { x = 1, y = 0, z = 0 };
            axisRotation[2] = axisRotation[3] = axisRotation[1];
            axisRotation[4] = new MyVec { x = 0, y = 1, z = 0 };

            // Initialize control flags
            iteration2ControlFlag = true;
            iteration3ControlFlag = true;
        }
        #endregion



        // EX1: this function will place the robot in the initial position
        #region EX1
        public void PutRobotStraight(out MyQuat rot0, out MyQuat rot1, out MyQuat rot2, out MyQuat rot3) {

            // Initialize rot0 with the identity quaternion
            rot0 = NullQ;

            // Calculate the rotations for each joint using the initial angles and rotation axes
            rot0 = Rotate(rot0, axisRotation[0], (float)Radians(initialAngles[0]));
            rot1 = Rotate(rot0, axisRotation[1], (float)Radians(initialAngles[1]));
            rot2 = Rotate(rot1, axisRotation[2], (float)Radians(initialAngles[2]));
            rot3 = Rotate(rot2, axisRotation[3], (float)Radians(initialAngles[3]));

            // Reset control flags for iterations 2 and 3
            iteration2ControlFlag = true;
            iteration3ControlFlag = true;
        }
        #endregion


        //EX2: this function will interpolate the rotations necessary to move the arm of the robot until its end effector collides with the target (called Stud_target)
        //it will return true until it has reached its destination. The main project is set up in such a way that when the function returns false, the object will be droped and fall following gravity.
        #region EX2
        public bool PickStudAnim(out MyQuat rot0, out MyQuat rot1, out MyQuat rot2, out MyQuat rot3)
        {

            bool myCondition = false;
            //todo: add a check for your condition



            if (myCondition)
            {
                //todo: add your code here
                rot0 = NullQ;
                rot1 = NullQ;
                rot2 = NullQ;
                rot3 = NullQ;


                return true;
            }

            //todo: remove this once your code works.
            rot0 = NullQ;
            rot1 = NullQ;
            rot2 = NullQ;
            rot3 = NullQ;

            return false;
        }
        #endregion

        //EX3: this function will calculate the rotations necessary to move the arm of the robot until its end effector collides with the target (called Stud_target)
        //it will return true until it has reached its destination. The main project is set up in such a way that when the function returns false, the object will be droped and fall following gravity.
        //the only difference wtih exercise 2 is that rot3 has a swing and a twist, where the swing will apply to joint3 and the twist to joint4
        #region EX3
        public bool PickStudAnimVertical(out MyQuat rot0, out MyQuat rot1, out MyQuat rot2, out MyQuat rot3)
        {

            bool myCondition = false;
            //todo: add a check for your condition



            while (myCondition)
            {
                //todo: add your code here


            }

            //todo: remove this once your code works.
            rot0 = NullQ;
            rot1 = NullQ;
            rot2 = NullQ;
            rot3 = NullQ;

            return false;
        }


        public static MyQuat GetSwing(MyQuat rot3)
        {
            //todo: change the return value for exercise 3
            return NullQ;

        }


        public static MyQuat GetTwist(MyQuat rot3)
        {
            //todo: change the return value for exercise 3
            return NullQ;

        }
        #endregion

        #endregion

        #region private and internal methods

        internal int TimeSinceMidnight { get { return (DateTime.Now.Hour * 3600000) + (DateTime.Now.Minute * 60000) + (DateTime.Now.Second * 1000) + DateTime.Now.Millisecond; } }


        private static MyQuat NullQ
        {
            get
            {
                MyQuat a;
                a.w = 1;
                a.x = 0;
                a.y = 0;
                a.z = 0;
                return a;
            }
        }

        internal MyQuat Multiply(MyQuat q1, MyQuat q2) {

            //todo: change this so it returns a multiplication:

            //1  First, we create a new quaternion to store our result later
            MyQuat result = NullQ;
            //2  Calculate the scalar w component
            result.w = q1.w * q2.w - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z;
            //3  And then, calculate the x, y and z components
            result.x = q1.w * q2.x + q1.x * q2.w + q1.y * q2.z - q1.z * q2.y;
            result.y = q1.w * q2.y - q1.x * q2.z + q1.y * q2.w + q1.z * q2.x;
            result.z = q1.w * q2.z + q1.x * q2.y - q1.y * q2.x + q1.z * q2.w;

            //Finally simply return the result
            return result;
        }

        internal MyQuat Rotate(MyQuat currentRotation, MyVec axis, float angle)
        {
            //SHOULD BE CHANGED BECAUSE IT IS NOT NORMALIZED IN ANY MOMENT(?????????)


            //todo: change this so it takes currentRotation, and calculate a new quaternion rotated by an angle "angle" radians along the normalized axis "axis"

            //1  Calculate the half-angle and sine of the half-angle
            float halfAngle = angle / 2.0f;
            float sinHalfAngle = (float)Math.Sin(halfAngle);

            //2  Create a new quaternion for rotation
            MyQuat rotation = NullQ;
            rotation.w = (float)Math.Cos(halfAngle);
            rotation.x = axis.x * sinHalfAngle;
            rotation.y = axis.y * sinHalfAngle;
            rotation.z = axis.z * sinHalfAngle;

            //3  Multiply the current rotation by the new rotation quaternion
            return Multiply(currentRotation, rotation);
        }
        //todo: add here all the functions needed

        internal double Radians(double angleRadians)
        {
            return (angleRadians * (Math.PI / 180));
        }
        #endregion
    }
}
