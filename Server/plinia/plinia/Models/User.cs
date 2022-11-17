namespace plinia.Models
{
    public class User
    {
        internal string? uuid;

        public int id { get; set; }
        public string? name { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
        public bool mailConfirmed { get; internal set; }
    }
}
