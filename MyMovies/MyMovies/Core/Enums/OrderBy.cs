using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMovies.Core.Enums
{
    public enum OrderBy
    {
        [Display(Name = "Ascending")]
        Asc,
        [Display(Name = "Descending")]
        Desc
    }
}
