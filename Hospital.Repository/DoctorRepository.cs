using System;
using System.Collections.Generic;
using Hospital.Model;
using Hospital.Repository.Common;
using Npgsql;
using System.Threading.Tasks;
using Hospital.Common;
using System.Text;

namespace Hospital.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly string connectionString = "Server=localhost; Port=5432; Username=postgres; Password=root; Database=\"Hospital\";";

        public async Task<Paging<DoctorModel>> GetDoctorsAsync(Filter filtering, Sorting<DoctorModel> sorting, Paging<DoctorModel> paging)

        {
            List<DoctorModel> doctors = new List<DoctorModel>();

            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
               await conn.OpenAsync();
                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = conn;

                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT \"Id\", \"FirstName\", \"LastName\", \"Dob\", \"SpecializationId\" FROM \"Doctor\"");

                   //filtering
                   if(filtering != null)
                {
                    queryBuilder.Append(" WHERE 1=1 ");


                    if (filtering.Dob != null)
                    {
                        queryBuilder.Append(" AND \"Dob\" = @Dob ");
                        command.Parameters.AddWithValue("@Dob", filtering.Dob);
                    }

                    if(filtering.SpecializationId != null)
                    {
                        queryBuilder.Append(" AND  \"SpecializationId\" = @SpecializationId ");
                        command.Parameters.AddWithValue("@SpecializationId", filtering.SpecializationId);
                    }

                    if (!string.IsNullOrWhiteSpace(filtering.SearchQuery))
                    {
                        queryBuilder.Append(" AND  \"FirstName\" LIKE @SearchQuery ");
                        command.Parameters.AddWithValue("@SearchQuery", filtering.SearchQuery);
                    }

                  
                }
                if (paging != null)
                {
                    queryBuilder.Append(" LIMIT @PageSize OFFSET @Offset");
                    command.Parameters.AddWithValue("@PageSize", paging.PageSize);
                    command.Parameters.AddWithValue("@Offset", (paging.PageNumber - 1) * paging.PageSize);
                }
                //sorting
                if (sorting != null)
                {
                    queryBuilder.Append($" ORDER BY \"{sorting.SortingBy}\" {sorting.IsAscending.ToString().ToUpper()}");
                }

                // paging
                if (paging != null)
                {
                    queryBuilder.Append(" LIMIT @PageSize OFFSET @Offset");
                    command.Parameters.AddWithValue("@PageSize", paging.PageSize);
                    command.Parameters.AddWithValue("@Offset", (paging.PageNumber - 1) * paging.PageSize);
                }

                
                command.CommandText = queryBuilder.ToString();

                NpgsqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    DoctorModel doctor = new DoctorModel
                    {
                        Id = reader.GetGuid(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Dob = reader.GetDateTime(3),
                        SpecializationId = reader.GetGuid(4)
                    };
                    doctors.Add(doctor);
                }

            }
            int totalDoctorsCount = doctors.Count;

            return new Paging<DoctorModel>(doctors, paging.PageNumber, paging.PageSize, totalDoctorsCount);


        }

        public async Task<DoctorModel> GetDoctorByIdAsync(Guid id)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
              await  conn.OpenAsync();
                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = conn;
                command.CommandText = "SELECT \"FirstName\", \"LastName\", \"SpecializationId\" FROM \"Doctor\" WHERE \"Id\" = @Id";
                command.Parameters.AddWithValue("@Id", id);

                NpgsqlDataReader reader = command.ExecuteReader();

                if (await reader.ReadAsync())
                {
                    DoctorModel doctor = new DoctorModel
                    {
                        FirstName = reader.GetString(0),
                        LastName = reader.GetString(1),
                        SpecializationId = reader.GetGuid(2)
                    };
                    reader.Close();
                    return doctor;
                }
                reader.Close();
                return null;
            }
        }

        public async Task<bool> CreateDoctorAsync(DoctorModel doctor)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (NpgsqlCommand command = new NpgsqlCommand())
                    {
                        command.Connection = conn;
                        command.CommandText = "INSERT INTO \"Doctor\" (\"Id\", \"FirstName\", \"LastName\", \"Dob\", \"SpecializationId\") " +
                            "VALUES (@Id, @FirstName, @LastName, @Dob, @SpecializationId)";
                        command.Parameters.AddWithValue("@Id", doctor.Id);
                        command.Parameters.AddWithValue("@FirstName", doctor.FirstName);
                        command.Parameters.AddWithValue("@LastName", doctor.LastName);
                        command.Parameters.AddWithValue("@Dob", doctor.Dob);
                        command.Parameters.AddWithValue("@SpecializationId", doctor.SpecializationId);
                        command.ExecuteNonQuery();
                    }
                }
                return true; 
            }
            catch
            {
                return false; 
            }
        }

        public async Task<bool> UpdateDoctorAsync(Guid id, DoctorModel doctor)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
              await  conn.OpenAsync();
                using (NpgsqlCommand command = new NpgsqlCommand())
                {
                    command.Connection = conn;
                    command.CommandText = "UPDATE \"Doctor\" SET \"FirstName\" = @FirstName, \"LastName\" = @LastName, \"SpecializationId\" = @SpecializationId " +
                        "WHERE \"Id\" = @Id";
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@FirstName", doctor.FirstName);
                    command.Parameters.AddWithValue("@LastName", doctor.LastName);
                    command.Parameters.AddWithValue("@SpecializationId", doctor.SpecializationId);
                    int rowsAffected = await  command.ExecuteNonQueryAsync();

                    return rowsAffected > 0;
                }
            }
        }

        public async Task<bool> DeleteDoctorAsync(Guid id)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
              await  conn.OpenAsync();
                using (NpgsqlCommand command = new NpgsqlCommand())
                {
                    command.Connection = conn;
                    command.CommandText = "DELETE FROM \"Doctor\" WHERE \"Id\" = @Id";
                    command.Parameters.AddWithValue("@Id", id);
                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    return rowsAffected > 0;
                }
            }
        }
    }
}
