using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vidly.Models
{
    public class MembershipType
    {
        public byte Id { get; set; }
        public short SignUpFee { get; set; }
        public byte DurationInMonths { get; set; }
        public byte Discount { get; set; }
        [Required]
        public string NameOfPackage { get; set; }
        [Required]
        public string PackageName { get; set; }

        public static readonly byte Unknown = 0;
        public static readonly byte PayAsUGo = 1;

    }
}