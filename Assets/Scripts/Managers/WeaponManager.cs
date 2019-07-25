using Assets.Data;
using Assets.Scripts.Enumerations;


namespace Assets.Scripts.Managers
{
    [System.Serializable]
    public class WeaponManager
    {
        public ActionContainer[] actions;

        public ActionContainer GetAction(InputType t)
        {
            for (int i = 0; i < actions.Length; i++)
            {
                if (actions[i].inp == t)
                    return actions[i];
            }

            return null;
        }

        public void Init()
        {
            actions = new ActionContainer[4];
            for (int i = 0; i < actions.Length; i++)
            {
                ActionContainer a = new ActionContainer();
                a.inp = (InputType)i;
                actions[i] = a;
            }
        }

        [System.Serializable]
        public class ActionContainer
        {
            public InputType inp;
            public Action action;
        }
    }
}
