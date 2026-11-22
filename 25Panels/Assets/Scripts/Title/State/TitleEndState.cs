using UnityEngine;

public partial class TitleSceneCtrl
{
    public class TitleEndState : StateBase<TitleSceneCtrl>
    {
        public override void OnEnter()
        {
            Debug.Log("TitleEndState.OnEnter");
            SceneCtrl.instance.LoadScene(SceneCtrl.Scene.MAINGAME);
        }

        public override void OnUpdate()
        {
            Debug.Log("TitleEndState.OnUpdate");
        }
    }
}
