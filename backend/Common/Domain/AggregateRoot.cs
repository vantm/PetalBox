﻿using System.Text.Json.Serialization;

namespace Common.Domain;

public abstract class AggregateRoot<T> : Entity<T>, IAggregateRoot
{
    protected AggregateRoot(T id) : base(id)
    {
    }

    [JsonIgnore]
    private readonly IList<IDomainEvent> _events = [];

    protected void AddDomainEvent(IDomainEvent evt)
    {
        _events.Add(evt);
    }

    public IEnumerable<IDomainEvent> GetDomainEvents()
    {
        return [.. _events];
    }

    public void ClearDomainEvents()
    {
        _events.Clear();
    }
}
