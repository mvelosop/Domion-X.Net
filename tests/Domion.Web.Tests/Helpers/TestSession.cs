using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

//-----------------------------------------------------------------------------------------------------------------------------------------------------------
// Taken from: https://github.com/aspnet/Mvc/blob/rel/1.1.3/test/Microsoft.AspNetCore.Mvc.ViewFeatures.Test/ViewFeatures/SessionStateTempDataProviderTest.cs
//-----------------------------------------------------------------------------------------------------------------------------------------------------------

namespace Domion.Web.Tests.Helpers
{
    public class TestSession : ISession
    {
        private Dictionary<string, byte[]> _innerDictionary = new Dictionary<string, byte[]>();

        public string Id => "TestId";

        public bool IsAvailable { get; } = true;

        public IEnumerable<string> Keys { get { return _innerDictionary.Keys; } }

        public void Clear()
        {
            _innerDictionary.Clear();
        }

        public Task CommitAsync()
        {
            return Task.FromResult(0);
        }

        public Task LoadAsync()
        {
            return Task.FromResult(0);
        }

        public void Remove(string key)
        {
            _innerDictionary.Remove(key);
        }

        public void Set(string key, byte[] value)
        {
            _innerDictionary[key] = value.ToArray();
        }

        public bool TryGetValue(string key, out byte[] value)
        {
            return _innerDictionary.TryGetValue(key, out value);
        }
    }
}
