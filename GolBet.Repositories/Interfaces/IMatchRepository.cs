using GolBet.Entities;
using GolBet.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolBet.Repositories.Interfaces
{
    public interface IMatchRepository : IGenericRepository<Match>
    {
        //Se agrega un parámetro [Search] opcional para la búsqueda
        Task<IEnumerable<Match>> GetAllWithTeamsAsync(MatchStatus? status = null, string? search = null);
        Task<Match?> GetByIdWithDetailsAsync(int id);

    }

}
