using LoginApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Win32;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LoginApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RegistroController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public RegistroController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("registro")]
        public string registro(Registro registro)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection").ToString();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Registro(Senha, Email, Ativo) VALUES (@Senha, @Email, @Ativo)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Senha", registro.Senha);
                    cmd.Parameters.AddWithValue("@Email", registro.Email);
                    cmd.Parameters.AddWithValue("@Ativo", registro.Ativo);

                    con.Open();
                    int i = cmd.ExecuteNonQuery();

                    if (i > 0)
                    {
                        return "Usuario criado";
                    }
                    else
                    {
                        return "Erro!";
                    }
                }
            }
        }
        [HttpPost]
        [Route("login")]
        public string login(Registro registro)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection").ToString();

            string query = "SELECT * FROM Registro WHERE Senha = @Senha AND Email = @Email";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                   
                    cmd.Parameters.AddWithValue("@Senha", registro.Senha);
                    cmd.Parameters.AddWithValue("@Email", registro.Email);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                       
                        return "Usuário logado!";
                    }
                    else
                    {
                   
                        return "Credenciais inválidas!";
                    }
                }
            }
        }


    }
}