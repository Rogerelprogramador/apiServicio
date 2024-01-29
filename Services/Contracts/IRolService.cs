using apiServicio.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace apiServicio.Services.Contracts
{
    public interface IRolService
    {
        Task<List<Rol>> GetList();
        Task<Rol> AgregaActualiza(Rol l, string t);
    }
}
