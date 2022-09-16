using System;
using System.Text.RegularExpressions;

namespace CardBox.CompanyManagement.ViewModels.Validators;

internal static class EIKValidator
{
    #region Declarations

    /// <summary>
    /// Шаблон за първична регекс валидация
    /// </summary>
    private const string pattern = "^[0-9]{9,9}|[0-9]{13,13}$";

    /// <summary>
    /// Тегловни коефициенти
    /// </summary>
    private static readonly int[] FIRST_SUM_9DIGIT_WEIGHTS = { 1, 2, 3, 4, 5, 6, 7, 8 };
    private static readonly int[] SECOND_SUM_9DIGIT_WEIGHTS = { 3, 4, 5, 6, 7, 8, 9, 10 };
    private static readonly int[] FIRST_SUM_13DIGIT_WEIGHTS = { 2, 7, 3, 5 };
    private static readonly int[] SECOND_SUM_13DIGIT_WEIGHTS = { 4, 9, 5, 7 };

    #endregion Declarations

    #region Methods

    /// <summary>
    /// Валидира подадения булстат
    /// </summary>
    /// <param name="bulstat"> булстат </param>
    /// <returns> дали е валиден </returns>
    public static bool Validate(string bulstat)
    {
        if (bulstat == null || !Regex.IsMatch(bulstat, pattern))
        {
            return false;
        }

        if (bulstat.Length == 9)
        {
            return Int32.Parse(bulstat.Substring(8, 1)) == CalculateNinthDigitInEIK(bulstat);
        }

        if (bulstat.Length == 13)
        {
            return Int32.Parse(bulstat.Substring(12, 1)) == CalculateThirteenthDigitInEIK(bulstat);
        }

        return false;
    }

    /// <summary>
    /// Изчислява чек сумата на девет цифров булстат
    /// </summary>
    /// <param name="bulstat"> </param>
    /// <returns> </returns>
    private static int CalculateNinthDigitInEIK(string bulstat)
    {
        int firstSum = 0;
        int secondSum = 0;
        for (int i = 0; i < 8; i++)
        {
            firstSum += Int32.Parse(bulstat.Substring(i, 1)) * FIRST_SUM_9DIGIT_WEIGHTS[i];
            secondSum += Int32.Parse(bulstat.Substring(i, 1)) * SECOND_SUM_9DIGIT_WEIGHTS[i];
        }

        int controlDigit = firstSum % 11;
        if (controlDigit == 10)
        {
            controlDigit = secondSum % 11;
            if (controlDigit == 10)
            {
                return 0;
            }
        }

        return controlDigit;
    }

    /// <summary>
    /// Изчислява чек сумата на тринайсет цифров булстат
    /// </summary>
    /// <param name="bulstat"> булстат </param>
    /// <returns> чек сума </returns>
    private static int CalculateThirteenthDigitInEIK(string bulstat)
    {
        int ninthDigit = CalculateNinthDigitInEIK(bulstat);
        if (ninthDigit != Int32.Parse(bulstat.Substring(8, 1)))
        {
            return -1;
        }

        int firstSum = 0;
        int secondSum = 0;
        for (int i = 8, j = 0; j < 4; i++, j++)
        {
            firstSum += Int32.Parse(bulstat.Substring(i, 1)) * FIRST_SUM_13DIGIT_WEIGHTS[j];
            secondSum += Int32.Parse(bulstat.Substring(i, 1)) * SECOND_SUM_13DIGIT_WEIGHTS[j];
        }

        int controlDigit = firstSum % 11;
        if (controlDigit == 10)
        {
            controlDigit = secondSum % 11;
            if (controlDigit == 10)
            {
                return 0;
            }
        }

        return controlDigit;
    }

    #endregion Methods
}