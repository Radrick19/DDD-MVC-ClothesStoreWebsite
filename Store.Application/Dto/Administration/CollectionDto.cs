using Microsoft.AspNetCore.Mvc;
using Store.Domain.Models.ManyToManyProductEntities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Store.Application.Dto.Administration
{
    public class CollectionDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название коллекции должно быть заполнено")]
        [StringLength(60, ErrorMessage = "Название не должно превышать 60 символов")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Название коллекции должно быть заполнено")]
        [StringLength(60, ErrorMessage = "Название не должно превышать 60 символов")]
        public string DisplayName { get; set; }

        [Required(ErrorMessage = "Описание коллекции должно быть заполнено")]
        [StringLength(240, ErrorMessage = "Описание не должно превышать 240 символов")]
        public string Description { get; set; }

        [DisplayName("Актуальная?")]
        public bool IsActual { get; set; } = true;

        [DisplayName("Порядок отображения")]
        public int Order { get; set; }
    }
}
