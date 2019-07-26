using System.Collections.Generic;
using System;
using ChoETL;
using System.Text;
using System.Linq;

namespace EmployeesLibrary
{
    public static class Utils
    {
        /// <summary>
        /// <list type="bullet">
        /// <item><see cref="Tuple{T1,T2}.Item1"/>: A boolean indicating whether the validation succeeded.</item>
        /// <item><see cref="Tuple{T1,T2}.Item2"/>: The list of employees.</item>
        /// </list>
        /// 
        /// https://github.com/Cinchoo/ChoETL
        /// </summary>
        /// <param name="csvData"></param>
        /// <returns>
        /// <list type="bullet">
        /// <item><see cref="Tuple{T1,T2}.Item1"/>: A boolean indicating whether the validation succeeded.</item>
        /// <item><see cref="Tuple{T1,T2}.Item2"/>: The list of employees.</item>
        /// </list>
        /// </returns>
        public static (bool, List<EmployeeModel>) ValidateEmployeesCsvData(string csvData)
        {
            try
            {
                var reader = new ChoCSVReader<EmployeeModel>(new StringBuilder(csvData)).WithFirstLineHeader(true);

                return (true, reader?.ToList());
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.InnerException().Message);
                return (false, null);
            }
        }

        /// <summary>
        /// Checks if string value is null or empty.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool HasValue(this string value) => !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value);

        /// <summary>
        /// Gets the inner most exception
        /// </summary>
        /// <param name="exception"></param>
        /// <returns>
        /// <see cref="Exception" />
        /// </returns>
        public static Exception InnerException(this Exception exception)
        {
            while (exception.InnerException != null) exception = exception.InnerException;
            return exception;
        }
    }
}
