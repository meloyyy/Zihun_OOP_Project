using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Zihun_OOP_Project_2026.Models;
using Zihun_OOP_Project_2026.Services;

namespace Zihun_OOP_Project_2026;

public partial class MainWindow : Window
{
    private const string DataFilePath = "courses_data.json";
    private readonly IDataService _jsonService = new JsonDataService();
    private AppData _data = new();
    private ObservableCollection<Course> _visibleCourses = new();
    private Course? _selectedCourse;

    public MainWindow()
    {
        InitializeComponent();
        _data = _jsonService.Load(DataFilePath);
        RoleComboBox.SelectedIndex = _data.CurrentRole == UserRole.Guest ? 0 : 1;
        RefreshCourses();
    }

    private void RefreshCourses(bool onlyMyCourses = false)
    {
        string search = SearchTextBox?.Text?.Trim().ToLower() ?? string.Empty;
        var filtered = _data.Courses.Where(c =>
            (!onlyMyCourses || c.IsEnrolled) &&
            (string.IsNullOrWhiteSpace(search) || c.Title.ToLower().Contains(search)));

        _visibleCourses = new ObservableCollection<Course>(filtered);
        CoursesListBox.ItemsSource = _visibleCourses;
    }

    private void CoursesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_selectedCourse != null)
        {
            _selectedCourse.CourseCompleted -= OnCourseCompletedHandler;
        }

        _selectedCourse = CoursesListBox.SelectedItem as Course;

        if (_selectedCourse != null)
        {
            _selectedCourse.CourseCompleted += OnCourseCompletedHandler;
        }

        ShowCourseDetails();
    }

    private void OnCourseCompletedHandler(string courseTitle)
    {
        MessageBox.Show($"Вітаємо! Ви успішно завершили курс: {courseTitle}", "Курс пройдено", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void ShowCourseDetails()
    {
        if (_selectedCourse == null)
        {
            CourseTitleTextBlock.Text = "Оберіть курс";
            CourseInfoTextBlock.Text = string.Empty;
            CourseDescriptionTextBlock.Text = string.Empty;
            ModulesListBox.ItemsSource = null;
            ProgressBar.Value = 0;
            ProgressTextBlock.Text = "0%";
            return;
        }

        CourseTitleTextBlock.Text = _selectedCourse.Title;
        CourseInfoTextBlock.Text = $"Категорія: {_selectedCourse.Category} | Викладач: {_selectedCourse.Teacher} | Тривалість: {_selectedCourse.DurationHours} год.";
        CourseDescriptionTextBlock.Text = _selectedCourse.Description;
        ModulesListBox.ItemsSource = _selectedCourse.Modules;
        ProgressBar.Value = _selectedCourse.Progress;
        ProgressTextBlock.Text = $"{_selectedCourse.Progress}%";
        EnrollButton.Content = _selectedCourse.IsEnrolled ? "Курс уже додано" : "Записатися на курс";
    }

    private void EnrollButton_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedCourse == null)
        {
            MessageBox.Show("Спочатку оберіть курс.", "Повідомлення", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        if (_data.CurrentRole == UserRole.Guest)
        {
            MessageBox.Show("Гість може переглядати курси, але для запису потрібно обрати роль зареєстрованого користувача.",
                "Обмеження доступу", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        _selectedCourse.IsEnrolled = true;
        ShowCourseDetails();
        MessageBox.Show("Ви успішно записалися на курс.", "Успішно", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void ModuleCheckBox_Changed(object sender, RoutedEventArgs e)
    {
        if (_selectedCourse == null) return;

        if (_data.CurrentRole == UserRole.Guest || !_selectedCourse.IsEnrolled)
        {
            MessageBox.Show("Проходити модулі може лише зареєстрований користувач, який записався на курс.",
                "Обмеження доступу", MessageBoxButton.OK, MessageBoxImage.Warning);
            ShowCourseDetails();
            return;
        }

        ShowCourseDetails();
    }

    private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e) => RefreshCourses();

    private void MyCoursesButton_Click(object sender, RoutedEventArgs e)
    {
        if (_data.CurrentRole == UserRole.Guest)
        {
            MessageBox.Show("У гостя немає власних курсів. Оберіть роль зареєстрованого користувача.",
                "Повідомлення", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }
        RefreshCourses(onlyMyCourses: true);
    }

    private void AllCoursesButton_Click(object sender, RoutedEventArgs e) => RefreshCourses();

    private void RoleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_data == null) return;
        _data.CurrentRole = RoleComboBox.SelectedIndex == 0 ? UserRole.Guest : UserRole.RegisteredUser;
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        _jsonService.Save(DataFilePath, _data);
        MessageBox.Show("Дані збережено у файл courses_data.json", "JSON", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void LoadButton_Click(object sender, RoutedEventArgs e)
    {
        _data = _jsonService.Load(DataFilePath);
        RoleComboBox.SelectedIndex = _data.CurrentRole == UserRole.Guest ? 0 : 1;
        RefreshCourses();
        ShowCourseDetails();
        MessageBox.Show("Дані завантажено з JSON-файлу.", "JSON", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}