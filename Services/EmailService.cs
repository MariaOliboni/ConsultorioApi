namespace ConsultorioApi.Services
{
    public class EmailService
    {
        public bool ValidarEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;
            if (!email.Contains("@") || !email.Contains("."))
                return false;
            return true;
        }
    }
}
