using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git.ViewModels.CommitsModels
{
    public class CommitsAllModel
    {
        public string RepositoryName { get; set; }

        public string Description { get; set; }

        public string CreatedOn { get; set; }

        public string Id { get; set; }
    }
}
