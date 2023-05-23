using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class ThirdPersonAdapter : PlayerController{
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
            UpdateState();
            m_Jump = false;
        }
        else if (CurrentState == PlayerState.FailedJump)
        {
            m_Move = Vector3.zero;
            m_Jump = false;
        }
        else
        {
            m_Move = Vector3.right;
            m_Jump = false;
        }
    }

    protected override void UpdateState()
    {
        m_Character.Move(m_Move, false, m_Jump);
    }
}