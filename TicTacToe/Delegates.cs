namespace TTT;

public delegate void EventCallback(string eventName, Dictionary<string, object> data);
public delegate void ListenCallback(Dictionary<string, object> data);