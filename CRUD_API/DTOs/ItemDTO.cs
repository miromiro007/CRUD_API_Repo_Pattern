using CRUD_API.Models;

namespace CRUD_API.DTOs
{
    public class ItemDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        public ItemDTO(Item item)
        {
            id = item.Id;
            name = item.Name;
            description = item.Description;
        }

        public ItemDTO()
        {
        }

    }
}
