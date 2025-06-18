using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using W04_Phase_01.Models;

namespace W04_Phase_01.DAL
{
	public class DALPerson
	{
		private readonly string _connectionString;

		public DALPerson(IConfiguration config)
		{
			_connectionString = config.GetConnectionString("DefaultConnection");
		}

		public int InsertPerson(Person person)
		{
			int insertedId = -1;
			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				string sql = @"
                    INSERT INTO Person (FName, LName, Email, Phone, Address, UserName, Password)
                    VALUES (@FName, @LName, @Email, @Phone, @Address, @UserName, @Password);
                    SELECT SCOPE_IDENTITY();";

				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.AddWithValue("@FName", person.FName);
				cmd.Parameters.AddWithValue("@LName", person.LName);
				cmd.Parameters.AddWithValue("@Email", person.Email);
				cmd.Parameters.AddWithValue("@Phone", person.Phone ?? (object)DBNull.Value);
				cmd.Parameters.AddWithValue("@Address", person.Address ?? (object)DBNull.Value);
				cmd.Parameters.AddWithValue("@UserName", person.UserName);
				cmd.Parameters.AddWithValue("@Password", person.Password);

				conn.Open();
				insertedId = Convert.ToInt32(cmd.ExecuteScalar());
			}
			return insertedId;
		}

		public Person GetPersonById(int id)
		{
			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				string sql = "SELECT * FROM Person WHERE PersonId = @Id"; // Use PersonId not Id
				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.AddWithValue("@Id", id);

				conn.Open();
				using (SqlDataReader reader = cmd.ExecuteReader())
				{
					if (reader.Read())
					{
						return new Person
						{
							PersonId = Convert.ToInt32(reader["PersonId"]),
							FName = reader["FName"].ToString(),
							LName = reader["LName"].ToString(),
							Email = reader["Email"].ToString(),
							Phone = reader["Phone"] != DBNull.Value ? reader["Phone"].ToString() : null,
							Address = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : null,
							UserName = reader["UserName"].ToString(),
							Password = reader["Password"].ToString()
						};
					}
				}
			}
			return null;
		}

		public List<Person> GetAllPersons()
		{
			var persons = new List<Person>();

			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				string sql = "SELECT * FROM Person";
				SqlCommand cmd = new SqlCommand(sql, conn);
				conn.Open();

				using (SqlDataReader reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						persons.Add(new Person
						{
							PersonId = Convert.ToInt32(reader["PersonId"]),
							FName = reader["FName"].ToString(),
							LName = reader["LName"].ToString(),
							Email = reader["Email"].ToString(),
							Phone = reader["Phone"] != DBNull.Value ? reader["Phone"].ToString() : null,
							Address = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : null,
							UserName = reader["UserName"].ToString(),
							Password = reader["Password"].ToString()
						});
					}
				}
			}

			return persons;
		}
	}
}
