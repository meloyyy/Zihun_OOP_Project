using System.Collections.ObjectModel;

namespace Zihun_OOP_Project_2026.Models;

public class AppData
{
    public UserRole CurrentRole { get; set; } = UserRole.Guest;
    public ObservableCollection<Course> Courses { get; set; } = new();
}
