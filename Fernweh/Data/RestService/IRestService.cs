using System.Threading.Tasks;
using Fernweh.Models;

namespace Fernweh.Data.RestService
{
    public interface IRestService
    {
        Task<Trip> GetTripAsync(string id);

        Task<Trip> SaveTripAsync(Trip trip, bool isNewItem);

        Task DeleteTripAsync(string id);
    }
}