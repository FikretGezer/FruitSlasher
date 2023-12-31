using System;
using System.Collections.Generic;

namespace Runtime
{
    public enum GameEvents{
        OnStartGame,
        OnFinishGame,
        OnPlayerValuesChanges,
        OnCuttingBomb,
        OnCuttingFruit,
        OnCuttingSpecialFruit
    }
    public static class EventManager
    {
        private static Dictionary<GameEvents, Action> _events = new Dictionary<GameEvents, Action>();

        public static void AddHandler(GameEvents _gameEvent, Action _action)
        {
            if(!_events.ContainsKey(_gameEvent))
                _events[_gameEvent] = _action;
            else
                _events[_gameEvent] += _action;
        }
        public static void RemoveHandler(GameEvents _gameEvent, Action _action)
        {
            if(_events[_gameEvent] != null)
                _events[_gameEvent] -= _action;

            if(_events[_gameEvent] == null)
                _events.Remove(_gameEvent);

        }
        public static void Broadcasting(GameEvents _gameEvent)
        {
            if(_events[_gameEvent] != null)
                _events[_gameEvent]();
        }
    }
}
