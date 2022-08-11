using System;
using System.Collections.Generic;
using UnityEngine;
public class StateMachine
{

  private Dictionary<Type,List<Transition>> _transitions;
  private List<Transition> _currentTransitions;
  private List<Transition> _anyTransitions;

  private List<Transition> s_emptyTransitions;
  private IState _currentState;

  public StateMachine()
  {
    _transitions = new Dictionary<Type, List<Transition>>();
    _currentTransitions = new List<Transition>();
    _anyTransitions = new List<Transition>();
    s_emptyTransitions = new List<Transition>();
  }


  public void SetState(IState state)
  {
     if(state == _currentState)
     {
        return;
     }

     _currentState?.OnExit();
     _currentState = state;

     _transitions.TryGetValue(_currentState.GetType(), out _currentTransitions);
     if(_currentTransitions == null)
     {
        _currentTransitions = s_emptyTransitions;
     }

     _currentState?.OnEnter();
  }

  public void AddTransition(IState from,IState to,Func<bool> checkCallback)
  {
    if(_transitions.TryGetValue(from.GetType(),out var transitions) == false)
    {
        transitions = new List<Transition>();
        _transitions[from.GetType()] = transitions;
    }
    
    transitions.Add(new Transition(to,checkCallback));
  }

  

  public void AddAnyTransition(IState from,IState to,Func<bool> checkCallback)
  {
    _anyTransitions.Add(new Transition(to,checkCallback));
  }

  

  public void OnUpdate()
  {
      //Debug.Log("Current state : " + _currentState.ToString());
      Transition transition = GetTransition();
      if(transition != null)
      {
        SetState(transition.ToState);
      }
      _currentState?.OnUpdate();
  }

  private class Transition
   {
      public Func<bool> Condition;
      public IState ToState;

      
      public Transition(IState toState, Func<bool> condition)
      {
         ToState = toState;
         Condition = condition;
      }

   }

   private Transition GetTransition()
   {
     foreach(Transition transition in _anyTransitions)
     {
        if(transition.Condition())
        {
            return transition;
        }
     }

     foreach(Transition transition in _currentTransitions)
     {
        if(transition.Condition())
        {
            return transition;
        }
     }
     return null;
   }
}
