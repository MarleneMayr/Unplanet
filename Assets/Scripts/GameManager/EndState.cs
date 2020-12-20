using UI;
using UnityEngine;
using DG.Tweening;

public class EndState : State
{
    [SerializeField] Transform cameraStart;
    [SerializeField] Transform cameraEnd;
    [SerializeField] Camera cam;
    //[SerializeField] Camera playerCam;

    public override void AfterActivate()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Menu.OnStartClicked.AddListener(StartGame);
        cam.transform.SetPositionAndRotation(cameraStart.position, cameraStart.rotation);
        cam.transform.DOLocalMove(cameraEnd.position, 10).SetEase(Ease.InOutSine);

    }

    public override void BeforeDeactivate()
    {
        Menu.OnStartClicked.RemoveListener(StartGame);
    }

    private void StartGame()
    {
        stateMachine.GoTo<GameState>();
    }
}
