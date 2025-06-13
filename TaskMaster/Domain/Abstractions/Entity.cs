namespace Domain.Abstractions;

/// <summary>
/// Базовый класс для всех сущностей в системе.
/// </summary>
/// <typeparam name="TId">Тип идентификатора сущности.</typeparam>
public abstract class Entity<TId> where TId : struct
{
    /// <summary>
    /// Уникальный идентификатор сущности.
    /// </summary>
    public TId Id { get; set; }
}