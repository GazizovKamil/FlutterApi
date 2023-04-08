using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlutterApi.Data
{
    public class User
    {
        public int Id { get; set; }
        public string? name { get; set; }
        public string? email { get; set; }   
        public string? password { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
