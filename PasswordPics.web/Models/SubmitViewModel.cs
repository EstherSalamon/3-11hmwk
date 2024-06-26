﻿using PasswordPics.data;

namespace PasswordPics.web.Models
{
    public class SubmitViewModel
    {
        public Image Image { get; set; }
        public int? CookieCount { get; set; }
        public int? SessionCount { get; set; }
        public int? StaticCount { get; set; }
        public bool AllowIn { get; set; }
    }
}
