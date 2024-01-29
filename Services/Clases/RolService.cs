using apiServicio.Business.Contracts;
using apiServicio.Models;
using apiServicio.Services.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace apiServicio.Services.Clases
{
    public class RolService : IRolService
    {
        private readonly IRolRepository _IRolRepository;

        public RolService(IRolRepository temp)
        {
            _IRolRepository = temp;
        }
        public Task<List<Rol>> GetList()
        {
            return _IRolRepository.GetList();
        }
        public Task<Rol> AgregaActualiza(Rol l, string t)
        {
            return _IRolRepository.AgregaActualiza(l, t);
        }
    }
}
