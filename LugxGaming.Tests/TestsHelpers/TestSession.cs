using Microsoft.AspNetCore.Http;

namespace LugxGaming.Tests.TestsHelpers
{
    // Helper class for Session handling in tests
    public class TestSession : ISession
    {
        private readonly Dictionary<string, byte[]> _sessionStorage = new Dictionary<string, byte[]>();

        public IEnumerable<string> Keys => _sessionStorage.Keys;

        public string Id => Guid.NewGuid().ToString();

        public bool IsAvailable => true;

        public void Clear() => _sessionStorage.Clear();

        public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        public void Remove(string key) => _sessionStorage.Remove(key);

        public void Set(string key, byte[] value) => _sessionStorage[key] = value;

        public bool TryGetValue(string key, out byte[] value) => _sessionStorage.TryGetValue(key, out value);
    }

    public static class SessionExtensions
    {
        public static void SetString(this ISession session, string key, string value)
        {
            session.Set(key, System.Text.Encoding.UTF8.GetBytes(value));
        }

        public static string GetString(this ISession session, string key)
        {
            session.TryGetValue(key, out var data);
            return data == null ? null : System.Text.Encoding.UTF8.GetString(data);
        }

        public static void SetJson<T>(this ISession session, string key, T value)
        {
            var jsonData = System.Text.Json.JsonSerializer.Serialize(value);
            session.SetString(key, jsonData);
        }

        public static T GetJson<T>(this ISession session, string key)
        {
            var jsonData = session.GetString(key);
            return jsonData == null ? default : System.Text.Json.JsonSerializer.Deserialize<T>(jsonData);
        }
    }
}
