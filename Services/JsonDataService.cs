using System.IO;
using System.Text.Json;
using Zihun_OOP_Project_2026.Models;

namespace Zihun_OOP_Project_2026.Services;

public class JsonDataService : IDataService
{
    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    public AppData Load(string path)
    {
        if (!File.Exists(path))
            return CreateDefaultData();

        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<AppData>(json, _options) ?? CreateDefaultData();
    }

    public void Save(string path, AppData data)
    {
        string json = JsonSerializer.Serialize(data, _options);
        File.WriteAllText(path, json);
    }

    public AppData CreateDefaultData()
    {
        return new AppData
        {
            CurrentRole = UserRole.Guest,
            Courses = new()
            {
                new Course
                {
                    Id = 1,
                    Title = "Основи C# для початківців",
                    Category = "Програмування",
                    Teacher = "Віктор Франк",
                    DurationHours = 24,
                    Description = "Базовий курс з мови C#, синтаксису, класів, колекцій та об'єктно-орієнтованого програмування.",
                    Modules = new()
                    {
                        new CourseModule{Id=1, Title="Вступ до C#", Description="Знайомство з мовою та середовищем Visual Studio."},
                        new CourseModule{Id=2, Title="Класи та об'єкти", Description="Основи ООП, поля, властивості та методи."},
                        new CourseModule{Id=3, Title="Колекції та LINQ", Description="Робота зі списками, фільтрацією та пошуком даних."}
                    }
                },
                new Course
                {
                    Id = 2,
                    Title = "WPF Desktop UI",
                    Category = "Desktop-розробка",
                    Teacher = "Зак Арогундейд",
                    DurationHours = 30,
                    Description = "Курс з розробки графічних Desktop-застосунків засобами WPF, XAML та C#.",
                    Modules = new()
                    {
                        new CourseModule{Id=1, Title="Основи XAML", Description="Розмітка інтерфейсу користувача."},
                        new CourseModule{Id=2, Title="Елементи керування", Description="Кнопки, списки, поля введення та панелі."},
                        new CourseModule{Id=3, Title="Зв'язування даних", Description="Binding, ObservableCollection і оновлення UI."}
                    }
                },
                new Course
                {
                    Id = 3,
                    Title = "Основи кібербезпеки",
                    Category = "Інформаційна безпека",
                    Teacher = "Джамал",
                    DurationHours = 18,
                    Description = "Курс про базові принципи захисту інформації, автентифікацію, паролі та безпечну роботу в мережі.",
                    Modules = new()
                    {
                        new CourseModule{Id=1, Title="Загрози інформаційній безпеці", Description="Типові ризики та атаки."},
                        new CourseModule{Id=2, Title="Аутентифікація", Description="Паролі, MFA та ролі користувачів."},
                        new CourseModule{Id=3, Title="Безпечна поведінка", Description="Практичні правила захисту даних."}
                    }
                }
            }
        };
    }
}