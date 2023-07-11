using ZleceniaAPI.Models;

namespace ZleceniaAPI.Services
{
    public interface IContractorService
    {
        PagedResult<ContractorDto> GetAllContractors(ContractorQuery? query);
    }
}