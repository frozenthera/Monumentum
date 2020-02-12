using Gamekit3D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monumentum
{
    public class PlayerController : SingletonBehaviour<PlayerController>
    {
        public float maxForwardSpeed = 1f;        // How fast Ellen can run.

        public float minTurnSpeed = 400f;         // How fast Ellen turns when moving at maximum speed.
        public float maxTurnSpeed = 1200f;        // How fast Ellen turns when stationary.

        public PlayerProgress progress = new PlayerProgress();

        protected CharacterController m_CharCtrl;
        protected PlayerInput m_Input;

        protected float m_DesiredForwardSpeed;         // How fast Ellen aims be going along the ground based on input.
        protected float m_ForwardSpeed;                // How fast Ellen is currently going along the ground.
        protected Quaternion m_TargetRotation;         // What rotation Ellen is aiming to have based on input.
        protected float m_AngleDiff;                   // Angle in degrees between Ellen's current rotation and her target rotation.

        private Checkpoint m_CurrentCheckpoint;

        const float k_GroundAcceleration = 2.5f;
        const float k_GroundDeceleration = 1f;

        protected bool IsMoveInput
        {
            get { return !Mathf.Approximately(m_Input.MoveInput.sqrMagnitude, 0f); }
        }

        public bool CanPull { get => progress.canPull; set => progress.canPull = value; }

        void Awake()
        {
            m_Input = GetComponent<PlayerInput>();
            m_CharCtrl = GetComponent<CharacterController>();
        }

        void FixedUpdate()
        {
            CheckPreprocess();

            if (m_Input.Pull && CanPull)
                CheckPullBlock();

            CalculateForwardMovement();

            SetTargetRotation();

            if (IsOrientationUpdated() && IsMoveInput)
                UpdateOrientation();

            #region OnAnimator
            Vector3 prePosition = transform.position;
            Vector3 movement = m_ForwardSpeed * transform.forward * Time.fixedDeltaTime;
            m_CharCtrl.Move(movement);
            if(!IsPlayerOnPath())
                transform.position = prePosition;
            #endregion
        }

        void CheckPreprocess()
        {
            if(Input.GetButtonDown("Reset"))
            {
                RoomController.Load();
            }
        }

        Coroutine m_PullBlockCoroutine;
        void CheckPullBlock()
        {
            int layerMask = 1 << LayerMask.NameToLayer("Block");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, float.PositiveInfinity , layerMask)){
                if (hit.transform.CompareTag("Pullable"))
                {
                    if (m_PullBlockCoroutine == null)
                        m_PullBlockCoroutine = StartCoroutine(PullBlock(hit.transform));
                }
            }
        }

        IEnumerator PullBlock(Transform transform)
        {
            transform.GetComponent<TileController>().OnPullStart();
            Vector3 start = Input.mousePosition;
            yield return new WaitUntil(() => !Input.GetMouseButton(0));
            transform.GetComponent<TileController>().OnPullEnd();

            Vector3 delta = Input.mousePosition - start;
            const int DragMinJudge = 40;

            if (delta.magnitude < DragMinJudge)
                yield break;

            Vector3 mouseDeltaDir = delta.normalized;
            Vector3 destinationPos = transform.position;

            if (Mathf.Abs(mouseDeltaDir.x) > Mathf.Abs(mouseDeltaDir.y))
            {
                if (mouseDeltaDir.x > 0)
                    destinationPos += Vector3.right;
                else
                    destinationPos += Vector3.left;
            }
            else
            {
                if (mouseDeltaDir.y > 0)
                    destinationPos += Vector3.forward;
                else
                    destinationPos += Vector3.back;
            }

            if(IsPlayerOnPath(out var path) && path != transform)
            {
                if (!destinationPos.HasBlock())
                {
                    const float speed = 10f;
                    float count = 3f;
                    while (count > 0)
                    {
                        transform.position = Vector3.Lerp(transform.position, destinationPos, speed * Time.deltaTime);
                        count--;
                        yield return null;
                    }
                    transform.position = destinationPos;
                }
            }
            m_PullBlockCoroutine = null;
        }

        // Called each physics step.
        void CalculateForwardMovement()
        {
            // Cache the move input and cap it's magnitude at 1.
            Vector2 moveInput = m_Input.MoveInput;
            if (moveInput.sqrMagnitude > 1f)
                moveInput.Normalize();

            // Calculate the speed intended by input.
            m_DesiredForwardSpeed = moveInput.magnitude * maxForwardSpeed;

            // Determine change to speed based on whether there is currently any move input.
            float acceleration = IsMoveInput ? k_GroundAcceleration : k_GroundDeceleration;

            // Adjust the forward speed towards the desired speed.
            m_ForwardSpeed = Mathf.MoveTowards(m_ForwardSpeed, m_DesiredForwardSpeed, acceleration * Time.deltaTime);

            // Set the animator parameter to control what animation is being played.
            //m_Animator.SetFloat(m_HashForwardSpeed, m_ForwardSpeed);
        }

        void SetTargetRotation()
        {
            // Create three variables, move input local to the player, flattened forward direction of the camera and a local target rotation.
            Vector2 moveInput = m_Input.MoveInput;
            Vector3 localMovementDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

            /*Vector3 forward = Quaternion.Euler(0f, cameraSettings.Current.m_XAxis.Value, 0f) * Vector3.forward;
            forward.y = 0f;
            forward.Normalize();*/

            Quaternion targetRotation = localMovementDirection == Vector3.zero ? Quaternion.identity : Quaternion.LookRotation(localMovementDirection);

            // If the local movement direction is the opposite of forward then the target rotation should be towards the camera.
            //if (Mathf.Approximately(Vector3.Dot(localMovementDirection, Vector3.forward), -1.0f)) (하지만 카메라의 방향이 캐릭터 이동에 영향을 끼치지 않는 게임이므로 생략.)

            // The desired forward direction of Ellen.
            Vector3 resultingForward = localMovementDirection;//targetRotation * Vector3.forward;

            // If attacking try to orient to close enemies. (우리는 공격 개념이 없으므로 사용하지 않아요.)
            //if (m_InAttack) ....

            // Find the difference between the current rotation of the player and the desired rotation of the player in radians.
            float angleCurrent = Mathf.Atan2(transform.forward.x, transform.forward.z) * Mathf.Rad2Deg;
            float targetAngle = Mathf.Atan2(resultingForward.x, resultingForward.z) * Mathf.Rad2Deg;

            m_AngleDiff = Mathf.DeltaAngle(angleCurrent, targetAngle);
            m_TargetRotation = targetRotation;
        }

        // Called each physics step to help determine whether Ellen can turn under player input.
        bool IsOrientationUpdated()
        {
            /*bool updateOrientationForLocomotion = !m_IsAnimatorTransitioning && m_CurrentStateInfo.shortNameHash == m_HashLocomotion || m_NextStateInfo.shortNameHash == m_HashLocomotion;
            bool updateOrientationForAirborne = !m_IsAnimatorTransitioning && m_CurrentStateInfo.shortNameHash == m_HashAirborne || m_NextStateInfo.shortNameHash == m_HashAirborne;
            bool updateOrientationForLanding = !m_IsAnimatorTransitioning && m_CurrentStateInfo.shortNameHash == m_HashLanding || m_NextStateInfo.shortNameHash == m_HashLanding;*/

            return true;//updateOrientationForLocomotion || updateOrientationForAirborne || updateOrientationForLanding || m_InCombo && !m_InAttack;
        }

        // Called each physics step after SetTargetRotation if there is move input and Ellen is in the correct animator state according to IsOrientationUpdated.
        void UpdateOrientation()
        {
            //m_Animator.SetFloat(m_HashAngleDeltaRad, m_AngleDiff * Mathf.Deg2Rad);

            //Vector3 localInput = new Vector3(m_Input.MoveInput.x, 0f, m_Input.MoveInput.y);
            //float groundedTurnSpeed = Mathf.Lerp(maxTurnSpeed, minTurnSpeed, m_ForwardSpeed / m_DesiredForwardSpeed);
            //float actualTurnSpeed = m_IsGrounded ? groundedTurnSpeed : Vector3.Angle(transform.forward, localInput) * k_InverseOneEighty * k_AirborneTurnSpeedProportion * groundedTurnSpeed;
            float actualTurnSpeed = Mathf.Lerp(maxTurnSpeed, minTurnSpeed, m_ForwardSpeed / m_DesiredForwardSpeed);
            m_TargetRotation = Quaternion.RotateTowards(transform.rotation, m_TargetRotation, actualTurnSpeed * Time.deltaTime);

            transform.rotation = m_TargetRotation;
        }

        bool IsPlayerOnPath()
        {
            int layerMask = 1 << LayerMask.NameToLayer("Path");
            Ray rayFromPlayer = new Ray(transform.position, transform.TransformDirection(Vector3.down));
            const float Distance = 5f;
            return Physics.Raycast(rayFromPlayer, Distance, layerMask);
        }
        bool IsPlayerOnPath(out Transform pathBlock)
        {
            int layerMask = 1 << LayerMask.NameToLayer("Path");
            Ray rayFromPlayer = new Ray(transform.position, transform.TransformDirection(Vector3.down));
            const float Distance = 5f;
            bool ret = Physics.Raycast(rayFromPlayer, out var hitInfo, Distance, layerMask);
            pathBlock = hitInfo.transform;
            return ret;
        }

        // This is called by Checkpoints to make sure Ellen respawns correctly.
        public void SetCheckpoint(Checkpoint checkpoint)
        {
            if (checkpoint != null)
                m_CurrentCheckpoint = checkpoint;
        }

        // This is usually called by a state machine behaviour on the animator controller but can be called from anywhere.
        public void Respawn()
        {
            StartCoroutine(RespawnRoutine());
        }

        protected IEnumerator RespawnRoutine()
        {
            // Wait for the animator to be transitioning from the EllenDeath state.
            //while (m_CurrentStateInfo.shortNameHash != m_HashEllenDeath || !m_IsAnimatorTransitioning)
            {
                yield return null;
            }

            // Wait for the screen to fade out.
            yield return StartCoroutine(ScreenFader.FadeSceneOut());
            while (ScreenFader.IsFading)
            {
                yield return null;
            }

            // Enable spawning.
            //EllenSpawn spawn = GetComponentInChildren<EllenSpawn>();
            //spawn.enabled = true;

            // If there is a checkpoint, move Ellen to it.
            if (m_CurrentCheckpoint != null)
            {
                transform.position = m_CurrentCheckpoint.transform.position;
                transform.rotation = m_CurrentCheckpoint.transform.rotation;
            }
            else
            {
                Debug.LogError("There is no Checkpoint set, there should always be a checkpoint set. Did you add a checkpoint at the spawn?");
            }

            // Set the Respawn parameter of the animator.
            //m_Animator.SetTrigger(m_HashRespawn);

            // Start the respawn graphic effects.
            //spawn.StartEffect();

            // Wait for the screen to fade in.
            // Currently it is not important to yield here but should some changes occur that require waiting until a respawn has finished this will be required.
            yield return StartCoroutine(ScreenFader.FadeSceneIn());
        }

        // Called by a state machine behaviour on Ellen's animator controller.
        public void RespawnFinished()
        {
            //m_Respawning = false;
        }
        [System.Serializable]
        public class PlayerProgress
        {
            public bool canPull = false;            //당길 수 있는가 
        }
    }
}

/*private IEnumerator WaitForInteract
        {
            get
            {
                while (true)
                {
                    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.F));
                    player.TryInteract();
                    yield return null;
                }
            }
        }

        private IEnumerator Rotate90Character(bool isClockwise = true, float duration = 1.0f)
        {
            Vector3 center = transform.position.GetTileCenter();
            
            float radius = Vector3.Distance(transform.position, center);
            float radFrom = Mathf.Acos(-(transform.position.x - center.x) / radius) + Mathf.PI;
            float radTo = radFrom + Mathf.PI / (!isClockwise ? 2f : -2f);

            float elapsed = 0.0f;
            while (elapsed < duration)
            {
                radFrom = Mathf.Lerp(radFrom, radTo, elapsed / duration); //Quaternion.Slerp(from, to, elapsed / duration);
                transform.position = new Vector3(center.x + radius * Mathf.Cos(radFrom), center.y + radius * Mathf.Sin(radFrom));
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = new Vector3(center.x + radius * Mathf.Cos(radTo), center.y + radius * Mathf.Sin(radTo));
        }*/
/*private IEnumerator Update
        {
            get
            {
                while (true)
                {
                    player.Walk(Speed * Time.deltaTime * new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
                    yield return new WaitUntil(() => Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0);
                }
            }
        }*/