namespace VRTK
{
    using UnityEngine;

    public class Switch : VRTK_Control
    {
        public enum LeverDirection
        {
            x, y, z
        }

        public LeverDirection direction = LeverDirection.y;
        public float min = 0f;
        public float max = 100f;
        public float stepSize = 1f;

        private Rigidbody rb;
        private VRTK_InteractableObject io;
        private HingeJoint hj;

        protected override void InitRequiredComponents()
        {
            InitRigidBody();
            InitInteractable();
            InitJoint();
        }

        protected override bool DetectSetup()
        {
            return true;
        }

        protected override void HandleUpdate()
        {
            value = CalculateValue();
            if (!io.IsGrabbed())
            {
                SnapToValue(value);
            } else
            {
                DisableSnapping();
            }
        }

        private void InitRigidBody()
        {
            rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody>();
            }
            rb.isKinematic = false;
            rb.useGravity = false;
            rb.angularDrag = 30; // otherwise lever will continue to move too far on its own
        }

        private void InitInteractable()
        {
            io = GetComponent<VRTK_InteractableObject>();
            if (io == null)
            {
                io = gameObject.AddComponent<VRTK_InteractableObject>();
            }
            io.isGrabbable = true;
            io.precisionSnap = true;
            io.grabAttachMechanic = VRTK_InteractableObject.GrabAttachType.Rotator_Track;
        }

        private void InitJoint()
        {
            hj = GetComponent<HingeJoint>();
            if (hj == null)
            {
                hj = gameObject.AddComponent<HingeJoint>();
                hj.useLimits = true;
                hj.anchor = new Vector3(0, -0.5f, 0);
                JointLimits limits = hj.limits;

                // this involves quite some guesswork. It is very hard to find general purpose settings but we can try. The user can still create the hingejoint himself.
                switch (direction)
                {
                    case LeverDirection.x:
                        hj.axis = new Vector3(0, 1, 0);
                        limits.min = -130;
                        break;
                    case LeverDirection.y:
                        hj.axis = new Vector3(0, 0, 1);
                        limits.min = -130;
                        break;
                    case LeverDirection.z:
                        hj.axis = new Vector3(1, 0, 0);
                        limits.min = -130;
                        break;
                }
                hj.limits = limits;
            }
        }

        private float CalculateValue()
        {
            float angleFraction = (hj.angle - hj.limits.min) / (hj.limits.max - hj.limits.min);
            return Mathf.Round((min + Mathf.Clamp01(angleFraction) * (max - min)) / stepSize) * stepSize;
        }

        private void SnapToValue(float value)
        {
            float angle = ((value - min) / (max - min)) * (hj.limits.max - hj.limits.min) + hj.limits.min;

            hj.useSpring = true;
            JointSpring spring = hj.spring;
            spring.targetPosition = angle;
            spring.spring = 10;
            spring.damper = 1;
            hj.spring = spring;
        }

        private void DisableSnapping()
        {
            hj.useSpring = false;
        }
    }
}