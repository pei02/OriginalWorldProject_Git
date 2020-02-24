using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace OriginalWorldProject.Models
{
    public class MetadataAdministrator
    {
        [DisplayName("管理員編號")]
        [Key]
        [RegularExpression("[A][0-9]{3}", ErrorMessage = "格式錯誤")]
        public string Administrator_ID { get; set; }

        [DisplayName("管理員名稱")]
        [StringLength(25, ErrorMessage = "管理員名稱不得超過25個字")]
        [Required(ErrorMessage = "請輸入名稱")]
        [Remote("check_adm_name", "Administrators", ErrorMessage = "名稱已被使用")]
        [CheckAdmName]
        public string Adm_name { get; set; }

        [DisplayName("管理員帳號")]
        [RegularExpression("[a-zA-Z0-9]{8,20}", ErrorMessage = "請輸入8-20字(含)以內字元，內容不可包含特殊符號。")]
        [Required(ErrorMessage = "請輸入帳號")]
        [Remote("check_adm_account", "Administrators", ErrorMessage = "帳號已被使用")]
        [CheckAdmAccount]
        public string Account { get; set; }

        [DisplayName("管理員密碼")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z])(?=.*\W).{8,20}$", ErrorMessage = "密碼不符合強度規則!!請設定一組8-20字元的密碼，至少需包含一個數字、一個大寫或小寫字母、一個特殊符號。例:aIGSD789@@")]
        [Required(ErrorMessage = "請輸入密碼")]
        public string A_Password { get; set; }

        [DisplayName("管理員權限")]
        public bool Authority { get; set; }

        [DisplayName("確認密碼")]
        [Required(ErrorMessage = "請輸入密碼")]
        [System.Web.Mvc.Compare("A_Password", ErrorMessage = "密碼不相符，請再次輸入!!")]
        public string Confirm_pwd { get; set; }
    }
    public class MetadataAnnouncement
    {
        [Key]
        [DisplayName("公告編號")]
        public int Announcement_ID { get; set; }

        [DisplayName("公告標題")]
        [Required(ErrorMessage = "請輸入標題名稱!!")]
        [StringLength(50, ErrorMessage = "標題不得超過50字!!")]
        public string Announcement_title { get; set; }

        [DisplayName("公告內容")]
        [Required(ErrorMessage = "內容不得超過3000字!!")]
        public string Announcement_content { get; set; }

        [DisplayName("公告時間")]
        public System.DateTime Announcement_time { get; set; }

        [DisplayName("公告類別")]
        [Required(ErrorMessage = "請選擇類別!!")]
        public string Announcement_type_ID { get; set; }

        [DisplayName("管理員編號")]
        [Required(ErrorMessage = "請輸入管理員編號!!")]
        public string Administrator_ID { get; set; }

        [DisplayName("公告顯示狀態")]
        public bool Ann_Display { get; set; }
    }

    public class MetadataMember
    {
        [DisplayName("會員編號")]
        [Key]
        [RegularExpression("[M][0-9]{9}", ErrorMessage = "格式錯誤")]
        public int MemberID { get; set; }

        [DisplayName("暱稱")]
        [Required(ErrorMessage = "請輸入暱稱!!")]
        [StringLength(25, ErrorMessage = "暱稱不得超過25字!!")]
        [Remote("check_Nickname", "Members", ErrorMessage = "暱稱已被使用")]
        [CheckNickname]
        public string Nickname { get; set; }

        [DisplayName("帳號")]
        [RegularExpression("[a-zA-Z0-9]{8,20}", ErrorMessage = "請輸入8-20字(含)以內字元，內容不可包含特殊符號。")]
        [Required(ErrorMessage = "請輸入帳號!!")]
        [Remote("check_M_account", "Members", ErrorMessage = "帳號已被使用")]
        [CheckM_Account]
        public string Account { get; set; }

        [DisplayName("密碼")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z])(?=.*\W).{8,20}$", ErrorMessage = "密碼不符合強度規則!!請設定一組8-20字元的密碼，至少需包含一個數字、一個大寫或小寫字母、一個特殊符號。例:aIGSD789@@")]
        [Required(ErrorMessage = "請輸入密碼!!")]
        public string M_Password { get; set; }

        [DisplayName("電話")]
        [Phone(ErrorMessage ="電話格式錯誤,請輸入正確電話格式!! 範例:0912345678")]
        [Required(ErrorMessage = "請輸入電話!!")]
        public string Phone { get; set; }

        [DisplayName("信箱")]
        [EmailAddress(ErrorMessage ="格式錯誤,請輸入正確信箱格式!! 範例:abc123@gmail.com")]
        [Required(ErrorMessage = "請輸入信箱!!")]
        public string Email { get; set; }

        [DisplayName("性別")]
        public bool Gender { get; set; }

        [DisplayName("生日")]
        [Required(ErrorMessage = "請輸入生日!!")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public System.DateTime Birthday { get; set; }

        [DisplayName("會員狀態")]
        public bool M_status { get; set; }

        [DisplayName("作家權限")]
        public bool Writter_qualifications { get; set; }

        [DisplayName("確認密碼")]
        [Required(ErrorMessage = "請輸入密碼")]
        [System.Web.Mvc.Compare("M_Password", ErrorMessage = "密碼不相符，請再次輸入!!")]
        public string Confirm_pwd { get; set; }
    }

    public class CheckAdmName : ValidationAttribute
    {
        OriginalWorldEntities db = new OriginalWorldEntities();

        public CheckAdmName()
        {
            ErrorMessage = "名稱已被使用";
        }

        public override bool IsValid(object value)
        {
            var result = db.Administrator.Where(c => c.Adm_name == value.ToString()).FirstOrDefault();
            if (result == null)
                return true;

            return false;
        }

    }
    public class CheckAdmAccount : ValidationAttribute
    {
        OriginalWorldEntities db = new OriginalWorldEntities();

        public CheckAdmAccount()
        {
            ErrorMessage = "帳號已被使用";
        }

        public override bool IsValid(object value)
        {
            var result = db.Administrator.Where(c => c.Account == value.ToString()).FirstOrDefault();
            if (result == null)
                return true;

            return false;
        }

    }

    public class CheckNickname : ValidationAttribute
    {
        OriginalWorldEntities db = new OriginalWorldEntities();

        public CheckNickname()
        {
            ErrorMessage = "暱稱已被使用";
        }

        public override bool IsValid(object value)
        {
            var result = db.Member.Where(c => c.Nickname == value.ToString()).FirstOrDefault();
            if (result == null)
                return true;

            return false;
        }

    }
    public class CheckM_Account : ValidationAttribute
    {
        OriginalWorldEntities db = new OriginalWorldEntities();

        public CheckM_Account()
        {
            ErrorMessage = "帳號已被使用";
        }

        public override bool IsValid(object value)
        {
            var result = db.Member.Where(c => c.Account == value.ToString()).FirstOrDefault();
            if (result == null)
                return true;

            return false;
        }

    }
}