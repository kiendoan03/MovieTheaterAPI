using Microsoft.AspNetCore.Mvc.ModelBinding;
using MovieTheaterAPI.Entities;
using System.Text.Json.Serialization;

namespace MovieTheaterAPI.DTOs
{
    public class CastDTO
    {
        //[BindNever]
        [JsonIgnore]
        public int Id { get; set; } 
        public string CastName { get; set; } = null!;
        public string? CastImage { get; set; } = null!;
        public List<MovieDTO> Movies { get;  } = [];
        public ICollection<MovieCastDTO>? MovieCasts { get;  } = new List<MovieCastDTO>();

    }
}
