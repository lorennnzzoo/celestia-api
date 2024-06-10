using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace celestia_api.Models.Returns_Reponses
{
    public class ThreadCreationClass
    {        
        public byte[] imagebytes { get; set; }
        public int thread_id { get; set; }       
        public Nullable<int> board_id { get; set; }        
        public string thread_caption { get; set; }
        public string content { get; set; }      
        
        public string imagefilename { get; set; }
    }

    public class ChildThreadCreationClass
    {
        public int parent_thread { get; set; }
        public string thread_caption { get; set; }
        public string content { get; set; }        
        public string image_file_name { get; set; }
        public byte[] imagebytes { get; set; }

    }
}