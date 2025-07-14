using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    bool IsInteractable { get; }
    void SetInterface(bool active);
    void OnInteraction();

    event Action<IInteractable> OnInteracted;
}
