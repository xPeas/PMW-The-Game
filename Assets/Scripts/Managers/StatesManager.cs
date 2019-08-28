using Assets.Scripts.Controllers;
using Assets.Scripts.Enumerations;
using Assets.Data;
using UnityEngine;
using Assets.Scripts.References;
using Assets.Scripts.Scriptables;
using Assets.Scripts.Scriptables.Variables;
using Assets.Scripts.Character;

namespace Assets.Scripts.Managers
{
    public class StatesManager : MonoBehaviour
    {
        public ControllerStats stats;
        public States states;
        [HideInInspector]
        public InputVariables inp;
        public GameObject activeModel;

        public InventoryManager inv_manager;
        public WeaponManager w_manager;
        [HideInInspector]
        public ResourcesManager r_manager;

        #region References
        [HideInInspector]
        public Animator anim;
        [HideInInspector]
        public AnimatorHook a_hook;
        [HideInInspector]
        public Rigidbody rigid;
        [HideInInspector]
        public Collider controllerCollider;
        #endregion

        [HideInInspector]
        public LayerMask ignoreLayers;
        [HideInInspector]
        public LayerMask ignoreForGroundCheck;        
        [HideInInspector]
        public float jumpHeight = 10f;

        public float delta;
        public Transform mTransform;
        private Vector3 currentVelocity;
        private float distToGround;

        private System.Action<bool> GroundChanged;

        //Collider Variables
        private PhysicMaterial capsuleMatt;

        public CharState charState;
        public enum CharState
        {
            moving,
            onAir,
            armsInteracting,
            overrideLayerInteracting
        }

        #region Init
        public void Init()
        {
            inp = new InputVariables();
            if (r_manager == null)
            {
                r_manager = Resources.Load("ResourcesManager") as ResourcesManager;
            }
            SetupAnimator();
            mTransform = this.transform;
            rigid = GetComponent<Rigidbody>();
            controllerCollider = GetComponent<CapsuleCollider>();
            distToGround = controllerCollider.bounds.extents.y;
            rigid.angularDrag = 999;
            rigid.drag = 4;
            rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            
            gameObject.layer = 8;
            ignoreLayers = ~(1 << 9);
            ignoreForGroundCheck = ~(1 << 9 | 1 << 10);

            a_hook = activeModel.AddComponent<AnimatorHook>();
            a_hook.Init(this);

            InitInventory();
            InitWeaponManager();

            //Set up ground changing for physics material values
            capsuleMatt = GetComponent<Collider>().material;
            GroundChanged += UpdateFrictionValues;
        }
        
        void InitInventory()
        {
            if (inv_manager.rh_item)
            {
                WeaponToRuntime(inv_manager.rh_item.obj, ref inv_manager.rh);
                EquipWeapon(inv_manager.rh, false);
            }
            if (inv_manager.lh_item)
            {
                WeaponToRuntime(inv_manager.lh_item.obj, ref inv_manager.lh);
                EquipWeapon(inv_manager.lh, false);
            }
        }

        void WeaponToRuntime(Object obj, ref Inventory.RuntimeWeapon slot)
        {
            Inventory.Weapon w = (Inventory.Weapon)obj;
            GameObject go = Instantiate(w.modelPrefab) as GameObject;
            go.SetActive(false);
            Inventory.RuntimeWeapon rw = new Inventory.RuntimeWeapon();
            rw.instance = go;
            rw.w_actual = w;

            r_manager.runtime.RegesterRW(rw);
        }

        void EquipWeapon(Inventory.RuntimeWeapon rw, bool isLeft)
        {
            Vector3 p = Vector3.zero;
            Vector3 e = Vector3.zero;
            Vector3 s = Vector3.one;
            Transform par = null;

            if (isLeft)
            {
                p = rw.w_actual.lh_position.pos;
                e = rw.w_actual.lh_position.eulers;
                par = anim.GetBoneTransform(HumanBodyBones.LeftHand);
            }
            else
            {
                par = anim.GetBoneTransform(HumanBodyBones.RightHand);
            }

            rw.instance.transform.parent = par;
            rw.instance.transform.localPosition = p;
            rw.instance.transform.localEulerAngles = e;
            rw.instance.transform.localScale = s;

            rw.instance.SetActive(true);
        }

        void InitWeaponManager()
        {
            if (inv_manager.lh == null && inv_manager.rh == null)
                return;

            if (inv_manager.rh != null)
            {
                WeaponManager.ActionContainer rb = w_manager.GetAction(InputType.rb);
                rb.action = inv_manager.rh.w_actual.GetAction(InputType.rb);

                WeaponManager.ActionContainer rt = w_manager.GetAction(InputType.rt);
                rt.action = inv_manager.rh.w_actual.GetAction(InputType.rt);

                if (inv_manager.lh == null)
                {
                    WeaponManager.ActionContainer lb = w_manager.GetAction(InputType.lb);
                    lb.action = inv_manager.rh.w_actual.GetAction(InputType.lb);

                    WeaponManager.ActionContainer lt = w_manager.GetAction(InputType.lt);
                    lt.action = inv_manager.rh.w_actual.GetAction(InputType.lt);
                }
                else
                {
                    WeaponManager.ActionContainer lb = w_manager.GetAction(InputType.lb);
                    lb.action = inv_manager.lh.w_actual.GetAction(InputType.lb);

                    WeaponManager.ActionContainer lt = w_manager.GetAction(InputType.lt);
                    lt.action = inv_manager.lh.w_actual.GetAction(InputType.lt);
                }

                return;
            }

            if (inv_manager.lh != null)
            {
                WeaponManager.ActionContainer rb = w_manager.GetAction(InputType.rb);
                rb.action = inv_manager.lh.w_actual.GetAction(InputType.rb);

                WeaponManager.ActionContainer rt = w_manager.GetAction(InputType.rt);
                rt.action = inv_manager.lh.w_actual.GetAction(InputType.rt);

                WeaponManager.ActionContainer lb = w_manager.GetAction(InputType.lb);
                lb.action = inv_manager.lh.w_actual.GetAction(InputType.lb);

                WeaponManager.ActionContainer lt = w_manager.GetAction(InputType.lt);
                lt.action = inv_manager.lh.w_actual.GetAction(InputType.lt);
            }
        }


        void SetupAnimator()
        {
            if (activeModel == null)
            {
                anim = GetComponentInChildren<Animator>();
                activeModel = anim.gameObject;
            }

            if (anim == null)
                anim = GetComponentInChildren<Animator>();

            anim.applyRootMotion = false;
            anim.GetBoneTransform(HumanBodyBones.LeftHand).localScale = Vector3.one;
            anim.GetBoneTransform(HumanBodyBones.RightHand).localScale = Vector3.one;
        }
        #endregion

        #region Fixed Update
        public void Fixed_Tick(float d)
        {
            delta = d;
            if (states.onGround)
            {
                charState = CharState.moving;
            }
            else
            {
                charState = CharState.onAir;
            }
            
            switch (charState)
            {
                case CharState.moving:
                    HandleRotation();
                    HandleMovement();
                    break;
                case CharState.onAir:
                    HandleRotation();
                    HandleAerialMovement();
                    break;
                case CharState.armsInteracting:
                    break;
                case CharState.overrideLayerInteracting:
                    rigid.drag = 0;
                    Vector3 v = rigid.velocity;
                    Vector3 tv = inp.animDelta;
                    tv *= 95;
                    tv.y = v.y;
                    rigid.velocity = tv;
                    break;
                default:
                    break;
            }
        }

        private void HandleRotation()
        {
            Vector3 targetDir = (states.isLockedOn == false) ?
                inp.moveDir :
                (inp.lockOnTransform == null) ? inp.lockOnTransform.position - mTransform.position : inp.moveDir;

            targetDir.y = 0;
            if (targetDir == Vector3.zero)
                targetDir = mTransform.forward;

            Quaternion tr = Quaternion.LookRotation(targetDir);            
            Quaternion targetRotation = Quaternion.Slerp(mTransform.rotation, tr, delta *  inp.moveAmount * stats.rotateSpeed );
            mTransform.rotation = targetRotation;
        }

        private void HandleMovement()
        {
            currentVelocity = mTransform.forward;
            if (states.isLockedOn)
                currentVelocity = inp.moveDir;

            if (inp.moveAmount > 0 || states.isJumping)
                rigid.drag = 0;
            else
                rigid.drag = 4;
            
            if (states.isRunning)
                currentVelocity *= inp.moveAmount * stats.sprintSpeed;
            else
                currentVelocity *= inp.moveAmount * stats.moveSpeed;

            if (inp.jump > 0f)
            {
                currentVelocity.y = 10f;
            }
            rigid.velocity = currentVelocity;
        }

        private void HandleAerialMovement()
        {
            //allows limited aerial steering
            rigid.velocity = new Vector3(currentVelocity.x + (inp.moveDir.x / .5f), rigid.velocity.y, currentVelocity.z + (inp.moveDir.z / .5f)); 
        }
        #endregion

        #region Update
        public void Tick(float d)
        {
            delta = d;
            GroundCheck();

            switch (charState)
            {
                case CharState.moving:
                    bool interact = CheckForInteractionInput();
                    if (!interact)
                    {
                        HandleMovementAnim();
                    }
                    break;
                case CharState.onAir:
                    break;
                case CharState.armsInteracting:

                    break;
                case CharState.overrideLayerInteracting:
                    states.animIsInteracting = anim.GetBool("isInteracting");
                    if (states.animIsInteracting == false)
                    {
                        if (states.isInteracting)
                        {
                            states.isInteracting = false;
                            ChangeState(CharState.moving);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        bool CheckForInteractionInput()
        {
            Action a = null;

            if (inp.rb)
            {
                a = GetAction(InputType.rb);
                if (a != null)
                {
                    HandleAction(a);
                    return true;
                }
            }

            if (inp.rt)
            {
                a = GetAction(InputType.rt);
                if (a != null)
                {
                    HandleAction(a);
                    return true;
                }
            }

            if (inp.lb)
            {
                a = GetAction(InputType.lb);
                if (a != null)
                {
                    HandleAction(a);
                    return true;
                }
            }

            if (inp.lt)
            {
                a = GetAction(InputType.lt);
                if (a != null)
                {
                    HandleAction(a);
                    return true;
                }
            }

            return false;
        }


        #endregion

        #region Manager Functions
        void HandleAction(Action a)
        {
            switch (a.actionType)
            {
                case ActionType.attack:
                    AttackAction aa = (AttackAction)a.action_obj;
                    PlayAttackAction(a, aa);
                    break;
                case ActionType.block:
                    break;
                case ActionType.spell:
                    break;
                case ActionType.parry:
                    break;
                default:
                    break;
            }
        }

        Action GetAction(InputType inp)
        {
            WeaponManager.ActionContainer ac = w_manager.GetAction(inp);
            if (ac == null)
                return null;
            return ac.action;
        }

        void PlayInteractAnimation(string a)
        {
            //I SET THIS MYSELF, THE CROSSFADE DOESN'T WORK RIGHT
            anim.CrossFade(a, 0.05f);
            anim.PlayInFixedTime(a, 5, 0.05f);
        }

        void PlayAttackAction(Action a, AttackAction aa)
        {
            anim.SetBool(StaticStrings.mirror, a.isMirrored);
            PlayInteractAnimation(aa.attack_anim.value);
            if (aa.changeSpeed)
            {
                anim.SetFloat("speed", aa.animSpeed);
            }
            ChangeState(CharState.overrideLayerInteracting);
        }

        void HandleMovementAnim()
        {
            if (states.isLockedOn)
            {

            }
            else
            {
                anim.SetBool(StaticStrings.run, states.isRunning);
                anim.SetFloat(StaticStrings.vertical, inp.moveAmount, 0.15f, delta);
            }
        }

        void ChangeState(CharState t)
        {
            charState = t;
            switch (t)
            {
                case CharState.moving:
                    anim.applyRootMotion = false;
                    break;
                case CharState.onAir:
                    anim.applyRootMotion = false;
                    break;
                case CharState.armsInteracting:
                    anim.applyRootMotion = false;
                    break;
                case CharState.overrideLayerInteracting:
                    anim.applyRootMotion = true;
                    anim.SetBool("isInteracting", true);
                    states.isInteracting = true;
                    break;
                default:
                    break;
            }
        }
        #endregion

        private void GroundCheck()
        {
            Vector3 origin = mTransform.position;
            Vector3 dir = -Vector3.up;
            RaycastHit hit;
            float dis = .6f;

            origin.y += 0.4f;
            Debug.DrawRay(origin, dir * dis);

            bool newGroundCheck = Physics.Raycast(origin, dir, out hit, dis, ignoreForGroundCheck);

            //Save the value if the onGround value has changed
            bool groundChanged = newGroundCheck != states.onGround;

            if (newGroundCheck)
            {
                states.onGround = true;
                states.isJumping = false;
            }
            else
            {
                states.onGround = false;
                states.isJumping = true;
            }

            //If the OnGround value has changed and there are functions listening, call GroundChanged 
            if(groundChanged == true && GroundChanged != null)
            {
                GroundChanged(newGroundCheck);
            }
        }

        private void UpdateFrictionValues(bool isOnGround)
        {
            //0 friction for sliding agsin't walls, 1 friction to stop the sliding on slopes
            capsuleMatt.staticFriction = isOnGround ? 1f : 0f;
        }
    }

}