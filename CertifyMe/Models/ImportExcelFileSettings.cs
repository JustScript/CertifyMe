namespace CertifyMe.Models
{
    public struct ImportExcelFileSettings
    {
        public int StartRowNum { get; set; }
        public int NameColNum { get; set; }
        public int SurnameColNum { get; set; }
        public int EmailColNum { get; set; }
        public int CourseColNum { get; set; }
        public int CompletedColNum { get; set; }
    }

}