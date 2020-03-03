using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OriginalWorldProject.Models;
using PagedList;


namespace OriginalWorldProject.ViewModel
{
    public class WritterClass
    {
        public List<Writer_application> writer_Applications { get; set; }
        public List<Writer> writers { get; set; }
        public IPagedList<Writer> WritersPageList { get; set; }
    }
}