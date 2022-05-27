using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTile : MonoBehaviour
{
    public delegate void Reaction();

    Reaction reaction;

    public void SetReaction(Reaction reaction)
    {
        this.reaction = reaction;
    }

    private void OnTriggerStay(Collider other)
    {
        reaction?.Invoke();
    }
}
