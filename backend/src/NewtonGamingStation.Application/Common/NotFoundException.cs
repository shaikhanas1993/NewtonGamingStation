namespace NewtonGamingStation.Application.Common;

/// <summary>Thrown by the application layer when a requested entity does not exist.</summary>
public class NotFoundException : Exception
{
    public NotFoundException(string entity, object key)
        : base($"{entity} with key '{key}' was not found.")
    {
    }
}
