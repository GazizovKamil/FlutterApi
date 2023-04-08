﻿using System.ComponentModel.DataAnnotations.Schema;

namespace FlutterApi.Models.Users
{
    public class AddUserRequest
    {
        public string? name { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
    }
}
