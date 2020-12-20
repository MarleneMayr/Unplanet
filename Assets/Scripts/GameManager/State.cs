using UI;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    [SerializeField] protected Menu menu;
    protected float menuFadeDuration = 0.2f;

    protected StateMachine stateMachine;
    protected static AudioManager audioManager;

    public void Activate(StateMachine sm)
    {
        audioManager ??= FindObjectOfType<AudioManager>();
        stateMachine = sm;
        menu?.Show(menuFadeDuration);

        AfterActivate();
    }

    public void Deactivate()
    {
        BeforeDeactivate();

        menu?.Hide(menuFadeDuration);
    }

    public abstract void AfterActivate();
    public abstract void BeforeDeactivate();

    public virtual void OnGeneralEvent() { }
}
