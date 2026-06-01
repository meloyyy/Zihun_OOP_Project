namespace Zihun_OOP_Project_2026.Models;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;

    public abstract string GetItemType();
}