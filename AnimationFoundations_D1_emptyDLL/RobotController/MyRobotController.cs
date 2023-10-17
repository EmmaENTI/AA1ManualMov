using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        #region FieldsEX1
        // First, we need arrays to store initial and final joint angles
        private float[] jointStartingAngles;
        // We need an array to store rotation axes for joints, that instead of being a float will need to be MyVec
        private MyVec[] axisRotation;
        // Variables for controlling iterations, that will simply add as flags
        private bool iteration2ControlFlag;
        private bool iteration3ControlFlag;
        private void InitializeFieldsEX1()
        {
            // Initialize joint starting angles
            jointStartingAngles = new float[5] { 74, -10, 80, 40, 0 };

            // Initialize axis rotation
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
        #region FieldsEX2
        // Track the progress of an animation
        private float animationProgress;
        // Store the target joint angles for the animation in array.
        private float[] targetJointAngles;
        private void InitializeFieldsEX2()
        {
            // Initialize the animation progress to 0 (beginning of animation).
            animationProgress = 0;
            // Initialize the target joint angles with specific values.
            targetJointAngles = new float[5] { 40, -10, 90, 20, 90 };
        }
        #endregion

        //Fields EX3
        private static MyQuat twist, swing;

        #region ConstructorOfMyRobotController
        public MyRobotController()
        {
            //1
            InitializeFieldsEX1();
            //2
            InitializeFieldsEX2();
            //3
        }
        #endregion



        // EX1: this function will place the robot in the initial position
        #region EX1
        public void PutRobotStraight(out MyQuat rot0, out MyQuat rot1, out MyQuat rot2, out MyQuat rot3) {

            // Initialize rot0 with the identity quaternion
            rot0 = NullQ;

            // Calculate the rotations for each joint using the initial angles and rotation axes
            rot0 = Rotate(rot0, axisRotation[0], (float)Radians(jointStartingAngles[0]));
            rot1 = Rotate(rot0, axisRotation[1], (float)Radians(jointStartingAngles[1]));
            rot2 = Rotate(rot1, axisRotation[2], (float)Radians(jointStartingAngles[2]));
            rot3 = Rotate(rot2, axisRotation[3], (float)Radians(jointStartingAngles[3]));

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
            // Reset control flags on the first iteration.
            iteration3ControlFlag = true;

            if (iteration2ControlFlag)
            {
                // Initialize animation progress.
                animationProgress = 0;
                iteration2ControlFlag = false;
            }

            if (animationProgress <= 1)
            {
                // Calculate the interpolated joint angles.
                float lerpProgress = Lerp(jointStartingAngles[0], targetJointAngles[0], animationProgress);
                rot0 = NullQ;
                rot0 = Rotate(rot0, axisRotation[0], (float)Radians(lerpProgress));

                lerpProgress = Lerp(jointStartingAngles[1], targetJointAngles[1], animationProgress);
                rot1 = Rotate(rot0, axisRotation[1], (float)Radians(lerpProgress));

                lerpProgress = Lerp(jointStartingAngles[2], targetJointAngles[2], animationProgress);
                rot2 = Rotate(rot1, axisRotation[2], (float)Radians(lerpProgress));

                lerpProgress = Lerp(jointStartingAngles[3], targetJointAngles[3], animationProgress);
                rot3 = Rotate(rot2, axisRotation[3], (float)Radians(lerpProgress));

                // Increment the animation progress.
                animationProgress += 0.0030f;
                return true;
            }
            else
            {
                // Reset rotation quaternions once the animation is complete.
                rot0 = NullQ;
                rot1 = NullQ;
                rot2 = NullQ;
                rot3 = NullQ;
                return false;
            }
        }
        #endregion

        //EX3: this function will calculate the rotations necessary to move the arm of the robot until its end effector collides with the target (called Stud_target)
        //it will return true until it has reached its destination. The main project is set up in such a way that when the function returns false, the object will be droped and fall following gravity.
        //the only difference wtih exercise 2 is that rot3 has a swing and a twist, where the swing will apply to joint3 and the twist to joint4
        #region EX3
        public bool PickStudAnimVertical(out MyQuat rot0, out MyQuat rot1, out MyQuat rot2, out MyQuat rot3)
        {

            // Set control flags for the vertical animation.
            iteration2ControlFlag = true;

            // Check if it's the first iteration of the animation.
            if (iteration3ControlFlag)
            {
                // Initialize the animation progress.
                animationProgress = 0;
                iteration3ControlFlag = false;
            }

            if (animationProgress <= 1)
            {
                // Initialize rotation quaternions for joints.
                rot0 = NullQ;

                // Interpolate and rotate the joints.
                rot0 = RotateJoint(rot0, 0, animationProgress);
                rot1 = RotateJoint(rot0, 1, animationProgress);
                rot2 = RotateJoint(rot1, 2, animationProgress);

                // Calculate the swing and twist rotations.
                MyQuat swing = RotateJoint(rot2, 3, animationProgress);
                MyQuat twist = RotateJoint(swing, 4, animationProgress);

                // Combine the swing and twist rotations to get rot3.
                rot3 = Multiply(twist, swing);

                // Increment the animation progress.
                animationProgress += 0.0030f;
                return true;
            }
            else
            {
                // Return false if the condition is not met or if the animation is complete.
                ResetRotationQuaternions(out rot0, out rot1, out rot2, out rot3);
                return false;
            }

        }


        public static MyQuat GetSwing(MyQuat rot3)
        {
            return Normalize(Multiply(Inverse(twist), rot3));
        }


        public static MyQuat GetTwist(MyQuat rot3)
        {
            return Normalize(Multiply(rot3, Inverse(swing)));
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

        internal static MyQuat Multiply(MyQuat q1, MyQuat q2) 
        {
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


        internal double Radians(double angleDegrees)
        {
            return angleDegrees * (Math.PI / 180);
        }
        internal float Lerp(float a, float b, float t)
        {
            return a + t * (b - a);
            // Returns a value that smoothly transitions from a to b based on the weight t.
        }

        internal static MyQuat Normalize(MyQuat _quat)
        {
            float magnitude = (float)Math.Sqrt(_quat.x * _quat.x + _quat.y * _quat.y + _quat.z * _quat.z + _quat.w * _quat.w);

            return new MyQuat
            {
                x = _quat.x / magnitude,
                y = _quat.y / magnitude,
                z = _quat.z / magnitude,
                w = _quat.w / magnitude
            };
        }

        internal static MyQuat Inverse(MyQuat _quat)
        {
            float num = 1.0f / (_quat.x * _quat.x + _quat.y * _quat.y + _quat.z * _quat.z + _quat.w * _quat.w);

            return new MyQuat
            {
                x = -_quat.x * num,
                y = -_quat.y * num,
                z = -_quat.z * num,
                w = _quat.w * num
            };
        }

        private MyQuat RotateJoint(MyQuat currentRotation, int jointIndex, float progress)
        {
            return Rotate(currentRotation, axisRotation[jointIndex], (float)Radians(Lerp(jointStartingAngles[jointIndex], targetJointAngles[jointIndex], progress)));
        }

        private void ResetRotationQuaternions(out MyQuat rot0, out MyQuat rot1, out MyQuat rot2, out MyQuat rot3)
        {
            rot0 = NullQ;
            rot1 = NullQ;
            rot2 = NullQ;
            rot3 = NullQ;
        }



        #endregion
    }
}
