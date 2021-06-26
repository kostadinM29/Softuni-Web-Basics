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
•	Has a Description – a string with min length 5 (required)
•	Has a CreatedOn – a datetime (required)
•	Has a CreatorId – a string
•	Has Creator – a User object
•	Has RepositoryId – a string
•	Has Repository– a Repository object
*/
    public class Commit
    {
        [Key]
        [Required]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public string  CreatorId { get; set; }
        public User Creator { get; set; }

        public string RepositoryId { get; set; }

        public Repository Repository { get; set; }
    }
}
