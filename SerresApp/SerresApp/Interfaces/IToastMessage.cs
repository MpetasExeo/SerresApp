
using System.Threading.Tasks;

namespace SerresApp.Interfaces
{
    public interface IToastMessage
    {
        Task MakeToastAsync(string message);
        void MakeSnackBarAsync(string message);
    }
}
