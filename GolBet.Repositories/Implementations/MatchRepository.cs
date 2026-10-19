using GolBet.Entities;
using GolBet.Entities.Enums;
using GolBet.Repositories.Data;
using GolBet.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolBet.Repositories.Implementations
{
    public class MatchRepository : GenericRepository<Match>, IMatchRepository
    {
        public MatchRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Match>> GetAllWithTeamsAsync(MatchStatus? status = null, string? search = null)
        {
            var query = _dbSet
                .Include(m => m.HomeTeam) //Include viene siendo similar a un JOIN en SQL
                .Include(m => m.AwayTeam)
                .Where(m => m.IsActive)
                .AsNoTracking()
                .AsQueryable(); //Para aplicar filtros dinámicos, ya que el Include devuelve un IQueryable y no un IEnumarable

            if (status.HasValue)
                query = query.Where(m => m.Status == status.Value);

            //BÚSQUEDA
            if (!string.IsNullOrWhiteSpace(search))
            {
                var mySearch = search.Trim();
                query = query.Where(m => m.HomeTeam.Name.Contains(mySearch)
                                      || m.AwayTeam.Name.Contains(mySearch));
            }

            return await query.OrderBy(m => m.Date).ToListAsync();
        }

        public async Task<Match?> GetByIdWithDetailsAsync(int id)
            => await _dbSet
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .Include(m => m.Bets)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
    }

}
