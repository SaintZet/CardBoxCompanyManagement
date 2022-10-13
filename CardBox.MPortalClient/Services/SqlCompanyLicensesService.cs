using CardBox.MPortalDataBaseClient.Constants;
using CardBox.MPortalDataBaseClient.Contracts;
using CardBox.MPortalDataBaseClient.Models;
using System.Data.SqlClient;

namespace CardBox.MPortalDataBaseClient.Services;

public class SqlCompanyLicensesService : ICompanyLicensesService
{
    public Dictionary<string, List<CompanyLicense>> GetCompaniesLicense(List<string> companiesID)
    {
        Dictionary<string, List<CompanyLicense>> result = new();

        using (var mportalDbCon = new SqlConnection(ConnectionStrings.PMportalDB))
        {
            using (var donglesDbCon = new SqlConnection(ConnectionStrings.DonglesDB))
            {
                mportalDbCon.Open();
                donglesDbCon.Open();

                using (var mportalDbCom = new SqlCommand(Queries.PMportalDB, mportalDbCon))
                {
                    using var donglesDbCom = new SqlCommand(Queries.DonglesDB, donglesDbCon);

                    foreach (var id in companiesID)
                    {
                        List<CompanyLicense> companyLicenses = new();
                        string userUniqueNumber;
                        string serialNumber;

                        mportalDbCom.Parameters.AddWithValue("@TaxId", id);

                        using var reader = mportalDbCom.ExecuteReader();
                        {
                            mportalDbCom.Parameters.Clear();

                            while (reader.Read())
                            {
                                userUniqueNumber = reader["UserUniqueNumber"].ToString()!;
                                serialNumber = reader["SerialNumber"].ToString()!;

                                if (string.IsNullOrEmpty(serialNumber))
                                {
                                    break;
                                }

                                donglesDbCom.Parameters.AddWithValue("@DongleID", serialNumber);
                                object dateTime = donglesDbCom.ExecuteScalar();
                                donglesDbCom.Parameters.Clear();

                                companyLicenses.Add(new()
                                {
                                    UserUniqueNumber = userUniqueNumber,
                                    SerialNumber = serialNumber,
                                    Date = dateTime == null ? string.Empty : ((DateTime)dateTime).ToString("dd.MM.yyyy"),
                                });
                            }
                        }

                        result.Add(id, companyLicenses);
                    }
                }
            }
        }

        return result;
    }
}