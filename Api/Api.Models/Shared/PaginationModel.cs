namespace Api.Models.Shared
{
    public class PaginationModel
    {
        public int Size { get; set; }

        public int Page { get; set; }

        public string SortElement { get; set; }

        public bool SortDesc { get; set; }

        public string FilterElement { get; set; }

        public string FilterValue { get; set; }
    }
}
