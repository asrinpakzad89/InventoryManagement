namespace Domain.Common;

public abstract class BaseEntity<TKey>
    : IEntity<TKey>
{
    public TKey Id { get; set; }
    public bool IsDisable { get; set; } = false;
    public bool IsDelete { get; set; } = false;
}
