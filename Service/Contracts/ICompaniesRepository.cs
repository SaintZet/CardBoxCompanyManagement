﻿using CardBoxCompanyManagement.Domain;

namespace CardBoxCompanyManagement.Infrastructure
{
    public interface ICompaniesRepository
    {
        public List<Company> GetAll();
    }
}