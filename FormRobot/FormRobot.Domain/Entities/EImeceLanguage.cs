using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EImece.Domain.Models.Enums
{
    public enum EImeceLanguage
    {
        [Description("tr-TR")]
        [Display(Name = "Türkçe")]
        Turkish = 1,
        [Description("en-US")]
        [Display(Name = "English")]
        English = 2,
        [Description("ru-RU")]
        [Display(Name = "Russian")]
        Russian = 3,
        [Description("de-DE")]
        [Display(Name = "German")]
        German = 4
    }
}
