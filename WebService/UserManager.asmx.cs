using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Script.Services;
using System.Web.Services;
using System.Net;
using UserService.Infrastructure.Models;
using UserService.Infrastructure.Helpers;

namespace UserService
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class UserManager : WebService
    {
        private readonly string _connectionString;

        public UserManager()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["defaultConnection"].ConnectionString;
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod]
        public void GetUsers()
        {
            List<User> users = new List<User>();
            string sqlExpression = "SELECT * FROM Users";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    connection.Open();
                    var rdr = command.ExecuteReader();

                    while (rdr.Read())
                    {
                        var user = new User();
                        user.Id = rdr.GetGuid(0);
                        user.FirstName = rdr.GetString(1);
                        user.LastName = rdr.GetString(2);
                        user.Age = rdr.GetInt32(3);
                        user.Email = rdr.GetString(4);

                        users.Add(user);
                    }
                    ResponseHelper.SetSuccessResponse(Context.Response, users);
                }
            }
            catch (ArgumentException ex)
            {
                ResponseHelper.SetErrorResponse(Context.Response, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                ResponseHelper.SetErrorResponse(Context.Response, HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod]
        public void AddUser()
        {
            try
            {
                var user = SerializeHelper.GetDeserializedObjectFromBody<User>(Context.Request.InputStream);

                if (user == null || !user.IsValid())
                {
                    throw new ArgumentException($"Invalid user model");
                }

                user.Id = Guid.NewGuid();

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    SqlCommand isExistUserEmailCmd = new SqlCommand(QueryHelper.GetIsExistUserEmailExpression(user.Email), connection);
                    if (QueryHelper.IsAlreadySmthExist(isExistUserEmailCmd))
                    {
                        throw new ArgumentException($"User with {user.Email} already exist");
                    }

                    SqlCommand addUserCmd = new SqlCommand(QueryHelper.GetAddUserExpression(user), connection);
                    var result = addUserCmd.ExecuteNonQuery();

                    if (result == default)
                    {
                        throw new ArgumentException("Error when was a trying to add user");
                    }
                }

                ResponseHelper.SetSuccessResponse(Context.Response, user);
            }
            catch (ArgumentException ex)
            {
                ResponseHelper.SetErrorResponse(Context.Response, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                ResponseHelper.SetErrorResponse(Context.Response, HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod]
        public void UpdateUser()
        {
            try
            {
                var user = SerializeHelper.GetDeserializedObjectFromBody<User>(Context.Request.InputStream);

                if (user == null || string.IsNullOrEmpty(user.Id.ToString()) || !user.IsValid())
                {
                    throw new ArgumentException($"Invalid user model");
                }

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    SqlCommand isExistUserEmailCmd = new SqlCommand(QueryHelper.GetIsExistUserEmailExpression(user.Email, user.Id), connection);
                    if (QueryHelper.IsAlreadySmthExist(isExistUserEmailCmd))
                    {
                        throw new ArgumentException($"User with {user.Email} already exist");
                    }

                    SqlCommand updateUserCmd = new SqlCommand(QueryHelper.GetUpdateUserExpression(user), connection);
                    var result = updateUserCmd.ExecuteNonQuery();

                    if (result == default)
                    {
                        throw new ArgumentException("Error when was a trying to update user");
                    }
                }

                ResponseHelper.SetSuccessResponse(Context.Response, user);
            }
            catch (ArgumentException ex)
            {
                ResponseHelper.SetErrorResponse(Context.Response, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                ResponseHelper.SetErrorResponse(Context.Response, HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod]
        public void DeleteUser(string id)
        {
            try
            {
                if (id == default & !Guid.TryParse(id, out _))
                {
                    throw new ArgumentException($"Invalid user model");
                }

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    SqlCommand removeUserCmd = new SqlCommand(QueryHelper.GetRemoveUserByIdExpression(id), connection);
                    var result = removeUserCmd.ExecuteNonQuery();

                    if (result == default)
                    {
                        throw new ArgumentException("Error when was a trying to remove user");
                    }
                }

                ResponseHelper.SetSuccessResponse(Context.Response, string.Empty);
            }
            catch (ArgumentException ex)
            {
                ResponseHelper.SetErrorResponse(Context.Response, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                ResponseHelper.SetErrorResponse(Context.Response, HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}