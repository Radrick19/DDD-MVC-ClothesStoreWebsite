using Microsoft.AspNetCore.Mvc;
using Store.Domain.Models.ProductEntities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Application.Dto.Administration
{
    public class SubcategoryDto
    {
        public int Id { get; set; }

        [DisplayName("Категория")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public CategoryDto Category { get; set; }

        public string Name { get; set; }

        [Required(ErrorMessage = "Название подкатегории должно быть заполнено")]
        [StringLength(60, ErrorMessage = "Название не должно превышать 60 символов")]
        public string DisplayName { get; set; }

        [Required(ErrorMessage = "Описание подкатегории должно быть заполнено")]
        [StringLength(300, ErrorMessage = "Описание не должно превышать 300 символов")]
        public string Description { get; set; }

        [DisplayName("Возможность возврата")]
        public bool CanReturn { get; set; }

        [DisplayName("Порядок отображения")]
        public int Order { get; set; }
    }
}
