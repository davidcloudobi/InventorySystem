using System.Threading.Tasks;
using Domain.DTO.Request;

namespace Domain.Interface
{
    public interface IInventoryProductService
    {

        Task<bool> Add(InventoryProductDTO request);
    }
}