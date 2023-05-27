using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Dto.Administration
{
    public class PromoPageDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Заголовок промо страницы должен быть заполнен")]
        [StringLength(60, ErrorMessage = "Заголовок не должен превышать 60 символов")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Описание промо страницы должено быть заполнен")]
        [StringLength(260, ErrorMessage = "Описание не должено превышать 60 символов")]
        public string Description { get; set; }
        public string PictureLink { get; set; }
        [Required(ErrorMessage = "Ссылка на промо страницы должена быть заполнен")]
        public string PromoLink { get; set; }
        public int Order { get; set; }
    }
}
