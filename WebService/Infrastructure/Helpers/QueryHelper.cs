﻿using System;
using System.Data.SqlClient;
using UserService.Infrastructure.Models;

namespace UserService.Infrastructure.Helpers
{
    public static class QueryHelper
    {
        public static string GetAddUserExpression(User user)
        {
            return $"INSERT INTO Users (Id, FirstName, LastName, Age, Email) VALUES ('{user.Id}', '{user.FirstName}', '{user.LastName}', {user.Age}, '{user.Email}')";
        }

        public static string GetUpdateUserExpression(User user)
        {
            return $"UPDATE Users SET FirstName='{user.FirstName}', LastName='{user.LastName}', Age={user.Age}, Email='{user.Email}' WHERE Id='{user.Id}'";
        }

        public static string GetRemoveUserByIdExpression(string id)
        {
            return $"DELETE FROM Users WHERE Id='{id}'";
        }

        public static string GetCreateDbExpression(string dbName)
        {
            return $"CREATE DATABASE {dbName}";
        }

        public static string GetUseExpression(string name)
        {
            return $"USE {name}";
        }

        public static string GetCreateUserTableExpression()
        {

            return "CREATE TABLE Users (Id uniqueidentifier NOT NULL, FirstName nvarchar(50) NOT NULL, LastName nvarchar(50) NOT NULL, Age int NOT NULL, Email nvarchar(50) NOT NULL, CONSTRAINT PK_Users PRIMARY KEY CLUSTERED (Id ASC))";
        }

        public static string GetIsExistUserEmailExpression(string email, Guid id = default)
        {
            var exceptYourselfExpr = id == default ? string.Empty : $"AND Id != '{id}'";

            return $"SELECT Email from Users WHERE Email = '{email}'{exceptYourselfExpr}";
        }

        public static bool IsAlreadySmthExist(SqlCommand command)
        {
            return command.ExecuteScalar() != null ? true : false;
        }
    }
}