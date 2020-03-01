using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OriginalWorldProject.Models;
using PagedList;
namespace OriginalWorldProject.ViewModel
{
    public class Writer_applicationClass
    {
        public List<Writer_application> Writer_Applications { get; set; }
        public List<Administrator> Administrators { get; set; }
        public List<Member> Members { get; set; }
        public List<Approval_status> Approval_Statuses { get; set; }
        public IPagedList<Writer_application> Writer_applicationPageList { get; set; }

    }
}