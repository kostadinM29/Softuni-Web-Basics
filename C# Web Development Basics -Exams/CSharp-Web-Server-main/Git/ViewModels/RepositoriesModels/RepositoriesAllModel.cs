using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git.ViewModels.RepositoriesModels
{
    /*<th scope="row">ASP.NET Core Template</th>
                                <td>Nikolay Kostov</td>
                                <td>22/10/2018 08:00</td>
                                <td>254</td>*/
    public class RepositoriesAllModel
    {
        public string Name { get; set; }

        public string Owner { get; set; }

        public string CreatedOn { get; set; }

        public int CommitCount { get; set; }

        public string Id { get; set; }
    }
}
