namespace ConsultorioApi.Services
{
    public class CpfService
    {
        public bool ValidarCpf(string cpf)
        {
            if (string.IsNullOrEmpty(cpf))
                return false;
            if (cpf.Length != 11 || !cpf.All(char.IsDigit))
                return false;
            return true;
        }
    }
}
