using System;
using System.Collections.Generic;
using System.Text;
public interface IInteractable
{
    public Reward Interact(float deltaTime, float speedFactor); 
}