local Event = Event

return {
    UpdateEvent = Event.NewSimple("Update"),
    LateUpdateEvent = Event.NewSimple("LateUpdate"),
    FixedUpdateEvent = Event.NewSimple("FixedUpdate"),
    DestroyEvent = Event.NewSimple("Destroy"),

    --- 通知事件
    NotifyEvent = Event.New("Notify"),
    --SystemEvent = Event.New("System"),
}