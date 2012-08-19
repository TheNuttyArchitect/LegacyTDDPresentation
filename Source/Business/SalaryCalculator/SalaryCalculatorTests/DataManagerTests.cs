using System.Collections.Generic;
using System.Data;
using System.Data.Fakes;
using System.Data.SqlClient;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient.Fakes;
using SalaryCalculator;
using System.Linq;

namespace SalaryCalculatorTests
{
    [TestClass]
    public class DataManagerTests
    {
        private static DataManager _dataManager;
        private StubIDbCommand _command;
        private StubIDbConnection _connection;

        [ClassInitialize]
        public static void FixtureSetup(TestContext testContext)
        {
            _dataManager = new DataManager();
        }

        [ClassCleanup]
        public static void FixtureTeardown()
        {
            if(_dataManager != null)
            {
                _dataManager.Dispose();
            }
        }

        [TestInitialize]
        public void Setup()
        {
            
            _connection = new StubIDbConnection();
            _command = new StubIDbCommand();
            _command.ConnectionGet = () => _connection;

            _dataManager.Command = _command;
            _dataManager.Connection = _connection;
        }

        [TestMethod]
        public void CanLazyLoadConnection()
        {
            var dataManager = new DataManager();
            Assert.IsNotNull(dataManager.Connection);
            Assert.IsInstanceOfType(dataManager.Connection, typeof(SqlConnection));
        }

        [TestMethod]
        public void CanLazyLoadCommand()
        {
            var dataManager = new DataManager();
            Assert.IsNotNull(dataManager.Command);
            Assert.IsInstanceOfType(dataManager.Command, typeof(SqlCommand));
            Assert.IsNotNull(dataManager.Command.Connection);
            Assert.IsInstanceOfType(dataManager.Command.Connection, typeof(SqlConnection));
        }

        [TestMethod]
        public void CanInitCommandForExecution()
        {
            bool clearCalled = false;
            bool openCalled = false;
            var addedParams = new List<IDbDataParameter>();
            string commandText = null;
            var parametersColl = new StubIDataParameterCollection();
            _connection.StateGet = () => ConnectionState.Closed;
            _connection.Open = () => { openCalled = true; };
            _command.CommandTextSetString = (v) => { commandText = v; };
            _command.ParametersGet = () => parametersColl;
            parametersColl.Clear = () => { clearCalled = true; };
            parametersColl.AddObject = (p) =>
            {
                addedParams.Add((IDbDataParameter)p);
                return 1;
            };

            _dataManager.InitCommandForExecution
                (
                    "ExpectedCommandText",
                    new CalculatorParameter("parm1", DbType.AnsiString, "value1"),
                    new CalculatorParameter("parm2", DbType.AnsiString, "value2"),
                    new CalculatorParameter("parm3", DbType.AnsiString, "value3")
                );

            Assert.IsTrue(clearCalled);
            Assert.IsTrue(openCalled);
            Assert.IsNotNull(commandText);
            Assert.AreEqual(commandText, "ExpectedCommandText");
            Assert.AreEqual(addedParams.Count, 3);
            Assert.IsTrue(addedParams.Any(p => p.ParameterName == "parm1" && p.DbType == DbType.AnsiString && p.Value.ToString() == "value1"));
            Assert.IsTrue(addedParams.Any(p => p.ParameterName == "parm2" && p.DbType == DbType.AnsiString && p.Value.ToString() == "value2"));
            Assert.IsTrue(addedParams.Any(p => p.ParameterName == "parm3" && p.DbType == DbType.AnsiString && p.Value.ToString() == "value3"));
        }

        [TestMethod]
        public void InitCommandForExecutionDoesNotTryToReopenConnectionIfItsAlreadyOpen()
        {
            bool openCalled = false;
            var parametersColl = new StubIDataParameterCollection();
            _connection.StateGet = () => ConnectionState.Open;
            _connection.Open = () => { openCalled = true; };
            _command.CommandTextSetString = (v) => { };
            _command.ParametersGet = () => parametersColl;
            parametersColl.Clear = () => { };
            parametersColl.AddObject = (p) =>
            {
                return 1;
            };

            _dataManager.InitCommandForExecution
                (
                    "ExpectedCommandText",
                    new CalculatorParameter("parm1", DbType.AnsiString, "value1"),
                    new CalculatorParameter("parm2", DbType.AnsiString, "value2"),
                    new CalculatorParameter("parm3", DbType.AnsiString, "value3")
                );

            Assert.IsFalse(openCalled);
        }

        [TestMethod]
        public void CanExecuteScalar()
        {
            bool executeCalled = false;
            var parametersColl = new StubIDataParameterCollection();
            _connection.StateGet = () => ConnectionState.Open;
            _connection.Open = () => { };
            _command.CommandTextSetString = (v) => { };
            _command.ParametersGet = () => parametersColl;
            parametersColl.Clear = () => { };
            parametersColl.AddObject = (p) => 1;
            _command.ExecuteScalar = () =>
                {
                    executeCalled = true;
                    return 1;
                };
            
            var result = _dataManager.ExecuteScalar("commandText");

            Assert.IsTrue(executeCalled);
            Assert.AreEqual(result, 1);
        }

        [TestMethod]
        public void CanExecuteNonQuery()
        {
            bool executeCalled = false;
            var parametersColl = new StubIDataParameterCollection();
            _connection.StateGet = () => ConnectionState.Open;
            _connection.Open = () => { };
            _command.CommandTextSetString = (v) => { };
            _command.ParametersGet = () => parametersColl;
            parametersColl.Clear = () => { };
            parametersColl.AddObject = (p) => 1;
            _command.ExecuteNonQuery = () =>
                {
                    executeCalled = true;
                    return 1;
                };

            _dataManager.ExecuteNonQuery("commandText");

            Assert.IsTrue(executeCalled);           
        }

        [TestMethod]
        public void CanExecuteReader()
        {
            bool executeCalled = false;
            var reader = new StubIDataReader();
            var parametersColl = new StubIDataParameterCollection();
            _connection.StateGet = () => ConnectionState.Open;
            _connection.Open = () => { };
            _command.CommandTextSetString = (v) => { };
            _command.ParametersGet = () => parametersColl;
            parametersColl.Clear = () => { };
            parametersColl.AddObject = (p) => 1;
            _command.ExecuteReader = () =>
            {
                executeCalled = true;
                return reader;
            };

            var result = _dataManager.ExecuteReader("commandText");

            Assert.IsTrue(executeCalled);    
            Assert.IsNotNull(result);
            Assert.AreSame(result, reader);
        }
    }
}