using apiServicio.Models;
using System.Threading.Tasks;

namespace apiServicio.Business.Contracts
{
    public interface IUsuarioRepository
    {
        Task<Usuario> GetNombreUsuario(string nombreusuario);
    }
}
