namespace FlutterApi.Models.Users
{
    public class UpdateUserRequest
    {
        public string? name { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
