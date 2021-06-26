using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Git.Data.Models;

namespace Git.Data.Models
{
    /*•	Has an Id – a string, Primary Key
•	Has a Name – a string with min length 3 and max length 10 (required)
•	Has a CreatedOn – a datetime (required)
•	Has a IsPublic – bool (required)
•	Has a OwnerId – a string
•	Has a Owner – a User object
•	Has Commits collection – a Commit type
*/
    public class Repository
    {
        [Key]
        [Required]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(10)]
        public string Name { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public bool IsPublic { get; set; }

        public string OwnerId { get; set; }
        public User Owner { get; set; }

        public ICollection<Commit> Commits { get; set; } = new List<Commit>();
    }
}
