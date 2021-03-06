﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Leox.TranxManager
{
    internal sealed class Connectionx : IDisposable
    {
        private static readonly string _connectionString = System.Configuration.ConfigurationManager.AppSettings["DbConnectionString"];
        public string Id { get; set; }
        public DateTime CreateTime { get; set; }
        private SqlConnection _connection { get; set; }
        private SqlCommand _command { get; set; }
        private SqlTransaction _transaction { get; set; }

        public SqlCommand SqlCommand
        {
            get { return _command; }
        }

        public  IsolationLevel IsolationLevel{ get; set; }

        //public Connectionx() { }
        public Connectionx(string id, IsolationLevel isolationLevel)
        {
            this.Id = id;
            IsolationLevel = isolationLevel;
            _connection = new SqlConnection(_connectionString);
            _command = new SqlCommand(string.Empty, _connection);
        }

        public bool BeginTransction()
        {
            try
            {
                //默认为15秒
                //_connection.ConnectionTimeout = 15;
                _connection.Open();
                _transaction = _connection.BeginTransaction(IsolationLevel, Id.ToString());
                _command.Transaction = _transaction;
            }
            catch (Exception)
            {
                throw;
            }
          
            return true;
        }

        public void Commit()
        {
            if (_transaction != null)
                _transaction.Commit();

            Finally();
        }

        public void RollBack()
        {
            if (_transaction != null)
                _transaction.Rollback();

            Finally();
        }

        private void Finally()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
                _connection.Close();
        }

        public void Dispose()
        {
            if (_transaction != null)
                _transaction.Dispose();

            if (_command != null)
                _command.Dispose();

            if (_connection != null)
            {
                if (_connection.State == ConnectionState.Open) 
                    _connection.Close();
                _connection.Dispose();
            }
        }
    }
}
