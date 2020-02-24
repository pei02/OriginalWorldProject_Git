using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GoogleRecaptcha;
namespace OriginalWorldProject.App_Code
{
    public class Google_reCAPTCHA
    {
        public bool reCAPTCHA()
        {
            //IRecaptcha<RecaptchaV2Result> recaptcha = new RecaptchaV2(new RecaptchaV2Data() { Secret = "6Lc4DtYUAAAAANxLL8XBBKBm3rREs2FaUnq7Vdmh" });
            IRecaptcha<RecaptchaV2Result> recaptcha = new RecaptchaV2(new RecaptchaV2Data() { Secret = "6Lf9vdoUAAAAAINqpsMnNngM3eRx1gl4kereiNcE" });
            var result = recaptcha.Verify();
            Boolean verification = result.Success ? true : false;
            if (verification == true) { return true; }
            return false;

        }
    }
}