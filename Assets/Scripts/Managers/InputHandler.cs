using UnityEngine;
using Assets.Scripts.References;
using Assets.Scripts.Enumerations;

namespace Assets.Scripts.Managers
{
    public class InputHandler : MonoBehaviour
    {
        float vertical;
        float horizontal;
        float jump;
        bool b_input;
        bool x_input;
        bool a_input;
        bool y_input;

        bool rb_input;
        bool rt_input;
        bool lb_input;
        bool lt_input;

        float rt_axis;
        float lt_axis;

        float b_timer;

        float delta;

        public Gamephase curPhase;
        public StatesManager states;
        public CameraController camManager;

        #region Init
        void Start()
        {
            InitinGame();

        }

        void FixedUpdate()
        {
            delta += Time.deltaTime;
            GetInput_FixedUpdate();

            switch (curPhase)
            {
                case Gamephase.inGame:
                    InGame_UpdateStates_FixedUpdate();
                    states.Fixed_Tick(delta);
                    break;
                case Gamephase.inMenu:
                    break;
                case Gamephase.inInventory:
                    break;
                default:
                    break;
            }
        }

        public void InitinGame()
        {
            states.Init();
            
        }
        #endregion

        #region Fixed Update
        void GetInput_FixedUpdate()
        {
            vertical = Input.GetAxis(StaticStrings.Vertical);
            horizontal = Input.GetAxis(StaticStrings.Horizontal);
            jump = Input.GetAxis(StaticStrings.jump);
        }

        void InGame_UpdateStates_FixedUpdate()
        {            
            states.inp.vertical = vertical;
            states.inp.horizontal = horizontal;
            states.inp.jump = jump;

            var speed = Mathf.Abs(vertical) + Mathf.Abs(horizontal);
            states.inp.moveAmount = Mathf.Clamp(speed,0,1f);
            
            Vector3 input = new Vector3(horizontal, jump, vertical);
            Vector3 worldSpaceInput = CameraRelativeFlatten(input, Vector3.up);
            states.inp.moveDir = worldSpaceInput;
        }

        Vector3 CameraRelativeFlatten(Vector3 input, Vector3 localUp)
        {
            return Quaternion.LookRotation(-localUp,camManager.cameraTransform.forward) * Quaternion.Euler(Vector3.right * -90f) * input;            
        }
        #endregion 

        #region Update
        void Update()
        {
            delta += Time.deltaTime;
            GetInput_Update();

            switch (curPhase)
            {
                case Gamephase.inGame:
                    InGame_UpdateStates_Update();
                    states.Tick(delta);
                    break;
                case Gamephase.inMenu:
                    break;
                case Gamephase.inInventory:
                    break;
                default:
                    break;
            }
        }

        void GetInput_Update()
        {
            b_input = Input.GetButton(StaticStrings.B);
            a_input = Input.GetButton(StaticStrings.A);
            y_input = Input.GetButton(StaticStrings.Y);
            x_input = Input.GetButton(StaticStrings.X);
            rt_input = Input.GetButton(StaticStrings.RT);
            rt_axis = Input.GetAxis(StaticStrings.RT);
            if (rt_axis != 0)
                rt_input = true;
            lt_input = Input.GetButton(StaticStrings.LT);
            lt_axis = Input.GetAxis(StaticStrings.LT);
            if (lt_axis != 0)
                lt_input = true;
            rb_input = Input.GetButton(StaticStrings.RB);
            lb_input = Input.GetButton(StaticStrings.LB);

            if (b_input)
                b_timer += delta;
        }

        void InGame_UpdateStates_Update()
        {
            states.inp.rb = rb_input;
            states.inp.rt = rt_input;
            states.inp.lb = lb_input;
            states.inp.lt = lt_input;
        }
    }
    #endregion
}