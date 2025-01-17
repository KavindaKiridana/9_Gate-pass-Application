﻿using GatePass_Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace GatePass.DataAccess.DoVerify
{
    public class DoVerifyRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<DoVerifyRepository> _logger;

        public DoVerifyRepository(IConfiguration configuration, ILogger<DoVerifyRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        public List<VerifyModel> Verify()
        {
            List<VerifyModel> requests = new List<VerifyModel>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "SELECT DISTINCT r.Request_ref_no, r.Sender_service_no, r.In_location_name, r.Out_location_name, " +
                        "r.Receiver_service_no, r.Created_date, r.ExO_service_no, r.Carrier_nic_no, " +
                        "ui.Name " +
                        "FROM Requests r " +
                        "INNER JOIN UserInfo ui ON r.Sender_service_no = ui.ServiceNo " +
                        "WHERE r.Request_ref_no IN (SELECT Request_ref_no FROM Workprogress WHERE Stage_id = 2) ORDER BY Created_date DESC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            VerifyModel request = new VerifyModel
                            {
                                Request_ref_no = reader.GetInt32(0),
                                Sender_service_no = reader.GetString(1),
                                In_location_name = reader.GetString(2),
                                Out_location_name = reader.GetString(3),
                                Receiver_service_no = reader.IsDBNull(4) ? "No Specific Receiver" : reader.GetString(4),
                                Created_date = reader.GetDateTime(5),
                                ExO_service_no = reader.GetString(6),
                                Carrier_nic_no = reader.IsDBNull(7) ? "No Specific Carrier" : reader.GetString(7),
                                Name = reader.GetString(8)
                            };

                            requests.Add(request);
                        }
                    }
                }

                return requests;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving requests for verification.");
                throw;
            }
        }

        public void Approve(int requestRefNo)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string updateQuery = "UPDATE Workprogress SET Stage_id = 5, Update_date = GETDATE(), Progress_status = 'DO Verified', Viewed = 0 WHERE Request_ref_no = @requestRefNo";

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@requestRefNo", requestRefNo);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while approving the request.");
                throw;
            }
        }

        public void Reject(int requestRefNo, string rejectComment)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string updateQuery = "UPDATE Workprogress SET Stage_id = 6, Update_date = GETDATE(), Any_comment = @rejectComment, Progress_status = 'DO Rejected' , Viewed = 0 WHERE Request_ref_no = @requestRefNo";

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@requestRefNo", requestRefNo);
                        command.Parameters.AddWithValue("@rejectComment", rejectComment);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while rejecting the request.");
                throw;
            }
        }

        public List<VerifyModel> GetDetailsById(int id)
        {
            List<VerifyModel> itemsList = new List<VerifyModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT i.Item_id, i.Item_serial_no, i.Item_name, i.Item_description, i.Returnable_status, " +
                            " i.Request_ref_no,  i.Attaches " +
                            "FROM Items i " +

                            "WHERE i.Request_ref_no = @id";

                try
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                VerifyModel request = new VerifyModel
                                {
                                    Item_id = reader.GetInt32(0), // Include Item_id
                                    Item_serial_no = reader.GetString(1),
                                    Item_name = reader.GetString(2),
                                    Item_Description = reader.GetString(3),
                                    Returnable_status = reader.GetString(4),
                                    Request_ref_no = reader.GetInt32(5), // Include Request_ref_no
                                    Attaches = reader["Attaches"] as byte[],
                                };

                                itemsList.Add(request);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while retrieving item details.");
                    // Handle the error as needed, e.g., return an error view or redirect to an error page.
                }
            }

            return itemsList;
        }

        public List<VerifyModel> GetRequestsByStageId(int stageId)
        {
            List<VerifyModel> requests = new List<VerifyModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
            SELECT DISTINCT
                r.Request_ref_no, 
                r.Sender_service_no, 
                r.In_location_name, 
                r.Out_location_name, 
                r.Receiver_service_no, 
                r.Created_date, 
                r.ExO_service_no, 
                r.Carrier_nic_no, 
                ui.Name 
            FROM 
                Requests r
                INNER JOIN UserInfo ui ON r.Sender_service_no = ui.ServiceNo
            WHERE 
                r.Request_ref_no IN (SELECT Request_ref_no FROM Workprogress WHERE Stage_id = @stageId)";

                try
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@stageId", stageId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                VerifyModel request = new VerifyModel
                                {
                                    Request_ref_no = reader.GetInt32(0),
                                    Sender_service_no = reader.GetString(1),
                                    In_location_name = reader.GetString(2),
                                    Out_location_name = reader.GetString(3),
                                    Receiver_service_no = reader.IsDBNull(4) ? "No Specific Receiver" : reader.GetString(4),
                                    Created_date = reader.GetDateTime(5),
                                    ExO_service_no = reader.GetString(6),
                                    Carrier_nic_no = reader.IsDBNull(7) ? "No Specific Carrier" : reader.GetString(7),
                                    Name = reader.GetString(8)
                                };

                                requests.Add(request);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"An error occurred while retrieving requests with stage_id {stageId}.");
                    throw;
                }
            }

            return requests;
        }
    }
}