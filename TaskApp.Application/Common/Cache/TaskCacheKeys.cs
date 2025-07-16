namespace TaskApp.Application.Common.Cache;
public static class TaskCacheKeys
{
    public static string ById(Guid id) => $"task:{id}";
}