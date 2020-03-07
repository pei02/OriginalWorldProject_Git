using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OriginalWorldProject.Models;
using PagedList;
namespace OriginalWorldProject.ViewModel
{
    public class WorksClass
    {
        public List<Works> Works { get; set; }
        public List<Writer> Writers { get; set; }
        public List<Work_status> Work_Statuses { get; set; }
        public IPagedList<Works> WorksPageList { get; set; }
    }
}