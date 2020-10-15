using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MVCClient
{
    public abstract class TypedClient
    {
        private readonly HttpClient _client;

        public TypedClient(HttpClient client)
        {
            _client = client;
        }

        public virtual async Task<string> CallApi()
        {
            return await _client.GetStringAsync("test");
        }
    }

    public class TypedUserClient : TypedClient
    {
        public TypedUserClient(HttpClient client) : base(client)
        {
        }
    }

    public class TypedClientClient : TypedClient
    {
        public TypedClientClient(HttpClient client) : base(client)
        {
        }
    }
}
