using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMovies.Core.Enums
{
    public enum Genres
    {
        None,
        Action,
        Adventure,
        Animation,
        Biography,
        Comedy,
        Crime,
        Documentary,
        Drama,
        Family,
        Fantasy,
        [Display(Name = "Film Noir")]
        FilmNoir,
        [Display(Name = "Game Show")]
        GameShow,
        History,
        Horror,
        Music,
        Musical,
        Mystery,
        News,
        [Display(Name = "Reality TV")]
        RealityTV,
        Romance,
        [Display(Name = "Sci Fi")]
        SciFi,
        Sport,
        [Display(Name = "Talk Show")]
        TalkShow,
        Thriller,
        War,
        Western
    }
}
