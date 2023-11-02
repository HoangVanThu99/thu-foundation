using Pancake;
using Pancake.Apex;
using Pancake.Scriptable;
using UnityEngine;

public class DiceRoller : GameComponent
{
    [SerializeField, Group("Event")] private ScriptableEventNoParam rollEvent;

    public void RollDicePressed()
    {
        gameObject.SetActive(false);
        rollEvent.Raise();
    }
}

