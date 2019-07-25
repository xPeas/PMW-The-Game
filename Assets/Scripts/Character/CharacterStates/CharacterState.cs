namespace Assets.Scripts.Character.CharacterStates
{
    public abstract class CharacterState
    {
        public abstract void HandleMovement();

        public abstract void HandleRotation();
    }
}
