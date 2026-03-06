namespace CRUD_API.DTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<ItemDTO> Items { get; set; }
    }
}
