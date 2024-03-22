using ZleceniaAPI.Models;

namespace ZleceniaAPI.Services
{
    public interface IContractorService
    {
        PagedResult<ContractorDto> GetAllContractors(ContractorQuery? query);
        UserProfileDto GetContractorProfile(int contractorId);
        AreaOfWorkDto GetContractorAreaOfWork(int contractorId);
    }
}