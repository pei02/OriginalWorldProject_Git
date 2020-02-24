using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OriginalWorldProject.Models;
using PagedList;

namespace OriginalWorldProject.ViewModel
{
    public class AnnouncementClass
    {
        public List<Announcement> Announcements { get; set; }
        public List<Announcement_type> Announcement_Types { get; set; }
        public List<Administrator> Administrators { get; set; }
        public IPagedList<Announcement> AnnouncementsPageList { get; set; }
    }
}