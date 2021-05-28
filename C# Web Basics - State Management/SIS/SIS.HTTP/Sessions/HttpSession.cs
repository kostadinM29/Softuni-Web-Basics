using System.Collections.Generic;
using System.Linq;

namespace SIS.HTTP.Sessions
{
    public class HttpSession : IHttpSession
    {
        private Dictionary<string, object> parameters;
        public HttpSession(string id)
        {
            this.Id = id;
            this.parameters = new Dictionary<string, object>();
        }
        public string Id { get; }

        public object GetParameter(string name)
        {
            return parameters.FirstOrDefault(x => x.Key == name);
        }

        public bool ContainsParameter(string name)
        {
            return parameters.Any(x => x.Key == name);
        }

        public void AddParameter(string name, object parameter)
        {
            parameters.Add(name, parameter);
        }

        public void ClearParameters()
        {
            parameters.Clear();
        }
    }
}