using System.Collections.Generic;

namespace CleanArchitecture.Requestor
{
    public sealed class RequestProperties
    {
        private readonly IDictionary<string, object> properties = new Dictionary<string, object>();
        
        public RequestProperties Set(string key, string value)
        {
            SetObject(key, value);
            
            return this;
        }

        public string GetString(string key)
        {
            return (string) GetObject(key);
        }

        public RequestProperties Set(string key, long value)
        {
            SetObject(key, value);

            return this;
        }

        public long GetInt(string key)
        {
            return (long) GetObject(key);
        }

        public RequestProperties Set(string key, bool value)
        {
            SetObject(key, value);

            return this;
        }

        public bool GetBoolean(string key)
        {
            return (bool) GetObject(key);
        }

        public RequestProperties Set(string key, double value)
        {
            SetObject(key, value);

            return this;
        }

        public double GetDouble(string key)
        {
            return (double) GetObject(key);
        }

        private void SetObject(string key, object value)
        {
            this.properties[key.ToUpperInvariant()] = value;
        }

        private object GetObject(string key)
        {
            return this.properties[key.ToUpperInvariant()];
        }
    }
}