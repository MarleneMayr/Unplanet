using UnityEngine;
using DG.Tweening;

public class EndState : State
{
    [SerializeField] private Transform cameraStart;
    [SerializeField] private Transform cameraEnd;
    [SerializeField] private Camera cam;
    [SerializeField] private float flightDuration;

    public override void AfterActivate()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Menu.OnStartClicked.AddListener(StartGame);

        DOTween.Kill(cam.transform);
        cam.transform.SetPositionAndRotation(cameraStart.position, cameraStart.rotation);
        cam.transform.DOMove(cameraEnd.position, flightDuration).SetEase(Ease.InOutSine);
        cam.transform.DORotateQuaternion(cameraEnd.rotation, flightDuration).SetEase(Ease.InOutSine);

        audioManager.PlayEnd();
    }

    public override void BeforeDeactivate()
    {
        Menu.OnStartClicked.RemoveListener(StartGame);
        audioManager.FadeOut(AudioManager.GlobalSound.End, 0.1f);
    }

    private void StartGame()
    {
        stateMachine.GoTo<GameState>();
    }
}
