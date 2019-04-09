using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DDDSample.Entity
{
    [Table("user")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [StringLength(50, MinimumLength = 3)]
        [Required]
        public String NickName { get; set; }

        [StringLength(50, MinimumLength = 3)]
        [Required]
        public String MyId { get; set; }

        [StringLength(50, MinimumLength = 3)]
        [Required]
        public String PassWord { get; set; }

        public DateTime RegDate { get; set; }


    }
}
