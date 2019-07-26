using System.Collections.Generic;
using System.IO;
using System;
using ChoETL;
using System.Text;
using System.Linq;
using System.Collections;

namespace EmployeesLibrary
{
    public static class Utils
    {
        /// <summary>
        /// https://github.com/Cinchoo/ChoETL
        /// </summary>
        /// <param name="csvData"></param>
        /// <returns></returns>
        public static (bool, List<EmployeeModel>) IsCSVDataValid(string csvData)
        {
            try
            {
                var reader = new ChoCSVReader<EmployeeModel>(new StringBuilder(csvData)).WithFirstLineHeader(true);

                return (true, reader?.ToList());
            }
            catch (Exception)
            {
                return (false, null);
            }
        }

        public static bool HasValue(this string value) => !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value);
    }
}
