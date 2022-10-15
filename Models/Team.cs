using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NBA_App.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="The name must not be empty")]
        [StringLength(30,MinimumLength =3, ErrorMessage = "The length of the name must be more than 3 characters")]
        public string? Name { get; set; }

        [Required(ErrorMessage ="The town must not be empty")]
        [StringLength(50,MinimumLength =3, ErrorMessage = "The length of the town must be more than 3 characters")]
        public string? Town { get; set; }

        [Required(ErrorMessage ="The coach must not be empty")]
        [StringLength(30,MinimumLength =3, ErrorMessage = "The length of the coach must be more than 3 characters")]
        public string? Coach { get; set; }
    }
}