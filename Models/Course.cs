using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Zihun_OOP_Project_2026.Models;

public delegate void CourseCompletionHandler(string courseTitle);

public class Course : BaseEntity
{
    private int _durationHours;
    private int _lastProgress;

    public static int TotalCoursesCount { get; private set; }

    public event CourseCompletionHandler? CourseCompleted;

    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Teacher { get; set; } = string.Empty;
    public bool IsEnrolled { get; set; }
    public ObservableCollection<CourseModule> Modules { get; set; } = new();

    public int DurationHours
    {
        get => _durationHours;
        set
        {
            if (value < 0) _durationHours = 0;
            else _durationHours = value;
        }
    }

    public Course()
    {
        TotalCoursesCount++;
    }

    public override string GetItemType()
    {
        return "Course";
    }

    public int Progress
    {
        get
        {
            if (Modules.Count == 0) return 0;
            int progress = (int)Math.Round(Modules.Count(m => m.IsCompleted) * 100.0 / Modules.Count);

            if (progress == 100 && _lastProgress != 100)
            {
                CourseCompleted?.Invoke(Title);
            }

            _lastProgress = progress;
            return progress;
        }
    }
}