﻿namespace CardBox.MPortalDataBaseClient.Constants
{
    internal static class Queries
    {
        public static string PMportal => @"SELECT SerialNumber, UserUniqueNumber FROM [dbo].Dongles d LEFT JOIN[Identity].[Users] u ON d.UserUniqueNumber = u.UniqueNumber WHERE u.TaxId = @TaxId";

        public static string Dongles => @"SELECT [ExpirationDate] FROM [Dongles].[dbo].[ProductsExpiration] WHERE ProductID = 6 AND DongleID = @DongleID";
    }
}