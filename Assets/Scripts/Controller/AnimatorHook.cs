using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class AnimatorHook : MonoBehaviour
    {
        Animator anim;
        StatesManager states;

        public void Init(StatesManager st)
        {
            states = st;
            anim = states.anim;
        }

        void OnAnimatorMove()
        {
            states.inp.animDelta = anim.deltaPosition;
            transform.localPosition = Vector3.zero;
        }
    }
}
