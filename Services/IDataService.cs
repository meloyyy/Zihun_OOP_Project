using Zihun_OOP_Project_2026.Models;

namespace Zihun_OOP_Project_2026.Services;

public interface IDataService
{
    AppData Load(string path);
    void Save(string path, AppData data);
}