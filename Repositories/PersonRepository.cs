using ApiEnergisa.Models;
using MySqlConnector;
using System;

namespace ApiEnergisa.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private string? _connectionString;

        public PersonRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DataBase");
        }

        public async Task<bool> AddAsync(Person person)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                string qrery = "INSERT INTO person (Name, Email) VALUES (@Name, @Email)";
                MySqlCommand command = new MySqlCommand(qrery, connection);
                command.Parameters.AddWithValue("@Name", person.Name);
                command.Parameters.AddWithValue("@Email", person.Email);

                await connection.OpenAsync();

                int registrosAfetados = await command.ExecuteNonQueryAsync() as int? ?? 0;
                return registrosAfetados > 0;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                string qrery = "DELETE FROM person WHERE Id = @Id";
                MySqlCommand command = new MySqlCommand(qrery, connection);
                command.Parameters.AddWithValue("@Id", id);

                await connection.OpenAsync();

                int registrosAfetados = await command.ExecuteNonQueryAsync() as int? ?? 0;
                return registrosAfetados > 0;
            }
        }

        public async Task<List<Person>> GetAllAsync()
        {
            List<Person> persons = new List<Person>();

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                string qrery = "SELECT Id, Name, Email FROM person";
                MySqlCommand command = new MySqlCommand(qrery, connection);

                await connection.OpenAsync();
                MySqlDataReader reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    Person person = new Person
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Email = reader.GetString(2)
                    };

                    persons.Add(person);
                }
            }

            return persons;
        }

        public async Task<Person?> GetByIdAsync(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                string qrery = "SELECT Id, Name, Email FROM person WHERE Id = @Id";
                MySqlCommand command = new MySqlCommand(qrery, connection);
                command.Parameters.AddWithValue("@Id", id);

                await connection.OpenAsync();
                MySqlDataReader reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    Person person = new Person
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Email = reader.GetString(2)
                    };
                    return person;
                }
            }
            return null;
        }

        public async Task<bool> UpdateAsync(Person person)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                string qrery = "UPDATE person SET Name = @Name, Email = @Email WHERE Id = @Id";
                MySqlCommand command = new MySqlCommand(qrery, connection);
                command.Parameters.AddWithValue("@Id", person.Id);
                command.Parameters.AddWithValue("@Name", person.Name);
                command.Parameters.AddWithValue("@Email", person.Email);

                await connection.OpenAsync();

                int registrosAfetados = await command.ExecuteNonQueryAsync() as int? ?? 0;
                return registrosAfetados > 0;
            }
        }
    }
}
