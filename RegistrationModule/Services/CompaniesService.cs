using RegistrationModule.DAO;
using RegistrationModule.Models;

namespace RegistrationModule.Services
{
    public class CompaniesService
    {
        private readonly CompanyDAO companyDAO;

        public CompaniesService()
        {
            companyDAO = new CompanyDAO();
        }

        public Company GetCurrentCompany() => companyDAO.GetCurrentCompany();

        public bool CheckDevicePermitted() => GetCurrentCompany() != null;
    }
}
