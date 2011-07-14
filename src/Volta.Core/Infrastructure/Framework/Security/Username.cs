namespace Volta.Core.Infrastructure.Framework.Security
{
    public class Username
    {
        private readonly string _username;

        public Username(string username)
        {
            if (username == null) return;
            _username = username.ToLower();
        }

        public bool IsEmpty { get { return string.IsNullOrEmpty(_username); } }

        public static implicit operator Username(string value) { return new Username(value); }
        public static implicit operator string(Username value) { return value._username; }
        public static bool operator ==(Username a, string b) { return Equals(a, b); }
        public static bool operator !=(Username a, string b) { return !Equals(a, b); }
        public static bool operator ==(string a, Username b) { return Equals(b, a); }
        public static bool operator !=(string a, Username b) { return !Equals(b, a); }
        public static bool operator ==(Username a, Username b) { return Equals(a, b); }
        public static bool operator !=(Username a, Username b) { return !Equals(a, b); }

        public override string ToString()
        {
            return _username;
        }

        public override bool Equals(object obj)
        {
            return obj is Username && Equals(this, (Username)obj);
        }

        public override int GetHashCode()
        {
            return _username != null ? _username.GetHashCode() : 0;
        }

        private static bool Equals(Username a, Username b)
        {
            return ((object)a == null && (object)b == null) ||
                   ((object)a != null && (object)b != null && 
                    a._username == b._username);
        }
    }
}