using System;
using System.Data;
using System.Data.Fakes;
using Microsoft.QualityTools.Testing.Fakes.Stubs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SalaryCalculator;
using SalaryCalculator.Fakes;
using System.Linq;

namespace SalaryCalculatorTests
{
    [TestClass]
    public class CalculatorRepositoryTests
    {
        [TestMethod]
        public void DataManagerLazyLoads()
        {
            var repo = new CalculatorRepository();
            Assert.IsNotNull(repo.DataManager);
            Assert.IsInstanceOfType(repo.DataManager, typeof(DataManager));
        }

        [TestMethod]
        public void CanGetEmployeeId()
        {
            var expectedId = Guid.NewGuid();
            var dataManager = new StubIDataManager();
            dataManager.ExecuteScalarStringIDbDataParameterArray = (text, parameters) =>
                {
                    return expectedId;
                };

            var repo = new CalculatorRepository
                {
                    DataManager = dataManager
                };

            var actualId = repo.GetEmployeeId("Jon", "Doe");

            Assert.AreEqual(actualId, expectedId);
        }

        [TestMethod]
        public void GetEmployeeIdThrowsCalculatorRepositoryExceptionWhenIdIsNull()
        {
            var dataManager = new StubIDataManager();
            dataManager.ExecuteScalarStringIDbDataParameterArray = (text, parameters) =>
            {
                return null;
            };

            var repo = new CalculatorRepository
            {
                DataManager = dataManager
            };

            AssertHelpers.Throws<CalculatorRepositoryException>(() => repo.GetEmployeeId("Jon", "Doe"));
        }

        [TestMethod]
        public void GetEmployeeIdThrowsCalculatorRepositoryExceptionWhenIdIsDBNull()
        {
            var dataManager = new StubIDataManager();
            dataManager.ExecuteScalarStringIDbDataParameterArray = (text, parameters) =>
            {
                return DBNull.Value;
            };

            var repo = new CalculatorRepository
            {
                DataManager = dataManager
            };

            AssertHelpers.Throws<CalculatorRepositoryException>(() => repo.GetEmployeeId("Jon", "Doe"));
        }
    
        [TestMethod]
        public void CanGetSalaryInformationForOneRecord()
        {
            #region Arrange
            DateTime salaryStart = DateTime.Now.AddYears(-1).AddDays(21);
            DateTime salaryEnd = DateTime.Now.AddDays(21);
            var dataManager = new StubIDataManager();
            dataManager.ExecuteReaderStringIDbDataParameterArray = (text, parameters) =>
                {
                    var reader = new StubIDataReader();
                    reader.GetOrdinalString = (fieldName) =>
                        {
                            switch (fieldName)
                            {
                                case "Amount": return 0;
                                case "BeginDate": return 1;
                                default: return 2;
                            }
                        };
                    //reader
                    
                    int readCounts = 0;

                    reader.Read = () =>
                        {
                            readCounts++;
                            return readCounts == 1;
                        };

                    reader.GetDecimalInt32 = (ndx) =>
                        {
                            return 12000.93m;
                        };

                    reader.GetDateTimeInt32 = (ndx) =>
                        {
                            if (ndx == 1)
                            {
                                return salaryStart;
                            }
                            else
                            {
                                return salaryEnd;
                            }
                        };
                    return reader;
                };

            var repo = new CalculatorRepository
                {
                    DataManager = dataManager
                };
            #endregion

            // Act
            decimal annualSalary = 0;
            var salaries = repo.GetSalaryInformation(Guid.NewGuid(), DateTime.Now.AddDays(-28), DateTime.Now.AddDays(-14), out annualSalary);

            // Assert
            Assert.IsNotNull(salaries);
            Assert.AreEqual(annualSalary, 12000.93m);
            Assert.AreEqual(salaries.Count(), 1);
            var salary = salaries.FirstOrDefault();
            Assert.IsNotNull(salary);
            Assert.AreEqual(salary.AnnualAmount, 12000.93m);
            Assert.AreEqual(salary.BeginDate, salaryStart);
            Assert.AreEqual(salary.EndDate, salaryEnd);
        }

        [TestMethod]
        public void CanGetSalaryInformationForTwoRecords()
        {
            #region Arrange
            DateTime firstSalaryStart = DateTime.Now.AddYears(-1).AddDays(-21);
            DateTime firstSalaryEnd = DateTime.Now.AddDays(-21);
            DateTime secondSalaryStart = DateTime.Now.AddDays(-20);
            DateTime secondSalaryEnd = DateTime.Now.AddDays(20);
            var dataManager = new StubIDataManager();
            dataManager.ExecuteReaderStringIDbDataParameterArray = (text, parameters) =>
            {
                var reader = new StubIDataReader();
                reader.GetOrdinalString = (fieldName) =>
                {
                    switch (fieldName)
                    {
                        case "Amount": return 0;
                        case "BeginDate": return 1;
                        default: return 2;
                    }
                };
                //reader

                int readCounts = 0;

                reader.Read = () =>
                {
                    readCounts++;
                    return readCounts <= 2;
                };

                reader.GetDecimalInt32 = (ndx) =>
                {
                    if (readCounts == 1)
                    {
                        return 12000.93m;
                    }
                    else
                    {
                        return 12254.02m;
                    }
                };

                reader.GetDateTimeInt32 = (ndx) =>
                {
                    if (readCounts == 1)
                    {
                        if (ndx == 1)
                        {
                            return firstSalaryStart;
                        }
                        else
                        {
                            return firstSalaryEnd;
                        }
                    }
                    else
                    {
                        if (ndx == 1)
                        {
                            return secondSalaryStart;
                        }
                        else
                        {
                            return secondSalaryEnd;
                        }
                    }
                };
                return reader;
            };

            var repo = new CalculatorRepository
            {
                DataManager = dataManager
            };
            #endregion

            // Act
            decimal annualSalary = 0;
            var salaries = repo.GetSalaryInformation(Guid.NewGuid(), DateTime.Now.AddDays(-28), DateTime.Now.AddDays(-14), out annualSalary);

            // Assert
            Assert.IsNotNull(salaries);
            Assert.AreEqual(annualSalary, 12254.02m);
            Assert.AreEqual(salaries.Count(), 2);
            var salary1 = salaries.FirstOrDefault(s => s.AnnualAmount == 12000.93m && s.BeginDate == firstSalaryStart && s.EndDate == firstSalaryEnd);
            var salary2 = salaries.FirstOrDefault(s => s.AnnualAmount == 12254.02m && s.BeginDate == secondSalaryStart && s.EndDate == secondSalaryEnd);
            Assert.IsNotNull(salary1);
            Assert.IsNotNull(salary2);
        }
    }
}