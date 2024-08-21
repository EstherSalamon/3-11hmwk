using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordPics.data
{
    public class Image
    {
        public int ID { get; set; }
        public string ImageTitle { get; set; }
        public string Password { get; set; }
        public string ImagePath { get; set; }
        public int Views { get; set; }
    }
}
