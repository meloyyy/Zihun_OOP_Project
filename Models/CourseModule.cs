namespace Zihun_OOP_Project_2026.Models;

public class CourseModule : BaseEntity
{
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }

    public override string GetItemType()
    {
        return "Module";
    }
}