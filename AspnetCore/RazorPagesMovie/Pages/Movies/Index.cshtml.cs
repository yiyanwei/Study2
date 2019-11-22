using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Models;

namespace RazorPagesMovie.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly RazorPagesMovie.Models.RazorPagesMovieContext _context;

        public IndexModel(RazorPagesMovie.Models.RazorPagesMovieContext context)
        {
            _context = context;
        }

        public IList<Movie> Movies { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        public string MovieGenre { get; set; }

        public SelectList Genres { get; set; }
        public async Task OnGetAsync()
        {
            //首先查询出所有的流派，使用linq延迟查询
            var selectGenres = from g in _context.Movies
                               group g by g.Genre into genres
                               orderby genres.Key descending
                               select genres.Key;

            //linq 获取movies
            var movies = from m in _context.Movies
                         select m;

            //判断searchstring是否为空，不为空的则要进行过滤
            if (!string.IsNullOrEmpty(this.SearchString))
            {
                movies = movies.Where(m=>m.Title.Contains(this.SearchString.Trim()));
            }
            //判断是否有选择流派
            if(!string.IsNullOrEmpty(this.MovieGenre))
            {
                movies = movies.Where(m=>m.Genre == this.MovieGenre);
            }
            //流派
            Genres = new SelectList(await selectGenres.ToListAsync());
            Movies = await movies.ToListAsync();
        }
    }
}
