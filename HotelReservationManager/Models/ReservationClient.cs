using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelReservationManager.Models
{
    public class ReservationClient
    {
        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }
    }
}
