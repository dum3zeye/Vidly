using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vidly.Dtos
{
    public class RentalDto
    {
        public int Id { get; set; }

        public IEnumerable<int> MovieIds { get; set; }

        public int CustomerId { get; set; }
    }
}