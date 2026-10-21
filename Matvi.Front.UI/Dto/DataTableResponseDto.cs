namespace Matvi.Front.UI.Dto
{
    public class DataTableResponseDto<T>
    {
        public int Draw { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public IReadOnlyList<T> Data { get; set; } = [];
    }

}
