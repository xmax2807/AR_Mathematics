using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

class ThirdPersonAdapter : PlayerController{
        [SerializeField]private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Vector3 m_Move = Vector3.right;
        private bool m_Jump;
    public override void ChangeState(PlayerState state)
    {
        CurrentState = state;
        if (CurrentState == PlayerState.HighJump)
        {
            m_Move = Vector3.right;
            m_Jump = true;
        }
        else if (CurrentState == PlayerState.FailedJump)
        {
        }
        else
        {
            m_Move = Vector3.right;
        }
    }

    protected override void UpdateState()
    {
        m_Character.Move(m_Move, false, m_Jump);
    }
}