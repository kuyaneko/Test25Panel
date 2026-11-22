using UnityEngine;


public partial class ResultSceneCtrl
{
    public class ResultMainState : StateBase<ResultSceneCtrl>
    {
        public override void OnEnter()
        {
            Debug.Log("ResultMainState.OnEnter");
        }

        public override void OnUpdate()
        {
            Debug.Log("ResultMainState.OnUpdate");

            if (UnityEngine.InputSystem.Mouse.current != null &&
                UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame)
            {
                owner.SetState<ResultEndState>();
            }
        }
    }
}
