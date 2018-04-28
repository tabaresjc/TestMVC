using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JCTest.Models
{
    [Table("app_application_settings")]
    public class ApplicationSetting
    {
        [Key]
        public int Id { get; set; }

        [Index("IX_NAME", IsClustered = true, IsUnique = true)]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Value { get; set; }
    }
}