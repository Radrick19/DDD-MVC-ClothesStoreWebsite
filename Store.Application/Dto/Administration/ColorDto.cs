using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Store.Application.Dto.Administration
{
    public class ColorDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название цвета не может быть пустым")]
        [StringLength(35, ErrorMessage = "Название не может привышать 35 символов")]
        public string Name { get; set; }

        [DisplayName("Hex цвета")]
        [Required(ErrorMessage = "Hex не может быть пустым")]
        [RegularExpression("^#?(([0-9a-fA-F]{2}){3}|([0-9a-fA-F]){3})$", ErrorMessage = "Не соответствует записи Hex")]
        public string Hex { get; set; }

        [DisplayName("Порядок отображения")]
        public int Order { get; set; }
    }
}
