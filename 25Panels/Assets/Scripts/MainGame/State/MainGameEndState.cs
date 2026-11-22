using UnityEngine;

public partial class MainGameSceneCtrl
{
    public class MainGameEndState : StateBase<MainGameSceneCtrl>
    {
        public override void OnEnter()
        {
            Debug.Log("MainGameEndState.OnEnter");
            SceneCtrl.instance.LoadScene(SceneCtrl.Scene.RESULT);
        }
    }
}
